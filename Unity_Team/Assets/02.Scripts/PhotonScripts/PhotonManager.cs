using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviour
{
    [Header("Photon")]
    public string version = "Ver 0.1.0"; // 게임 버전
    public PhotonLogLevel LogLevel = PhotonLogLevel.ErrorsOnly; // 로그 레벨
    PhotonView pv;
    public GameObject roomItem;        // 방 정보 표시하는 프리팹
    public GameObject scrollContents;  //RoomItem이 차일드로 생성될 Parent 객체
    public GameObject playerInfoPanel;   // 방에 들어갔을때 생성되는 PlayerInfo가 생성될 부모 객체


    [Header("Lobby UI")]
    public GameObject btns; // 메인화면의 버튼들
    public GameObject createRoomPanel; // 방만들기 패널
    public GameObject roomPanel;      //  방 패널
    public GameObject findRoomPanel; // 방 찾기 패널
    public GameObject SetResolutionPanel; // 환경설정 패널
    public Text roomNameText;        // 방 이름 텍스트
    public InputField roomName; // 방 제목 설정 인풋
    public Text gameStartText; // 게임 시작 텍스트
    public GameObject title;   // 게임 제목
    public GameObject titleLine;  // 베경화면 라인


    void Awake()
    {
        if (!PhotonNetwork.connected)  // 2번 연결 안되도록
        {
            PhotonNetwork.ConnectUsingSettings(version);

            PhotonNetwork.logLevel = LogLevel;

            // 마스터가 PhotonNetwork.LoadLevel()을 호출하면,
            // 모든 플레이어가 동일한 레벨을 자동으로 로드
            PhotonNetwork.automaticallySyncScene = true;
        }
    }

    #region 포톤 함수
    void OnJoinedLobby() // 로비에 들어 왔을때
    {
        btns.SetActive(true);
        title.SetActive(true);
        titleLine.SetActive(true);
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("nickName"); // 플레이어 닉네임 설정 
    }

    public void OnClickCreateRoom() // 방생성
    {
        createRoomPanel.SetActive(false); // 방생성 패널비활성화
        btns.SetActive(false);            // 버튼들 비활성화
        title.SetActive(false);
        titleLine.SetActive(false);

        string _roomName = roomName.text;  // 방 이름 설정
        


        if (string.IsNullOrEmpty(roomName.text)) // 방이름 설정 안할시
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

    public void OnJoinedRoom()  // 방에 접속 했을때
    {
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("nickName");  // 포톤 닉네임 설정
        roomNameText.text = PhotonNetwork.room.Name;  // 방 제목 세팅
        roomPanel.SetActive(true); // 방패널 활성화
        title.SetActive(true);    // 제목 활성화
       
        GameObject player =  PhotonNetwork.Instantiate("PlayerInfoItem", Vector3.zero, Quaternion.identity, 0);
        //player.transform.SetParent(null);
        //player.transform.SetParent(playerInfoPanel.transform);
        //player.transform.SetAsLastSibling();


        if (PhotonNetwork.isMasterClient)   // 마스터클라이언트면 게임시작버튼
        {
            gameStartText.text = "게임 시작";
        }
        else                               // 마스터클라이언트가 아니면 준비버튼
        {
            gameStartText.text = "준비";
        }
    }

    public void PlayerReady()   // 플레이어 레디 버튼
    {
        foreach (GameObject playerInfoItem in GameObject.FindGameObjectsWithTag("PlayerInfo"))  // 방에 있는 플레이어 정보
        {
            PlayerInfoItem _playerInfoItem = playerInfoItem.GetComponent<PlayerInfoItem>();
            _playerInfoItem.RPCIsReady();
        }
    }

    void OnLeftRoom() // 방에서 접속 종료됐을 때 호출
    {
        btns.SetActive(true);
        title.SetActive(true);
        titleLine.SetActive(true);
        roomPanel.SetActive(false);
        Debug.Log("quit");
    }

    public void OnClickJoinRandomRoom()  // 빠른 시작 버튼
    {
        PhotonNetwork.JoinRandomRoom();
        findRoomPanel.SetActive(false);
    }

    void OnPhotonRandomJoinFailed()    // 방이 없어서 빠른 시작 못했을때
    {
        findRoomPanel.SetActive(true);
        Debug.Log("No Rooms !!!");
    }

    //생성된 룸 목록이 변경됐을 때 호출되는 콜백 함수 (최초 룸 접속시 호출) (UI 버전에서 사용)
    void OnReceivedRoomListUpdate()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomItem"))  // 방 목록 삭제
        {
            Destroy(obj);
        }

        //Grid Layout Group 컴포넌트의 Constraint Count 값을 증가시킬 변수
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
            roomData.joinBtn.onClick.AddListener(delegate () { OnClickRoomItem(roomData.roomName); });  // 함수 할당
            roomData.joinBtn.onClick.AddListener(delegate () { SoundBtnClick(); });      

            scrollContents.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            scrollContents.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 20);
        }

    }

    void OnClickRoomItem(string roomName)  // 클릭한 방 접속 함수
    {
        PhotonNetwork.JoinRoom(roomName);
        findRoomPanel.SetActive(false);
    }

    #endregion

    #region Onclick 함수

    public void CreateRoomBtn()   // 방 생성 버튼
    {
        createRoomPanel.SetActive(true);
        
    }

    public void CreateRoomCancelBtn()  // 방 생성 취소 버튼
    {
        createRoomPanel.SetActive(false);
        
        roomName.text = string.Empty;
    }

    public void RoomQuitBtn()   // 방 나가기 버튼
    {
        PhotonNetwork.LeaveRoom();
        roomPanel.SetActive(false);
        title.SetActive(true);
    }
   
    public void FindRoomBtn()  // 방 목록 보기 버튼
    {
        findRoomPanel.SetActive(true);
        btns.SetActive(false);
        title.SetActive(false);
        titleLine.SetActive(false);
    }

    public void FindRoomQuitBtn()  // 방 목록 나가기 버튼
    {
        findRoomPanel.SetActive(false);
        btns.SetActive(true);
        title.SetActive(true);
        titleLine.SetActive(true);
    }

    public void SetResolutionBtn()  // 환경 설정 버튼
    {
        SetResolutionPanel.SetActive(true);
        title.SetActive(false);
        title.SetActive(false);
    }

    public void SetResolutionQuitBtn()  // 환경 설정 나가기 버튼
    {
        SetResolutionPanel.SetActive(false);
        title.SetActive(true);
        titleLine.SetActive(true);
    }

    public void GameStartBtn()  // 게임 시작 버튼
    {
        if (PhotonNetwork.isMasterClient)   // 마스터클라이언트면 게임시작버튼
        {
            foreach (GameObject playerInfoItem in GameObject.FindGameObjectsWithTag("PlayerInfo"))
            {
                PlayerInfoItem _playerInfoItem = playerInfoItem.GetComponent<PlayerInfoItem>();
                if (!_playerInfoItem.isReady)
                {
                    return; // 하나라도 준비가 안 된 플레이어가 있으면 종료
                }
            }
            //StartCoroutine(LoadStage(2));
            ScenesManager.instance.LoadStage(2);
        }
        else                          // 마스터클라이언트가 아니면 준비버튼
        {
            PlayerReady();
        }
    }

    
    public void QuitBtn()  // 로비 나가기 버튼
    {
        Application.Quit();
    }

    public void SoundBtnClick()         // 버튼 클릭시 사운드 재생
    {
        SoundManager.instance.BtnClick();
    }

    public void SettingsPanelBtn()
    {
        SettingsManager.instance.settingsPanel.SetActive(true);
    }
    #endregion


}
