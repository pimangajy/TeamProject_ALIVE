//using System.Collections.Generic;
//using UnityEngine;

//public class ZombieSpawner : MonoBehaviour
//{

//    public Transform[] spawnPoints; // 스폰 포인트 배열
//    public int curZombieCount = 0; // 현재 좀비 수
//    public int maxZombieCount = 30; // 최대 좀비 수
//    private float spawnTime = 0.0f; // 스폰 타이머
//    private float zombieSpawnTime = 10f;  // 좀비 스폰 시간

//    PhotonView pv;

//    private void Awake()
//    {
//        pv = GetComponent<PhotonView>();
//    }

//    private void Start()
//    {
//        if (PhotonNetwork.isMasterClient)
//        {
//            for (int i = 0; curZombieCount < maxZombieCount; i++)
//            {
//                PhotonNetwork.Instantiate("Zombie", spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, 0);
//                curZombieCount++;
//            }
//        }
//    }

//    void Update()
//    {
//        spawnTime += Time.deltaTime;

//        // 좀비 스폰 조건
//        if (spawnTime > zombieSpawnTime && PhotonNetwork.isMasterClient && curZombieCount < maxZombieCount)
//        {
//            GameObject newObj = PhotonNetwork.Instantiate("Zombie", spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, 0) as GameObject;
//            spawnTime = 0;
//            curZombieCount++;
//        }
//    }
//}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ZombieSpawner : MonoBehaviour
{
    public Transform[] spawnPoints; // 스폰 포인트 배열
    public int maxZombieCount = 30; // 최대 좀비 수
    private float spawnTime = 0.0f; // 스폰 타이머
    private float zombieSpawnTime = 15f;  // 좀비 스폰 시간
    public List<GameObject> zombiePool; // 좀비 오브젝트 풀
    public int curZombieCount;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        zombiePool = new List<GameObject>();
    }

    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {

            for (int i = 0; i < maxZombieCount; i++)
            {
                GameObject zombie = PhotonNetwork.Instantiate("Zombie", Vector3.zero, Quaternion.identity, 0);  // 좀비생성
                zombie.transform.SetParent(transform);  // 생성된 좀비 부모 설정
                zombie.SetActive(false); // 생성된 좀비를 비활성화
                zombiePool.Add(zombie);  // 리스트에 넣기
                pv.RPC("ZombieActivefalse", PhotonTargets.Others, zombie.GetComponent<PhotonView>().viewID);
                SpawnZombie();
            }
        }
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            spawnTime += Time.deltaTime;

            // 좀비 스폰 조건
            if (spawnTime > zombieSpawnTime && curZombieCount < maxZombieCount)
            {
                SpawnZombie();
                spawnTime = 0;
                curZombieCount++;
            }
        }
    }

    void SpawnZombie()
    {
        for (int i = 0; i < zombiePool.Count; i++)
        {
            GameObject zombie = zombiePool[i];
            if (!zombie.activeInHierarchy)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                SpawnZombie(zombie, spawnPoint.position);
                break;
            }
        }
    }

    void SpawnZombie(GameObject zombie, Vector3 position)   // 좀비 스폰
    {
        zombie.transform.position = position;
        zombie.SetActive(true); // 좀비를 활성화
        zombie.GetComponent<ZombieController>().isDead = false;
        pv.RPC("ZombieActivetrue", PhotonTargets.Others, zombie.GetComponent<PhotonView>().viewID, position);
    }

    [PunRPC]
    void ZombieActivetrue(int viewID, Vector3 position)    // 좀비 스폰 RPC
    {
        GameObject zombie = PhotonView.Find(viewID).gameObject;
        zombie.transform.position = position;
        zombie.SetActive(true); // 좀비를 활성화
        zombie.GetComponent<ZombieController>().isDead = false;
    }

    [PunRPC]
    void ZombieActivefalse(int viewID)   // 좀비 비활성화 RPC
    {
        GameObject zombie = PhotonView.Find(viewID).gameObject;  // 좀비의 포톤뷰 아이디를 찾기
        ZombieController zombieController = zombie.GetComponent<ZombieController>();
        zombie.SetActive(false); // 생성된 좀비를 비활성화
    }

    public void DieZombie(GameObject zombie)   // 좀비가 죽었을떄 호출
    {
        StartCoroutine(DieZombieCor(zombie));
    }

    private IEnumerator DieZombieCor(GameObject zombie)
    {
        yield return new WaitForSeconds(5f); // 5초 대기
        curZombieCount--;        // 현재 좀비수 감소
        zombie.SetActive(false); // 좀비를 비활성화
        pv.RPC("ZombieActivefalse", PhotonTargets.Others, zombie.GetComponent<PhotonView>().viewID);
    }
}


