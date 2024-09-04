using UnityEngine;
using UnityEngine.UI;
using Photon;

public class PlayerInfoItem : PunBehaviour
{
    Text playerNickName;        // 플레이어 닉네임 텍스트
    GameObject ready;                 // 레디 오브젝트
    Button kickBtn;         // 강퇴 버튼
    private PhotonView pv;
    private GameObject playerInfoPanel;


    public bool isReady = false;

    void Awake()
    {
        pv = GetComponent<PhotonView>();

        playerNickName = GetComponentInChildren<Text>();
        ready = GameObject.Find("ReadyText");
        ready.SetActive(false);

        kickBtn = GameObject.Find("KickButton").GetComponent<Button>();  // 강퇴버튼 레퍼런스
        kickBtn.onClick.AddListener(OnClickKickBtn);                     // 함수 할당


        playerInfoPanel = GameObject.Find("PlayerInfoPanel");
        transform.SetParent(playerInfoPanel.transform, false);
        SetPlayerInfo();

    }

    private void Start()
    {
        if (pv.isMine)
        {
            transform.SetAsLastSibling();
        }
    }

    private void Update()
    {
        
    }

    public void SetPlayerInfo()
    {
        if (playerNickName != null)
        {

            if (pv.owner.IsMasterClient)
            {
                playerNickName.text = "<color=#ff0000>" + pv.owner.NickName + "</color>";
                isReady = true;

            }
            else
            {
                playerNickName.text = pv.owner.NickName;

            }

            if (PhotonNetwork.isMasterClient && !pv.isMine)  // 마스터클라이언트이고 내 객체가 아니면 강퇴 버튼 활성화
            {
                kickBtn.gameObject.SetActive(true);
            }
            else                                            // 마스터 클라이언트가 아니면 강퇴 버튼 비활성화
            {
                kickBtn.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("PlayerInfoItem prefab is missing a Text component.");
        }
    }

    public void OnClickKickBtn()
    {
        PhotonNetwork.CloseConnection(pv.owner);  // 누른 버튼의 주인을 강퇴
    }

    public void RPCIsReady()
    {
        pv.RPC("IsReady", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void IsReady(PhotonMessageInfo info)
    {
        if (info.sender == pv.owner)  // PlayerItemInfo가 내 객채일때 실행
        {
            isReady = !isReady;
            ready.SetActive(isReady);
        }
    }
}
