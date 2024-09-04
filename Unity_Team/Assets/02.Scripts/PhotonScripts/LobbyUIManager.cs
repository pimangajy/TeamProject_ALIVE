using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIManager : MonoBehaviour
{
    public GameObject btns; // ����ȭ���� ��ư��
    public GameObject createRoomPanel; // �游��� �г�
    public GameObject roomPanel;      //  �� �г�
    public GameObject findRoomPanel; // �� ã�� �г�;

    public void CreateRoomBtn()   // �� ���� ��ư
    {
        createRoomPanel.SetActive(true);
    }

    public void CreateRoomCancelBtn()  // �� ���� ��� ��ư
    {
        
        createRoomPanel.SetActive(false);
    }

    public void CheckBtn() // �� ���� Ȯ�� ��ư
    {
        createRoomPanel.SetActive(false);
        btns.SetActive(false);
        roomPanel.SetActive(true);
    }

    public void RoomQuitBtn()   // �� ������ ��ư
    {
        roomPanel.SetActive(false);
        btns.SetActive(true);
    }

    public void FindRoomBtn()  // �濡 �����ϱ� ��ư
    {
        findRoomPanel.SetActive(true);
    }

    public void FindRoomQuitBtn()
    {
        findRoomPanel.SetActive(false);
    }
}
