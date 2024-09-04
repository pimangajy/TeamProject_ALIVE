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
    // 타이머, 퀵슬롯 변수
    // 퀵슬롯
    public GameObject Quick;
    // 퀵슬롯 이미지칸
    public Transform[] Q_Slot = new Transform[5];
    // 장비 퀵슬롯
    public GameObject EQ_Quick;
    // 장비 퀵슬롯 이미지칸
    [SerializeField]
    public Transform[] EQ_Q_Slot = new Transform[2];

    // 퀵 슬롯 선택 변수
    GameObject[] Slot_Num = new GameObject[5];
    // 장비퀵슬롯
    GameObject[] EqSlot_Num = new GameObject[2];
    // 입력한 숫자를 저장
    public int numberPressed;
    // 기본 컬러
    [SerializeField]
    Color defaultecolor;
    // 이전 선택한 칸
    [SerializeField]
    GameObject select_Slot;

    [SerializeField]
    RectTransform QuickSlot_Panel;
    [SerializeField]
    RectTransform Equipment_Quck_Slot_Panel;
    [SerializeField]
    RectTransform Timer_Panel;

    Vector2 quick_move;         // 목표지점
    Vector2 timer_move;
    Vector2 Equipment_Quck_Slot_move;

    Vector2 origin_Q_Pos;       // 원래 위치
    Vector2 origin_EQ_Pos;
    Vector2 origin_T_Pos;

    public float UI_Speed;      // 이동 속도

    bool move_Q = true;         // 버튼 확인
    bool move_Q_1 = true;       // 대기 확인

    bool move_T = true;
    bool move_T_1 = true;

    // 타이머 글자
    [SerializeField]
    public Text time;
    // 제한 시간
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
        // 퀵 슬롯의 자식들을 찾아 게임오브젝트 배열에 넣음
        for (int i = 0; i < Quick.transform.childCount; i++)
            {
                Slot_Num[i] = Quick.transform.GetChild(i).gameObject;
            }
            // 퀵슬롯을 찾음
            //EQ_Quick = GameObject.Find("Player/Canvas/Equipment_Quck_Slot");
            //퀵 슬롯의 자식들을 찾아 게임오브젝트 배열에 넣음
            for (int i = 0; i < EQ_Quick.transform.childCount; i++)
            {
                EqSlot_Num[i] = EQ_Quick.transform.GetChild(i).gameObject;
            }


            // 필드 퀵슬롯 이미지 저장
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
                // 입력된 키를 문자열로 받아옴
                string input = Input.inputString;

                // 입력된 문자열이 숫자키인지 확인
                if (input.Length > 0 && char.IsDigit(input[0]))
                {
                    // 이전에 선택한 칸은 색을 되돌림
                    if (select_Slot != null)
                    {
                        select_Slot.GetComponent<Image>().color = defaultecolor;
                    }
                    // 숫자키 값을 정수로 변환하여 저장
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
                // Lert(시작 위치, 끝 위치, 속도)
                QuickSlot_Panel.anchoredPosition = Vector2.Lerp(QuickSlot_Panel.anchoredPosition, quick_move, UI_Speed * Time.deltaTime);
                Equipment_Quck_Slot_Panel.anchoredPosition = Vector2.Lerp(Equipment_Quck_Slot_Panel.anchoredPosition, Equipment_Quck_Slot_move, UI_Speed * Time.deltaTime);
                // 일정거리 이하라면 목표위치 고정
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
            move_Q = true;                         // 다 내려왔는지 확인 
            yield return new WaitForSeconds(1.5f);
            move_Q_1 = true;                       // 다 올라갔는지 확인 
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

    // 가방 퀵슬롯과 필드 퀵슬록 이미지 동기화
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
            // time.text = Mathf.CeilToInt(realTime).ToString(); // 소수점 없이 초 단위로 표시 
        }
    }

    public void GetObj()
    {
        if (pv.isMine)
        {
            // 타이머 텍스트 찾음
            time = GameObject.Find("Player/Canvas/Timer/Text").GetComponent<Text>();

            // 차징 이미지
            charging_img = GameObject.Find("Player/Canvas/Charging").GetComponent<Image>();

            // 퀵슬롯, 타이머 패널
            QuickSlot_Panel = GameObject.Find("Player/Canvas/Quck_Slot").GetComponent<RectTransform>();
            Equipment_Quck_Slot_Panel = GameObject.Find("Player/Canvas/Equipment_Quck_Slot").GetComponent<RectTransform>();
            Timer_Panel = GameObject.Find("Player/Canvas/Timer").GetComponent<RectTransform>();

            // 창고 오픈 패널
            S_Panel = GameObject.Find("Player/Canvas/Storage_Panel");
            // HP, Stamina 스테이터스 패널
            Staters = GameObject.Find("Player/Canvas/Staters_Slot");
            // 인벤 패널
            panel = GameObject.Find("Player/Canvas/Panel");
            // 미니맵
            minimap = GameObject.Find("Player/Canvas/Minimap");

            // 퀵슬롯을 찾음
            Quick = GameObject.Find("Player/Canvas/Quck_Slot");
            //퀵 슬롯의 자식들을 찾아 게임오브젝트 배열에 넣음
            for (int i = 0; i < Quick.transform.childCount; i++)
            {
                Slot_Num[i] = Quick.transform.GetChild(i).gameObject;
            }
            // 퀵슬롯을 찾음
            EQ_Quick = GameObject.Find("Player/Canvas/Equipment_Quck_Slot");
            //퀵 슬롯의 자식들을 찾아 게임오브젝트 배열에 넣음
            for (int i = 0; i < EQ_Quick.transform.childCount; i++)
            {
                EqSlot_Num[i] = EQ_Quick.transform.GetChild(i).gameObject;
            } 
        }
    }
}
