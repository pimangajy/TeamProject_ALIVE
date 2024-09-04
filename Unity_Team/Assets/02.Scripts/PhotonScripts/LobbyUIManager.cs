using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject btns; // 메인화면의 버튼들
    public GameObject createRoomPanel; // 방만들기 패널
    public GameObject roomPanel;      //  방 패널
    public GameObject findRoomPanel; // 방 찾기 패널;

    public void CreateRoomBtn()   // 방 생성 버튼
    {
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomCancelBtn()  // 방 생성 취소 버튼
    {
        
        createRoomPanel.SetActive(false);
    }

    public void CheckBtn() // 방 생성 확인 버튼
    {
        createRoomPanel.SetActive(false);
        btns.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void RoomQuitBtn()   // 방 나가기 버튼
    {
        roomPanel.SetActive(false);
        btns.SetActive(true);
    }

    public void FindRoomBtn()  // 방에 참여하기 버튼
    {
        findRoomPanel.SetActive(true);
    }

    public void FindRoomQuitBtn()
    {
        findRoomPanel.SetActive(false);
    }
}
