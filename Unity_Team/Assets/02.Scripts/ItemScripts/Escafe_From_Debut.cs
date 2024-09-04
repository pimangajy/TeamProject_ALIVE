using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Escafe_From_Debut : MonoBehaviour
{
    GameObject Escape_Timer;
    Text Escape_Txt;

    public PhotonView[] pv = new PhotonView[4];

    public int Player_Max_Count;
    int Player_Count = 0;

    [SerializeField]
    float Escape_Time = 5.0f;
    void Start()
    {

    }

    
    void Update()
    {
        Player_Max_Count = PhotonNetwork.room.PlayerCount;

        if(Escape_Timer.GetActive())
        {
            Escape_Time -= Time.deltaTime;
        }

        if (Player_Count == Player_Max_Count && Escape_Time == 0)
        {
            SceneManager.LoadScene("Stage1");
        }
    }

    IEnumerator LoadStage(int level)  // 씬 전환 코루틴
    {
        //씬을 전환하는 동안 포톤 클라우드 서버로부터 네트워크 메시지 수신 중단
        //(Instantiate, RPC 메시지 그리고 모든 네트워크 이벤트를 안받음 )
        //차후 전환된 scene의 초기화 설정 작업이 완료후 이 속성을 true로 변경
        PhotonNetwork.isMessageQueueRunning = false;

        AsyncOperation ao = PhotonNetwork.LoadLevelAsync(level);

        Debug.Log("로딩 중");

        yield return ao;

        Debug.Log("로딩 완료");
    }

    

    void CountDown()
    {
        Escape_Time = 5.0f;
        Escape_Timer.SetActive(true);
    }
}
