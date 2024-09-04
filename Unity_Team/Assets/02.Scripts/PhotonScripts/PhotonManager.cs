using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviour
{
    [Header("Photon")]
    public string version = "Ver 0.1.0"; // ���� ����
    public PhotonLogLevel LogLevel = PhotonLogLevel.ErrorsOnly; // �α� ����
    PhotonView pv;
    public GameObject roomItem;        // �� ���� ǥ���ϴ� ������
    public GameObject scrollContents;  //RoomItem�� ���ϵ�� ������ Parent ��ü
    public GameObject playerInfoPanel;   // �濡 ������ �����Ǵ� PlayerInfo�� ������ �θ� ��ü


    [Header("Lobby UI")]
    public GameObject btns; // ����ȭ���� ��ư��
    public GameObject createRoomPanel; // �游��� �г�
    public GameObject roomPanel;      //  �� �г�
    public GameObject findRoomPanel; // �� ã�� �г�
    public GameObject SetResolutionPanel; // ȯ�漳�� �г�
    public Text roomNameText;        // �� �̸� �ؽ�Ʈ
    public InputField roomName; // �� ���� ���� ��ǲ
    public Text gameStartText; // ���� ���� �ؽ�Ʈ
    public GameObject title;   // ���� ����
    public GameObject titleLine;  // ����ȭ�� ����


    void Awake()
    {
        if (!PhotonNetwork.connected)  // 2�� ���� �ȵǵ���
        {
            PhotonNetwork.ConnectUsingSettings(version);

            PhotonNetwork.logLevel = LogLevel;

            // �����Ͱ� PhotonNetwork.LoadLevel()�� ȣ���ϸ�,
            // ��� �÷��̾ ������ ������ �ڵ����� �ε�
            PhotonNetwork.automaticallySyncScene = true;
        }
    }

    #region ���� �Լ�
    void OnJoinedLobby() // �κ� ��� ������
    {
        btns.SetActive(true);
        title.SetActive(true);
        titleLine.SetActive(true);
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("nickName"); // �÷��̾� �г��� ���� 
    }

    public void OnClickCreateRoom() // �����
    {
        createRoomPanel.SetActive(false); // ����� �гκ�Ȱ��ȭ
        btns.SetActive(false);            // ��ư�� ��Ȱ��ȭ
        title.SetActive(false);
        titleLine.SetActive(false);

        string _roomName = roomName.text;  // �� �̸� ����
        


        if (string.IsNullOrEmpty(roomName.text)) // ���̸� ���� ���ҽ�
        {
            _roomName = "ROOM_" + Random.Range(0, 999).ToString("000");
            roomNameText.text = _roomName;
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 4;

        PhotonNetwork.CreateRoom(_roomName, roomOptions, TypedLobby.Default);
        roomName.text = string.Empty;
    }

    public void OnJoinedRoom()  // �濡 ���� ������
    {
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("nickName");  // ���� �г��� ����
        roomNameText.text = PhotonNetwork.room.Name;  // �� ���� ����
        roomPanel.SetActive(true); // ���г� Ȱ��ȭ
        title.SetActive(true);    // ���� Ȱ��ȭ
       
        GameObject player =  PhotonNetwork.Instantiate("PlayerInfoItem", Vector3.zero, Quaternion.identity, 0);
        //player.transform.SetParent(null);
        //player.transform.SetParent(playerInfoPanel.transform);
        //player.transform.SetAsLastSibling();


        if (PhotonNetwork.isMasterClient)   // ������Ŭ���̾�Ʈ�� ���ӽ��۹�ư
        {
            gameStartText.text = "���� ����";
        }
        else                               // ������Ŭ���̾�Ʈ�� �ƴϸ� �غ��ư
        {
            gameStartText.text = "�غ�";
        }
    }

    public void PlayerReady()   // �÷��̾� ���� ��ư
    {
        foreach (GameObject playerInfoItem in GameObject.FindGameObjectsWithTag("PlayerInfo"))  // �濡 �ִ� �÷��̾� ����
        {
            PlayerInfoItem _playerInfoItem = playerInfoItem.GetComponent<PlayerInfoItem>();
            _playerInfoItem.RPCIsReady();
        }
    }

    void OnLeftRoom() // �濡�� ���� ������� �� ȣ��
    {
        btns.SetActive(true);
        title.SetActive(true);
        titleLine.SetActive(true);
        roomPanel.SetActive(false);
        Debug.Log("quit");
    }

    public void OnClickJoinRandomRoom()  // ���� ���� ��ư
    {
        PhotonNetwork.JoinRandomRoom();
        findRoomPanel.SetActive(false);
    }

    void OnPhotonRandomJoinFailed()    // ���� ��� ���� ���� ��������
    {
        findRoomPanel.SetActive(true);
        Debug.Log("No Rooms !!!");
    }

    //������ �� ����� ������� �� ȣ��Ǵ� �ݹ� �Լ� (���� �� ���ӽ� ȣ��) (UI �������� ���)
    void OnReceivedRoomListUpdate()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomItem"))  // �� ��� ����
        {
            Destroy(obj);
        }

        //Grid Layout Group ������Ʈ�� Constraint Count ���� ������ų ����
        int rowCount = 0;

        scrollContents.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 800);


        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            GameObject room = (GameObject)Instantiate(roomItem);

            room.transform.SetParent(scrollContents.transform, false);

            RoomData roomData = room.GetComponent<RoomData>();

            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayers = _room.MaxPlayers;
            roomData.DisplayRoomData();
            roomData.joinBtn.onClick.AddListener(delegate () { OnClickRoomItem(roomData.roomName); });  // �Լ� �Ҵ�
            roomData.joinBtn.onClick.AddListener(delegate () { SoundBtnClick(); });      

            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }

    }

    void OnClickRoomItem(string roomName)  // Ŭ���� �� ���� �Լ�
    {
        PhotonNetwork.JoinRoom(roomName);
        findRoomPanel.SetActive(false);
    }

    #endregion

    #region Onclick �Լ�

    public void CreateRoomBtn()   // �� ���� ��ư
    {
        createRoomPanel.SetActive(true);
        
    }

    public void CreateRoomCancelBtn()  // �� ���� ��� ��ư
    {
        createRoomPanel.SetActive(false);
        
        roomName.text = string.Empty;
    }

    public void RoomQuitBtn()   // �� ������ ��ư
    {
        PhotonNetwork.LeaveRoom();
        roomPanel.SetActive(false);
        title.SetActive(true);
    }
   
    public void FindRoomBtn()  // �� ��� ���� ��ư
    {
        findRoomPanel.SetActive(true);
        btns.SetActive(false);
        title.SetActive(false);
        titleLine.SetActive(false);
    }

    public void FindRoomQuitBtn()  // �� ��� ������ ��ư
    {
        findRoomPanel.SetActive(false);
        btns.SetActive(true);
        title.SetActive(true);
        titleLine.SetActive(true);
    }

    public void SetResolutionBtn()  // ȯ�� ���� ��ư
    {
        SetResolutionPanel.SetActive(true);
        title.SetActive(false);
        title.SetActive(false);
    }

    public void SetResolutionQuitBtn()  // ȯ�� ���� ������ ��ư
    {
        SetResolutionPanel.SetActive(false);
        title.SetActive(true);
        titleLine.SetActive(true);
    }

    public void GameStartBtn()  // ���� ���� ��ư
    {
        if (PhotonNetwork.isMasterClient)   // ������Ŭ���̾�Ʈ�� ���ӽ��۹�ư
        {
            foreach (GameObject playerInfoItem in GameObject.FindGameObjectsWithTag("PlayerInfo"))
            {
                PlayerInfoItem _playerInfoItem = playerInfoItem.GetComponent<PlayerInfoItem>();
                if (!_playerInfoItem.isReady)
                {
                    return; // �ϳ��� �غ� �� �� �÷��̾ ������ ����
                }
            }
            //StartCoroutine(LoadStage(2));
            ScenesManager.instance.LoadStage(2);
        }
        else                          // ������Ŭ���̾�Ʈ�� �ƴϸ� �غ��ư
        {
            PlayerReady();
        }
    }

    
    public void QuitBtn()  // �κ� ������ ��ư
    {
        Application.Quit();
    }

    public void SoundBtnClick()         // ��ư Ŭ���� ���� ���
    {
        SoundManager.instance.BtnClick();
    }

    public void SettingsPanelBtn()
    {
        SettingsManager.instance.settingsPanel.SetActive(true);
    }
    #endregion


}
