using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RaidManager : MonoBehaviour
{
    PhotonView pv;

    [Header("플레이어 스폰 포인트")]
    public Transform[] playerSpawnPos;      // 플레이어 스폰 포인트

    [Header("탈출구")]
    public GameObject[] exits;                 // 탈출구 배열

    [Header("탈출 UI")]
    public GameObject escapeUI;
    public Text escapeTimeText;                // 탈출 시간 텍스트
    float escapeTime = 10.0f;                  // 탈출 시간
    bool allDie = false;
    private StringBuilder sb = new StringBuilder(8);  //
    string exit = "(탈출)";
    string die = "(사망)";



    [Header("결과창")]
    public GameObject ScoreBoard;        // 스코어 보드
    public GameObject[] playerResultInfo;   // 플레이어 정보를 표시할 오브젝트

    [Header("옵저버")]
    public GameObject observe;

    public int dieCount = 0;

    [Header("탈출 이미지")]
    public GameObject Exit_IMG_1;
    public GameObject Exit_IMG_2;
    public GameObject Exit_IMG_3;


    public GameObject[] players; // 플레이어들
    private Player[] cplayers;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

    }

    void Start()
    {
        dieCount = 0;
        players = GameObject.FindGameObjectsWithTag("Player");  // 플레이어 찾아서 배열에 할당
        if (PhotonNetwork.isMasterClient)
        {
            int rand = Random.Range(0, exits.Length);
            pv.RPC("SpawnExit", PhotonTargets.AllBuffered, rand);   // 탈출구 랜덤으로 하나만 스폰
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
        //// 모든 플레이어의 die 상태를 확인
        for (int i = 0; i < cplayers.Length; i++)
        {
            if (!cplayers[i].die) // 살아있는 플레이어가 있으면 리턴
            {
                return;
            }
        }
        allDie = true;

        // 모든 플레이어가 죽었다면 탈출 실행
        StartCoroutine(Escape());
    }

    [PunRPC]
    void SpawnExit(int i)          // 탈출구 스폰 rpc
    {
        exits[i].SetActive(true);
    }

    public void ActivateEscape()     // 탈출 UI On
    {
        StartCoroutine(EscapeTimeCount());
        
    }

    public void ActivateObserve()     // 관전 UI On
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
            Transform Pos = playerSpawnPos[rand]; // 스폰 포인트 랜덤으로 배정
            players[i].transform.position = Pos.position; // 플레이어들 스폰 포인트로 이동;
        }
        yield return null;
    }



    IEnumerator EscapeTimeCount()        // 탈출 카운트다운 코루틴
    {
        escapeUI.SetActive(true);       // 탈출 UI 키기
        Exit_IMG_1.SetActive(true);     
        Exit_IMG_2.SetActive(true);
        Exit_IMG_3.SetActive(true);

        

        while (escapeTime > 0f)      // 시간 감소
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
        
       StartCoroutine(Escape());           // 탈출 코루틴 시작
    }

    IEnumerator Escape()           // 탈출 코루틴
    {
        int plyaerNum = 0; // for 문에서 쓸 플레이어 수
        for (int i = 0; i < players.Length; i++) // 플레이어의 수만큼 반복
        {
            MouseLook _playerCamera = players[i].GetComponentInChildren<MouseLook>();

            if (_playerCamera != null) // null 이면 죽은 플레이어
            {
                _playerCamera.specificCamera.enabled = false; // 플레이어 카메라 끄기
            }

           string isexit = null; // 탈출 표시 문자열
            Player player_ = players[i].GetComponent<Player>(); // 플레이어 컴포넌트

            player_.miniMap.transform.GetChild(0).gameObject.SetActive(false); // 미니맵 ui 끄기
            player_.miniMap.transform.GetChild(1).gameObject.SetActive(false);

            player_.InvenClear(); // 인벤 비우기 탈출상태거나 다이면 실행

            player_.observePlayer.SetActive(false); // 관전 UI 끄기

            if (player_.exit && !player_.die) // 탈출했는지 안했는지 체크
            {
                isexit = exit;
            }
            else
            {
                isexit = die;
                ScenesManager.instance.deathCount++;
            }

            Text[] resutlText = playerResultInfo[plyaerNum].GetComponentsInChildren<Text>(); // 플레이어 결과 정보를 표시할 UI의 자식 텍스트 컴포넌트를 가져옴
            resutlText[0].text = players[i].GetComponent<PhotonView>().owner.NickName.ToString() + isexit; // 0부터 2까지 문자열 세팅
            resutlText[1].text = player_.zombileKillCount.ToString();
            resutlText[2].text = player_.Score.ToString();
            plyaerNum++; // 다음 플레이어 세팅
        }

        if (!allDie)
        {
            for (int i = 0; i < exits.Length; i++)              // 자동차 출발
            {
                if (exits[i].activeSelf == true)        // 탈출구가 활성화된 탈출구 찾기
                {
                    exits[i].GetComponent<GoCar>().exit = true;
                }
            }
        }

        escapeUI.SetActive(false);      // 탈출 타이머 UI 끄고
        Exit_IMG_1.SetActive(false);
        Exit_IMG_2.SetActive(false);
        Exit_IMG_3.SetActive(false);
        ScoreBoard.SetActive(true);     // 스코어보드 UI 켜기

        yield return new WaitForSeconds(5.0f);
        ScenesManager.instance.LoadStage(2);   // 다시 베이스로

        if (PhotonNetwork.isMasterClient)
        {
            // 마스터 클라이언트에게 죽은 사은사람 카운트 전달 후 그 수만큼 베이스 체력 줄어듬
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

