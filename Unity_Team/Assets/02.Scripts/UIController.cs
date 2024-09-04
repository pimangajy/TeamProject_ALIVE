using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [Header("Scripts")]
    [SerializeField]
    Player player;
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    Status status;
    [SerializeField]
    R_Click_Button r_Click_Button;
    [Space(30)]

    //------------------------------------------------

    [SerializeField]
    GameObject canvas;
    [SerializeField]
    public GameObject panel; 
    [SerializeField]
    GameObject Staters;
    [SerializeField]
    GameObject minimap;

    //------------------------------------------------
    // Ÿ�̸�, ������ ����
    // ������
    public GameObject Quick;
    // ������ �̹���ĭ
    public Transform[] Q_Slot = new Transform[5];
    // ��� ������
    public GameObject EQ_Quick;
    // ��� ������ �̹���ĭ
    [SerializeField]
    public Transform[] EQ_Q_Slot = new Transform[2];

    // �� ���� ���� ����
    GameObject[] Slot_Num = new GameObject[5];
    // ���������
    GameObject[] EqSlot_Num = new GameObject[2];
    // �Է��� ���ڸ� ����
    public int numberPressed;
    // �⺻ �÷�
    [SerializeField]
    Color defaultecolor;
    // ���� ������ ĭ
    [SerializeField]
    GameObject select_Slot;

    [SerializeField]
    RectTransform QuickSlot_Panel;
    [SerializeField]
    RectTransform Equipment_Quck_Slot_Panel;
    [SerializeField]
    RectTransform Timer_Panel;

    Vector2 quick_move;         // ��ǥ����
    Vector2 timer_move;
    Vector2 Equipment_Quck_Slot_move;

    Vector2 origin_Q_Pos;       // ���� ��ġ
    Vector2 origin_EQ_Pos;
    Vector2 origin_T_Pos;

    public float UI_Speed;      // �̵� �ӵ�

    bool move_Q = true;         // ��ư Ȯ��
    bool move_Q_1 = true;       // ��� Ȯ��

    bool move_T = true;
    bool move_T_1 = true;

    // Ÿ�̸� ����
    [SerializeField]
    public Text time;
    // ���� �ð�
    [SerializeField]
    public float realTime;
    
    public Slider hide_out_hp;

    [SerializeField]
    Image charging_img;

    public float charging = 0.0f;

    public GameObject S_Panel;
    public bool open_Storage;
    public Text open_Storage_txt;

    PhotonView pv;

    public GameObject hideout;
    public GameObject worktable;
    public GameObject medittable;

    public GameObject Setting_UI;

    
    public Text pingtxt;
    public Image[] ping_img = new Image[3];
    //--------------------------------------------------


    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        //GetObj();
    }

    void Start()
    {
        if (pv.isMine)
        {
        // �� ������ �ڽĵ��� ã�� ���ӿ�����Ʈ �迭�� ����
        for (int i = 0; i < Quick.transform.childCount; i++)
            {
                Slot_Num[i] = Quick.transform.GetChild(i).gameObject;
            }
            // �������� ã��
            //EQ_Quick = GameObject.Find("Player/Canvas/Equipment_Quck_Slot");
            //�� ������ �ڽĵ��� ã�� ���ӿ�����Ʈ �迭�� ����
            for (int i = 0; i < EQ_Quick.transform.childCount; i++)
            {
                EqSlot_Num[i] = EQ_Quick.transform.GetChild(i).gameObject;
            }


            // �ʵ� ������ �̹��� ����
            for (int i = 0; i < 5; i++)
            {
                Q_Slot[i] = Quick.transform.GetChild(i).GetChild(0);
            }
            for (int i = 0; i < 2; i++)
            {
                EQ_Q_Slot[i] = EQ_Quick.transform.GetChild(i).GetChild(0);
            }


            UpdateTimerText();

            origin_Q_Pos = QuickSlot_Panel.anchoredPosition;
            origin_EQ_Pos = Equipment_Quck_Slot_Panel.anchoredPosition;
            origin_T_Pos = Timer_Panel.anchoredPosition;

            Vector2 a = new Vector2(0, -150);
            Vector2 b = new Vector2(-300, 0);
            Vector2 c = new Vector2(-550, -75);
            quick_move = QuickSlot_Panel.anchoredPosition + a;
            timer_move = Timer_Panel.anchoredPosition + b;
            Equipment_Quck_Slot_move = Equipment_Quck_Slot_move + c;

            charging_img.fillAmount = charging;

            S_Panel.SetActive(false); 
        }else
        {
            canvas.SetActive(false);
        }
    }

    void Update()
    {

        if (pv.isMine)
        {
            
            if (Input.GetMouseButton(1))
            {
                charging += Time.deltaTime / 2;
                charging = Mathf.Clamp(charging, 0.0f, 2.0f);
            }
            if (Input.GetMouseButtonUp(1))
            {
                Invoke("ChagingReset", 1.0f);
            }
            charging_img.fillAmount = charging / 2; //(Mathf.Lerp(0, 100, charging)) / 100;

            if (Input.anyKeyDown)
            {
                // �Էµ� Ű�� ���ڿ��� �޾ƿ�
                string input = Input.inputString;

                // �Էµ� ���ڿ��� ����Ű���� Ȯ��
                if (input.Length > 0 && char.IsDigit(input[0]))
                {
                    // ������ ������ ĭ�� ���� �ǵ���
                    if (select_Slot != null)
                    {
                        select_Slot.GetComponent<Image>().color = defaultecolor;
                    }
                    // ����Ű ���� ������ ��ȯ�Ͽ� ����
                    numberPressed = int.Parse(input);

                    if (numberPressed == 1)
                    {
                        select_Slot = EqSlot_Num[0];
                        EqSlot_Num[0].GetComponent<Image>().color = Color.white;
                        SwapHand();

                    }
                    else if (numberPressed == 2)
                    {
                        select_Slot = EqSlot_Num[1];
                        EqSlot_Num[1].GetComponent<Image>().color = Color.white;
                        SwapHand();
                    }
                    else if (numberPressed <= 7)
                    {
                        select_Slot = Slot_Num[numberPressed - 3];
                        Slot_Num[numberPressed - 3].GetComponent<Image>().color = Color.white;
                        SwapHand();
                    }


                } 
            }

            int ping = PhotonNetwork.GetPing();

            pingtxt.text = ping.ToString();

            if(ping <= 80)
            {
                pingtxt.color = Color.green;
                for(int i = 0; i < 3; i++)
                {
                    ping_img[i].enabled = true;
                    ping_img[i].color = Color.green;
                }
            }
            else if(ping <= 100)
            {
                pingtxt.color = Color.yellow;
                for (int i = 0; i < 2; i++)
                {
                    ping_img[i].enabled = true;
                    ping_img[i].color = Color.yellow;
                }
                ping_img[2].enabled = false;
            }
            else
            {
                pingtxt.color = Color.red;
                for (int i = 0; i < 1; i++)
                {
                    ping_img[i].enabled = true;
                    ping_img[i].color = Color.red;
                }
                ping_img[1].enabled = false;
                ping_img[2].enabled = false;
            }
        }

        

        //----------------------------------------------------------------------------------------

        if (pv.isMine)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartCoroutine(Timer());
            }

            if (move_Q && move_Q_1
                && (Input.GetKeyDown(KeyCode.Alpha1)
                || Input.GetKeyDown(KeyCode.Alpha2)
                || Input.GetKeyDown(KeyCode.Alpha3)
                || Input.GetKeyDown(KeyCode.Alpha4)
                || Input.GetKeyDown(KeyCode.Alpha5)
                || Input.GetKeyDown(KeyCode.Alpha6)
                || Input.GetKeyDown(KeyCode.Alpha7)))
            {
                move_Q = false;
                move_Q_1 = false;
                StartCoroutine(QuickSlot());
            }

            if (!move_Q && !move_Q_1)
            {
                // Lert(���� ��ġ, �� ��ġ, �ӵ�)
                QuickSlot_Panel.anchoredPosition = Vector2.Lerp(QuickSlot_Panel.anchoredPosition, quick_move, UI_Speed * Time.deltaTime);
                Equipment_Quck_Slot_Panel.anchoredPosition = Vector2.Lerp(Equipment_Quck_Slot_Panel.anchoredPosition, Equipment_Quck_Slot_move, UI_Speed * Time.deltaTime);
                // �����Ÿ� ���϶�� ��ǥ��ġ ����
                if (Vector2.Distance(QuickSlot_Panel.anchoredPosition, quick_move) < 5f)
                {
                    QuickSlot_Panel.anchoredPosition = quick_move;
                }
                if (Vector2.Distance(Equipment_Quck_Slot_Panel.anchoredPosition, Equipment_Quck_Slot_move) < 5f)
                {
                    Equipment_Quck_Slot_Panel.anchoredPosition = Equipment_Quck_Slot_move;
                }
            }
            else
            {
                QuickSlot_Panel.anchoredPosition = Vector2.Lerp(QuickSlot_Panel.anchoredPosition, origin_Q_Pos, UI_Speed * Time.deltaTime);
                Equipment_Quck_Slot_Panel.anchoredPosition = Vector2.Lerp(Equipment_Quck_Slot_Panel.anchoredPosition, origin_EQ_Pos, UI_Speed * Time.deltaTime);

                if (Vector2.Distance(Equipment_Quck_Slot_Panel.anchoredPosition, origin_Q_Pos) < 5f)
                {
                    QuickSlot_Panel.anchoredPosition = origin_Q_Pos;
                }
                if (Vector2.Distance(Equipment_Quck_Slot_Panel.anchoredPosition, origin_EQ_Pos) < 5f)
                {
                    Equipment_Quck_Slot_Panel.anchoredPosition = origin_EQ_Pos;
                }
            }

            //-----------------------------------------------------------------------------------------------------------------------------------------

            if (move_T && move_T_1 && Input.GetKeyDown(KeyCode.O))
            {
                move_T = false;
                move_T_1 = false;
                StartCoroutine(Timer());
            }
            if (!move_T && !move_T_1)
            {
                Timer_Panel.anchoredPosition = Vector2.Lerp(Timer_Panel.anchoredPosition, timer_move, UI_Speed * Time.deltaTime);

                if (Vector2.Distance(Timer_Panel.anchoredPosition, timer_move) < 5f)
                {
                    Timer_Panel.anchoredPosition = timer_move;
                }
            }
            else
            {
                Timer_Panel.anchoredPosition = Vector2.Lerp(Timer_Panel.anchoredPosition, origin_T_Pos, UI_Speed * Time.deltaTime);

                if (Vector2.Distance(Timer_Panel.anchoredPosition, origin_T_Pos) < 5f)
                {
                    Timer_Panel.anchoredPosition = origin_T_Pos;
                }
            }
            //--------------------------------------------------------------------------------------------------------------------------
            if (realTime > 0)
            {
                realTime -= Time.deltaTime;
                if (realTime < 0)
                {
                    realTime = 0;
                    player.Die();
                }
                UpdateTimerText();
            }
            //--------------------------------------------------------------------------------------------------------------------------

            Staters.SetActive(!panel.GetActive());
            minimap.SetActive(Staters.GetActive());

            if (open_Storage)
                S_Panel.SetActive(true);
            else
                S_Panel.SetActive(false); 
        }

    }

    public void ChagingReset()
    {
        charging = 0.0f;
    }
    public void SwapHand()
    {
        if (pv.isMine)
        {
            Player player = gameObject.transform.parent.gameObject.GetComponent<Player>();
            // Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            PhotonView playerID = player.gameObject.GetComponent<PhotonView>();

            if (numberPressed == 1)
            {
                if (inventory.EquipmentSpace[0].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.EquipmentSpace[0].GetChild(0).GetComponent<Item>().numbering);
                   // player.HandChainge(inventory.EquipmentSpace[0].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                    // player.HandChainge();
                    player.useWeapon = false;
                }
            }
            else if (numberPressed == 2)
            {
                if (inventory.EquipmentSpace[1].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.EquipmentSpace[1].GetChild(0).GetComponent<Item>().numbering);
                    //player.HandChainge(inventory.EquipmentSpace[1].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                   // player.HandChainge();
                    player.useWeapon = false;
                }
            }
            else if (numberPressed == 3)
            {
                if (inventory.quickslotSpace[0].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.quickslotSpace[0].GetChild(0).GetComponent<Item>().numbering);
                   // player.HandChainge(inventory.quickslotSpace[0].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                    // player.HandChainge();
                    player.useWeapon = false;
                }
            }
            else if (numberPressed == 4)
            {
                if (inventory.quickslotSpace[1].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.quickslotSpace[1].GetChild(0).GetComponent<Item>().numbering);
                   // player.HandChainge(inventory.quickslotSpace[1].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                    // player.HandChainge();
                    player.useWeapon = false;
                }
            }
            else if (numberPressed == 5)
            {
                if (inventory.quickslotSpace[2].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.quickslotSpace[2].GetChild(0).GetComponent<Item>().numbering);
                    //player.HandChainge(inventory.quickslotSpace[2].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                    // player.HandChainge();
                    player.useWeapon = false;
                }
            }
            else if (numberPressed == 6)
            {
                if (inventory.quickslotSpace[3].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.quickslotSpace[3].GetChild(0).GetComponent<Item>().numbering);
                   // player.HandChainge(inventory.quickslotSpace[3].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                    // player.HandChainge();
                    player.useWeapon = false;
                }
            }
            else if (numberPressed == 7)
            {
                if (inventory.quickslotSpace[4].transform.childCount > 0)
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, inventory.quickslotSpace[4].GetChild(0).GetComponent<Item>().numbering);
                    //player.HandChainge(inventory.quickslotSpace[4].GetChild(0).GetComponent<Item>().numbering);
                    player.useWeapon = true;
                }
                else
                {
                    playerID.RPC("HandChainge", PhotonTargets.AllBuffered, -1);
                    // player.HandChainge();
                    player.useWeapon = false;
                }
            } 
        }
    }

    IEnumerator QuickSlot()
    {
        if (pv.isMine)
        {
            yield return new WaitForSeconds(3.0f);
            move_Q = true;                         // �� �����Դ��� Ȯ�� 
            yield return new WaitForSeconds(1.5f);
            move_Q_1 = true;                       // �� �ö󰬴��� Ȯ�� 
        }
    }

    IEnumerator Timer()
    {
        if (pv.isMine)
        {
            yield return new WaitForSeconds(3.0f);
            move_T = true;
            yield return new WaitForSeconds(1.5f);
            move_T_1 = true; 
        }
    }

    // ���� �����԰� �ʵ� ������ �̹��� ����ȭ
    public void ChaingeQuick()
    {
        if (pv.isMine)
        {
            for (int i = 0; i < 2; i++)
            {
                if (inventory.EquipmentSpace[i].childCount == 0)
                {
                    EQ_Q_Slot[i].GetComponent<Image>().sprite = null;
                }
                else if (inventory.EquipmentSpace[i].childCount > 0)
                {
                    EQ_Q_Slot[i].GetComponent<Image>().sprite = inventory.EquipmentSpace[i].GetChild(0).GetComponent<Image>().sprite;
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (inventory.quickslotSpace[i].childCount == 0)
                {
                    Q_Slot[i].GetComponent<Image>().sprite = null;
                }
                else if (inventory.quickslotSpace[i].childCount > 0)
                {
                    Q_Slot[i].GetComponent<Image>().sprite = inventory.quickslotSpace[i].GetChild(0).GetComponent<Image>().sprite;
                }
            } 
        }
    }

    void UpdateTimerText()
    {
        if (pv.isMine)
        {
            time.text = (Mathf.CeilToInt(realTime) / 60).ToString() + " : " + (Mathf.CeilToInt(realTime) % 60);
            // time.text = Mathf.CeilToInt(realTime).ToString(); // �Ҽ��� ���� �� ������ ǥ�� 
        }
    }

    public void GetObj()
    {
        if (pv.isMine)
        {
            // Ÿ�̸� �ؽ�Ʈ ã��
            time = GameObject.Find("Player/Canvas/Timer/Text").GetComponent<Text>();

            // ��¡ �̹���
            charging_img = GameObject.Find("Player/Canvas/Charging").GetComponent<Image>();

            // ������, Ÿ�̸� �г�
            QuickSlot_Panel = GameObject.Find("Player/Canvas/Quck_Slot").GetComponent<RectTransform>();
            Equipment_Quck_Slot_Panel = GameObject.Find("Player/Canvas/Equipment_Quck_Slot").GetComponent<RectTransform>();
            Timer_Panel = GameObject.Find("Player/Canvas/Timer").GetComponent<RectTransform>();

            // â�� ���� �г�
            S_Panel = GameObject.Find("Player/Canvas/Storage_Panel");
            // HP, Stamina �������ͽ� �г�
            Staters = GameObject.Find("Player/Canvas/Staters_Slot");
            // �κ� �г�
            panel = GameObject.Find("Player/Canvas/Panel");
            // �̴ϸ�
            minimap = GameObject.Find("Player/Canvas/Minimap");

            // �������� ã��
            Quick = GameObject.Find("Player/Canvas/Quck_Slot");
            //�� ������ �ڽĵ��� ã�� ���ӿ�����Ʈ �迭�� ����
            for (int i = 0; i < Quick.transform.childCount; i++)
            {
                Slot_Num[i] = Quick.transform.GetChild(i).gameObject;
            }
            // �������� ã��
            EQ_Quick = GameObject.Find("Player/Canvas/Equipment_Quck_Slot");
            //�� ������ �ڽĵ��� ã�� ���ӿ�����Ʈ �迭�� ����
            for (int i = 0; i < EQ_Quick.transform.childCount; i++)
            {
                EqSlot_Num[i] = EQ_Quick.transform.GetChild(i).gameObject;
            } 
        }
    }
}
