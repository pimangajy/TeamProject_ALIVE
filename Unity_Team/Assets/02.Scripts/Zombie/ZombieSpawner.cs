//using System.Collections.Generic;
//using UnityEngine;

//public class ZombieSpawner : MonoBehaviour
//{

//    public Transform[] spawnPoints; // ���� ����Ʈ �迭
//    public int curZombieCount = 0; // ���� ���� ��
//    public int maxZombieCount = 30; // �ִ� ���� ��
//    private float spawnTime = 0.0f; // ���� Ÿ�̸�
//    private float zombieSpawnTime = 10f;  // ���� ���� �ð�

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

//        // ���� ���� ����
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
    public Transform[] spawnPoints; // ���� ����Ʈ �迭
    public int maxZombieCount = 30; // �ִ� ���� ��
    private float spawnTime = 0.0f; // ���� Ÿ�̸�
    private float zombieSpawnTime = 15f;  // ���� ���� �ð�
    public List<GameObject> zombiePool; // ���� ������Ʈ Ǯ
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
                GameObject zombie = PhotonNetwork.Instantiate("Zombie", Vector3.zero, Quaternion.identity, 0);  // �������
                zombie.transform.SetParent(transform);  // ������ ���� �θ� ����
                zombie.SetActive(false); // ������ ���� ��Ȱ��ȭ
                zombiePool.Add(zombie);  // ����Ʈ�� �ֱ�
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

            // ���� ���� ����
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

    void SpawnZombie(GameObject zombie, Vector3 position)   // ���� ����
    {
        zombie.transform.position = position;
        zombie.SetActive(true); // ���� Ȱ��ȭ
        zombie.GetComponent<ZombieController>().isDead = false;
        pv.RPC("ZombieActivetrue", PhotonTargets.Others, zombie.GetComponent<PhotonView>().viewID, position);
    }

    [PunRPC]
    void ZombieActivetrue(int viewID, Vector3 position)    // ���� ���� RPC
    {
        GameObject zombie = PhotonView.Find(viewID).gameObject;
        zombie.transform.position = position;
        zombie.SetActive(true); // ���� Ȱ��ȭ
        zombie.GetComponent<ZombieController>().isDead = false;
    }

    [PunRPC]
    void ZombieActivefalse(int viewID)   // ���� ��Ȱ��ȭ RPC
    {
        GameObject zombie = PhotonView.Find(viewID).gameObject;  // ������ ����� ���̵� ã��
        ZombieController zombieController = zombie.GetComponent<ZombieController>();
        zombie.SetActive(false); // ������ ���� ��Ȱ��ȭ
    }

    public void DieZombie(GameObject zombie)   // ���� �׾����� ȣ��
    {
        StartCoroutine(DieZombieCor(zombie));
    }

    private IEnumerator DieZombieCor(GameObject zombie)
    {
        yield return new WaitForSeconds(5f); // 5�� ���
        curZombieCount--;        // ���� ����� ����
        zombie.SetActive(false); // ���� ��Ȱ��ȭ
        pv.RPC("ZombieActivefalse", PhotonTargets.Others, zombie.GetComponent<PhotonView>().viewID);
    }
}


