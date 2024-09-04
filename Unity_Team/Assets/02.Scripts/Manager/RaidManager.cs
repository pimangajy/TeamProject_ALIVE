using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour
{
    PhotonView pv;

    [Header("�÷��̾� ���� ����Ʈ")]
    public Transform[] playerSpawnPos;      // �÷��̾� ���� ����Ʈ

    [Header("Ż�ⱸ")]
    public GameObject[] exits;                 // Ż�ⱸ �迭

    [Header("Ż�� UI")]
    public GameObject escapeUI;
    public Text escapeTimeText;                // Ż�� �ð� �ؽ�Ʈ
    float escapeTime = 10.0f;                  // Ż�� �ð�
    bool allDie = false;
    private StringBuilder sb = new StringBuilder(8);  //
    string exit = "(Ż��)";
    string die = "(���)";



    [Header("���â")]
    public GameObject ScoreBoard;        // ���ھ� ����
    public GameObject[] playerResultInfo;   // �÷��̾� ������ ǥ���� ������Ʈ

    [Header("������")]
    public GameObject observe;

    public int dieCount = 0;

    [Header("Ż�� �̹���")]
    public GameObject Exit_IMG_1;
    public GameObject Exit_IMG_2;
    public GameObject Exit_IMG_3;


    public GameObject[] players; // �÷��̾��
    private Player[] cplayers;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

    }

    void Start()
    {
        dieCount = 0;
        players = GameObject.FindGameObjectsWithTag("Player");  // �÷��̾� ã�Ƽ� �迭�� �Ҵ�
        if (PhotonNetwork.isMasterClient)
        {
            int rand = Random.Range(0, exits.Length);
            pv.RPC("SpawnExit", PhotonTargets.AllBuffered, rand);   // Ż�ⱸ �������� �ϳ��� ����
            pv.RPC("PlayerSpawn", PhotonTargets.AllBuffered, rand);
        }

       

        cplayers = new Player[players.Length];                  
        for (int i = 0; i < players.Length; i++)
        {
            cplayers[i] = players[i].GetComponent<Player>();
        }
    }

  

    public void CheckPlayersDie()
    {
        //// ��� �÷��̾��� die ���¸� Ȯ��
        for (int i = 0; i < cplayers.Length; i++)
        {
            if (!cplayers[i].die) // ����ִ� �÷��̾ ������ ����
            {
                return;
            }
        }
        allDie = true;

        // ��� �÷��̾ �׾��ٸ� Ż�� ����
        StartCoroutine(Escape());
    }

    [PunRPC]
    void SpawnExit(int i)          // Ż�ⱸ ���� rpc
    {
        exits[i].SetActive(true);
    }

    public void ActivateEscape()     // Ż�� UI On
    {
        StartCoroutine(EscapeTimeCount());
        
    }

    public void ActivateObserve()     // ���� UI On
    {
        observe.SetActive(true);
    }

    [PunRPC]
    void PlayerSpawn(int rand)
    {
        StartCoroutine(PlayerSpawnCor(rand));
    }

    IEnumerator PlayerSpawnCor(int rand)
    {
        yield return ScenesManager.instance.ao;
        for (int i = 0; i < players.Length; i++)
        {
            Transform Pos = playerSpawnPos[rand]; // ���� ����Ʈ �������� ����
            players[i].transform.position = Pos.position; // �÷��̾�� ���� ����Ʈ�� �̵�;
        }
        yield return null;
    }



    IEnumerator EscapeTimeCount()        // Ż�� ī��Ʈ�ٿ� �ڷ�ƾ
    {
        escapeUI.SetActive(true);       // Ż�� UI Ű��
        Exit_IMG_1.SetActive(true);     
        Exit_IMG_2.SetActive(true);
        Exit_IMG_3.SetActive(true);

        

        while (escapeTime > 0f)      // �ð� ����
        {
            escapeTime -= Time.deltaTime;

            if (escapeTime <= 0f)
            {
                escapeTime = 0f;
            }

            sb.Clear();
            sb.AppendFormat("{0:F2}", escapeTime);
            escapeTimeText.text = sb.ToString();
            yield return null;
        }
        
       StartCoroutine(Escape());           // Ż�� �ڷ�ƾ ����
    }

    IEnumerator Escape()           // Ż�� �ڷ�ƾ
    {
        int plyaerNum = 0; // for ������ �� �÷��̾� ��
        for (int i = 0; i < players.Length; i++) // �÷��̾��� ����ŭ �ݺ�
        {
            MouseLook _playerCamera = players[i].GetComponentInChildren<MouseLook>();

            if (_playerCamera != null) // null �̸� ���� �÷��̾�
            {
                _playerCamera.specificCamera.enabled = false; // �÷��̾� ī�޶� ����
            }

           string isexit = null; // Ż�� ǥ�� ���ڿ�
            Player player_ = players[i].GetComponent<Player>(); // �÷��̾� ������Ʈ

            player_.miniMap.transform.GetChild(0).gameObject.SetActive(false); // �̴ϸ� ui ����
            player_.miniMap.transform.GetChild(1).gameObject.SetActive(false);

            player_.InvenClear(); // �κ� ���� Ż����°ų� ���̸� ����

            player_.observePlayer.SetActive(false); // ���� UI ����

            if (player_.exit && !player_.die) // Ż���ߴ��� ���ߴ��� üũ
            {
                isexit = exit;
            }
            else
            {
                isexit = die;
                ScenesManager.instance.deathCount++;
            }

            Text[] resutlText = playerResultInfo[plyaerNum].GetComponentsInChildren<Text>(); // �÷��̾� ��� ������ ǥ���� UI�� �ڽ� �ؽ�Ʈ ������Ʈ�� ������
            resutlText[0].text = players[i].GetComponent<PhotonView>().owner.NickName.ToString() + isexit; // 0���� 2���� ���ڿ� ����
            resutlText[1].text = player_.zombileKillCount.ToString();
            resutlText[2].text = player_.Score.ToString();
            plyaerNum++; // ���� �÷��̾� ����
        }

        if (!allDie)
        {
            for (int i = 0; i < exits.Length; i++)              // �ڵ��� ���
            {
                if (exits[i].activeSelf == true)        // Ż�ⱸ�� Ȱ��ȭ�� Ż�ⱸ ã��
                {
                    exits[i].GetComponent<GoCar>().exit = true;
                }
            }
        }

        escapeUI.SetActive(false);      // Ż�� Ÿ�̸� UI ����
        Exit_IMG_1.SetActive(false);
        Exit_IMG_2.SetActive(false);
        Exit_IMG_3.SetActive(false);
        ScoreBoard.SetActive(true);     // ���ھ�� UI �ѱ�

        yield return new WaitForSeconds(5.0f);
        ScenesManager.instance.LoadStage(2);   // �ٽ� ���̽���

        if (PhotonNetwork.isMasterClient)
        {
            // ������ Ŭ���̾�Ʈ���� ���� ������� ī��Ʈ ���� �� �� ����ŭ ���̽� ü�� �پ��
            GameObject master = GameObject.FindGameObjectWithTag("Player").gameObject;
            master.GetComponent<Player>().dieCount = dieCount;
            master.GetComponent<Player>().hide_Out_Update = true;
        }
    }

    public void DieCount()
    {
        dieCount++;
    }
}

