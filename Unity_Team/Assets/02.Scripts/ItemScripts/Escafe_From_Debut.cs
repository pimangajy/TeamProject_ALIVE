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

    IEnumerator LoadStage(int level)  // �� ��ȯ �ڷ�ƾ
    {
        //���� ��ȯ�ϴ� ���� ���� Ŭ���� �����κ��� ��Ʈ��ũ �޽��� ���� �ߴ�
        //(Instantiate, RPC �޽��� �׸��� ��� ��Ʈ��ũ �̺�Ʈ�� �ȹ��� )
        //���� ��ȯ�� scene�� �ʱ�ȭ ���� �۾��� �Ϸ��� �� �Ӽ��� true�� ����
        PhotonNetwork.isMessageQueueRunning = false;

        AsyncOperation ao = PhotonNetwork.LoadLevelAsync(level);

        Debug.Log("�ε� ��");

        yield return ao;

        Debug.Log("�ε� �Ϸ�");
    }

    

    void CountDown()
    {
        Escape_Time = 5.0f;
        Escape_Timer.SetActive(true);
    }
}
