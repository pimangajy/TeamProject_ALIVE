using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    [HideInInspector]
    // 방 이름
    public string roomName = "";

    // 현재 접속 유저수
    [HideInInspector]
    public int connectPlayer = 0;

    // 룸의 최대 접속자수
    [HideInInspector]
    public int maxPlayers = 0;



    //룸 이름 표시할 Text UI 항목
    public Text RoomNameText;
    public Text ConnectInfoText;

    public Button joinBtn;

    public void DisplayRoomData()
    {
        RoomNameText.text = roomName;
        ConnectInfoText.text = "(" + connectPlayer.ToString() + "/" + maxPlayers.ToString() + ")";
    }
}
