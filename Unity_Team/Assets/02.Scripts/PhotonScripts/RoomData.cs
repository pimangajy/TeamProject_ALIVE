using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [HideInInspector]
    // �� �̸�
    public string roomName = "";

    // ���� ���� ������
    [HideInInspector]
    public int connectPlayer = 0;

    // ���� �ִ� �����ڼ�
    [HideInInspector]
    public int maxPlayers = 0;



    //�� �̸� ǥ���� Text UI �׸�
    public Text RoomNameText;
    public Text ConnectInfoText;

    public Button joinBtn;

    public void DisplayRoomData()
    {
        RoomNameText.text = roomName;
        ConnectInfoText.text = "(" + connectPlayer.ToString() + "/" + maxPlayers.ToString() + ")";
    }
}
