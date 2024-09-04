using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    //플레이어의 생성 위치
    
    public Transform[] playerSpawnPos;
    public PhotonView pv;

    private void Awake()
    {
        playerSpawnPos = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        StartCoroutine(CheckAndCreatePlayer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator CheckAndCreatePlayer()
    {
        yield return ScenesManager.instance.ao;
        
        // 태그를 통해 현재 씬에서 플레이어를 찾음
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // 포톤 룸에 있는 플레이어 수와 현재 씬에 있는 플레이어 수를 비교
        if (players.Length < PhotonNetwork.room.PlayerCount)
        {
            // 플레이어를 생성
            GameObject player = PhotonNetwork.Instantiate("Player", playerSpawnPos[Random.Range(0, playerSpawnPos.Length)].position, Quaternion.identity, 0);
        }
        else   // 레이드하고 다시 베이스 씬으로 올때
        {
            for(int i = 0; i < players.Length; i++)
            {
                Transform Pos = playerSpawnPos[Random.Range(0, playerSpawnPos.Length)]; // 스폰 포인트 랜덤으로 배정 
                players[i].transform.position = Pos.position;  // 플레이어들 스폰 포인트로 이동;

                Player player = players[i].GetComponent<Player>();
                player.exit = false;    // 탈출 상태 초기화

                if (player.pv.owner.NickName != PhotonNetwork.player.NickName)  // 자기 객체가 아니면
                {
                    player.cam.GetComponent<Camera>().enabled = false;       // 카메라 키고
                    player.cam.GetComponent<AudioListener>().enabled = false;  // 오디오리스너 키기

                }

                if (player.pv.owner.NickName == PhotonNetwork.player.NickName)  // 자기 객체만 실행
                {
                    player.cam.GetComponent<Camera>().enabled = true;       // 카메라 키고
                    player.cam.GetComponent<AudioListener>().enabled = true;  // 오디오리스너 키기
                    
                }

                if (player.die == true)   // 플레이어가 죽었다면 죽을때 했던 처리들 원상복구
                {
                    player.rigidbody.isKinematic = true;
                    player.rigidbody.useGravity = false;
                    player.enabled = true;
                    player.status.Hp = 100;
                    
                    if (player.pv.owner.NickName == PhotonNetwork.player.NickName)  // 자기 객체만 실행
                    {
                        player.Cusor.SetActive(true);
                        player.zombileKillCount = 0;
                        player.Score = 0;
                        //player.stateUI.SetActive(true);
                        player.stateUI.transform.GetChild(0).gameObject.SetActive(true);
                        player.stateUI.transform.GetChild(1).gameObject.SetActive(true);
                    }

                    Cursor.lockState = CursorLockMode.Locked;

                    player.die = false;
                    player.die_check = false;
                }
            }
        }

        yield return null;
    }
}
