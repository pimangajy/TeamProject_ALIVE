using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    //�÷��̾��� ���� ��ġ
    
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
        
        // �±׸� ���� ���� ������ �÷��̾ ã��
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        // ���� �뿡 �ִ� �÷��̾� ���� ���� ���� �ִ� �÷��̾� ���� ��
        if (players.Length < PhotonNetwork.room.PlayerCount)
        {
            // �÷��̾ ����
            GameObject player = PhotonNetwork.Instantiate("Player", playerSpawnPos[Random.Range(0, playerSpawnPos.Length)].position, Quaternion.identity, 0);
        }
        else   // ���̵��ϰ� �ٽ� ���̽� ������ �ö�
        {
            for(int i = 0; i < players.Length; i++)
            {
                Transform Pos = playerSpawnPos[Random.Range(0, playerSpawnPos.Length)]; // ���� ����Ʈ �������� ���� 
                players[i].transform.position = Pos.position;  // �÷��̾�� ���� ����Ʈ�� �̵�;

                Player player = players[i].GetComponent<Player>();
                player.exit = false;    // Ż�� ���� �ʱ�ȭ

                if (player.pv.owner.NickName != PhotonNetwork.player.NickName)  // �ڱ� ��ü�� �ƴϸ�
                {
                    player.cam.GetComponent<Camera>().enabled = false;       // ī�޶� Ű��
                    player.cam.GetComponent<AudioListener>().enabled = false;  // ����������� Ű��

                }

                if (player.pv.owner.NickName == PhotonNetwork.player.NickName)  // �ڱ� ��ü�� ����
                {
                    player.cam.GetComponent<Camera>().enabled = true;       // ī�޶� Ű��
                    player.cam.GetComponent<AudioListener>().enabled = true;  // ����������� Ű��
                    
                }

                if (player.die == true)   // �÷��̾ �׾��ٸ� ������ �ߴ� ó���� ���󺹱�
                {
                    player.rigidbody.isKinematic = true;
                    player.rigidbody.useGravity = false;
                    player.enabled = true;
                    player.status.Hp = 100;
                    
                    if (player.pv.owner.NickName == PhotonNetwork.player.NickName)  // �ڱ� ��ü�� ����
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
