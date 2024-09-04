using UnityEngine;
using UnityEngine.UI;
using Photon;

public class PlayerInfoItem : PunBehaviour
{
    Text playerNickName;        // �÷��̾� �г��� �ؽ�Ʈ
    GameObject ready;                 // ���� ������Ʈ
    Button kickBtn;         // ���� ��ư
    private PhotonView pv;
    private GameObject playerInfoPanel;


    public bool isReady = false;

    void Awake()
    {
        pv = GetComponent<PhotonView>();

        playerNickName = GetComponentInChildren<Text>();
        ready = GameObject.Find("ReadyText");
        ready.SetActive(false);

        kickBtn = GameObject.Find("KickButton").GetComponent<Button>();  // �����ư ���۷���
        kickBtn.onClick.AddListener(OnClickKickBtn);                     // �Լ� �Ҵ�


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

            if (PhotonNetwork.isMasterClient && !pv.isMine)  // ������Ŭ���̾�Ʈ�̰� �� ��ü�� �ƴϸ� ���� ��ư Ȱ��ȭ
            {
                kickBtn.gameObject.SetActive(true);
            }
            else                                            // ������ Ŭ���̾�Ʈ�� �ƴϸ� ���� ��ư ��Ȱ��ȭ
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
        PhotonNetwork.CloseConnection(pv.owner);  // ���� ��ư�� ������ ����
    }

    public void RPCIsReady()
    {
        pv.RPC("IsReady", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    public void IsReady(PhotonMessageInfo info)
    {
        if (info.sender == pv.owner)  // PlayerItemInfo�� �� ��ä�϶� ����
        {
            isReady = !isReady;
            ready.SetActive(isReady);
        }
    }
}
