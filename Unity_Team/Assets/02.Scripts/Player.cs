using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour, IComparable<Player>
{
    public string Name { get; set; }
    public int Score__ { get; set; }

    public int CompareTo(Player other)
    {
        if (other == null) return 1;

        // Score를 기준으로 정렬
        return this.Score__.CompareTo(other.Score__);
    }
    [Header("Scripts")]
    [SerializeField]
    public Inventory inventory;
    [SerializeField]
    public UIController uIController;
    [SerializeField]
    public Status status;
    [Space(30)]

    public float speed = 6.0f;                   // 캐릭터 이동 속도
    public float gravity = -9.81f;               // 중력 값
    public float jumpHeight = 1.0f;              // 점프 높이
    public int zombileKillCount = 0;             // 좀비킬수
    public int Score = 0;                        // 점수
    public bool exit = false;                    // 탈출 성공여부 결과창에서 쓸거임
    public bool die;                             // 플레이어 사망 확인
    public bool die_check = false;
    public int HideOut_Hp = 100;
    public int dieCount = 0;
    public bool hide_Out_Update;

    private float lastSwingSoundTime = 0f;
    private float lastFootstepSoundTime = 0f;
    public float swingSoundDelay = 0.5f; // swing 사운드 재생 간격
    public float footstepSoundDelay = 0.5f; // footstep 사운드 재생 간격

    public CharacterController controller;
    private Vector3 velocity;                   // 캐릭터의 현재 속도
    private bool isGrounded;                    // 캐릭터가 땅에 있는지 여부

    [Header("RotateCamera")]
    public float mouseSpeed;
    float yRotation;
    float xRotation;

    
    public GameObject cam;

    [SerializeField]
    GameObject Hand;
    public GameObject HandObj;
    Hand_ItemType player_item;
    bool cananime;

    public bool CanMove = true;

    public Animator anime;
    public bool useWeapon;
    public Hand_ItemType hand;
    public R_Click_Button rButton;
    GameObject shutgun;
    int chamber;

    [Header("PV")]
    public PhotonView pv;

    public bool useKichin = false;
    public bool useHeal = false;

    bool canuse = true;

    //CharacterController charator;

    [HideInInspector]
    public Rigidbody rigidbody;

    Vector3 currPos = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    Transform myTr;

    // 피격 이미지
    public int hitLevel = 0;

    [Header("옵저버 UI")]
    public GameObject observePlayer;
    public GameObject Cusor;
    public AudioListener lis;
    [Space(30)]

    public GameObject miniMap;
    public GameObject stateUI;
    public GameObject BaseHP;

    public bool GameOver = false;

    private void Awake()
    {
        lis = GetComponentInChildren<AudioListener>();
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        myTr = GetComponent<Transform>();
        currPos = myTr.position;
        currRot = myTr.rotation;
        //charator = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        SceneManager.sceneLoaded += OnSceneLoad;
        if (!pv.isMine)
        {
           // charator.enabled = false;
            
        }
        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {

        if (pv.isMine)
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 커서를 화면 안으로 고정
            Cursor.visible = false; // 커서 안보이게 하기
           // controller = GetComponent<CharacterController>();
        }
    }
    public Image image; // 알파값을 변경할 이미지
    float targetAlpha = 0.0f; // 목표 알파값 (0: 완전 투명, 1: 완전 불투명)
    float duration = 0.5f; // 알파값 변경에 걸리는 시간
    float imageAlpha;
    [SerializeField]
    GameObject hitimg;
    public IEnumerator FadeImage(float hit)
    {
        hitimg.SetActive(true);
        Color currentColor = image.color;
        float startAlpha = hit;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
            yield return null;
        }

        // 목표 알파값 설정
        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
        hitimg.SetActive(false);
    }

    public void StartFadeCoroutine()
    {
        StartCoroutine(FadeImage(imageAlpha));
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode) // OnSceneLoaded 함수
    {
        if(scene.buildIndex == 3)
        {
            miniMap.transform.GetChild(0).gameObject.SetActive(true);
            miniMap.transform.GetChild(1).gameObject.SetActive(true);
            miniMap.transform.GetChild(2).gameObject.SetActive(true);
            miniMap.transform.GetChild(3).gameObject.SetActive(true);
            miniMap.transform.GetChild(4).gameObject.SetActive(true);
            BaseHP.SetActive(false);
        }
        else if(scene.buildIndex == 2)
        {
            miniMap.transform.GetChild(2).gameObject.SetActive(false);
            miniMap.transform.GetChild(3).gameObject.SetActive(false);
            miniMap.transform.GetChild(4).gameObject.SetActive(false);
            BaseHP.SetActive(true);
        }
    }

    void Update()
    {
        if(PhotonNetwork.isMasterClient && hide_Out_Update == true)
        {
            Hide_Out_Setting hideout = GameObject.Find("BaseController").GetComponent<Hide_Out_Setting>();
            if(hideout != null)
            {
                int k = hideout.levelData.Kitchen;
                int h = hideout.levelData.Medical;
               // hideout.OnButtonPress(k, h, -HideOut_Hp);
                hide_Out_Update = false;
            }

        }

        if(die == true)
        {
            if (die_check == false)
            {
                pv.RPC("Die", PhotonTargets.All);


                die_check = true;
            }
        }

        if(status.Hp <= 100)
        {
            imageAlpha = 0.1f;
        }
        else if (status.Hp <= 50)
        {
            imageAlpha = 0.2f;
        }
        else if (status.Hp <= 20)
        {
            imageAlpha = 0.3f;
        }

        if (pv.isMine)
        {
            if (HandObj != null && HandObj.GetComponent<FeildItem>())
            {
                hand = HandObj.GetComponent<FeildItem>().Type;
            }

            // 버튼 누르고 있으면 공격
            if (Input.GetMouseButton(0) && player_item == Hand_ItemType.Weapon && !inventory.pan.GetActive() && HandObj.GetActive())
            {
                pv.RPC("Anime", PhotonTargets.All, 1);
                anime.SetBool("ATK", true);
            }
            if (Input.GetMouseButtonDown(0) && player_item == Hand_ItemType.Hunger && !inventory.pan.GetActive() && HandObj.GetActive())
            {
                pv.RPC("Anime", PhotonTargets.All, 2);
            }
            else if (Input.GetMouseButtonDown(0) && player_item == Hand_ItemType.thirst &&  !inventory.pan.GetActive() && HandObj.GetActive())
            {
                pv.RPC("Anime", PhotonTargets.All, 3);
            }
            else if (Input.GetMouseButtonDown(0) && player_item == Hand_ItemType.Injector && !inventory.pan.GetActive() && HandObj.GetActive())
            {
                pv.RPC("Anime", PhotonTargets.All, 4);
            }
            else if (Input.GetMouseButtonDown(0) && player_item == Hand_ItemType.Meditkit && !inventory.pan.GetActive() && HandObj.GetActive())
            {
                pv.RPC("Anime", PhotonTargets.All, 5);
            }
            else if (Input.GetMouseButtonDown(0) && player_item == Hand_ItemType.Gun && !inventory.pan.GetActive() && HandObj.GetActive())
            {
                if (shutgun.GetComponent<ShutGun>().chamber > 0)
                {
                    //SoundManager.instance.PlayerSound("Shotgun_shot", transform.position);
                    shutgun.GetComponent<ShutGun>().Fire_Shugun();
                    pv.RPC("Anime", PhotonTargets.All, 10);
                }
            }
            else if (Input.GetKeyDown(KeyCode.R) && player_item == Hand_ItemType.Gun && !inventory.pan.GetActive() && HandObj.GetActive())
            {
                int reload = 0;
                if (shutgun.GetComponent<ShutGun>().chamber < 2)
                {
                    if (inventory.CheckBullet())
                    {
                        shutgun.GetComponent<ShutGun>().Shutgun_Anime(1);
                        reload = 2;
                    }
                    StartCoroutine(Reload(reload));
                }
            }

            // 차징 공격
            if (!inventory.pan.GetActive() && Input.GetMouseButton(1) && player_item == Hand_ItemType.Weapon)
            {
                pv.RPC("ChagingAnime", PhotonTargets.All, uIController.charging);
                //pv.RPC("Anime", PhotonTargets.All, 6);
                //anime.SetFloat("Charging", uIController.charging);
            }
            if(Input.GetMouseButtonUp(1))
            {
                pv.RPC("ChagingAnime", PhotonTargets.All, 0.0f);
            }

            if (Input.GetMouseButtonUp(0))
            {
                pv.RPC("Anime", PhotonTargets.All, 7);
                anime.SetBool("ATK", false);
            }
            //----------------------------------------------------        

            if (CanMove)
            {
                RotateCamera();
                //Move();
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if(status.Stamina > 0)
                {
                    speed = 9f;
                    footstepSoundDelay = 0.3f;
                    status.StaminaUse();
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = 6f;
                footstepSoundDelay = 0.5f;
                
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                OpenBackPack();
                uIController.ChaingeQuick();
                CursurSetting();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                
                OpenSetting();
                CursurSetting();
            }

            // 캐릭터가 땅에 있는지 체크
            isGrounded = controller.isGrounded;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f; // 땅에 있을 때 속도를 초기화하여 바닥에 붙어 있게 합니다.
            }

            // 플레이어의 입력을 받아 이동 방향 설정
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                if (Time.time - lastFootstepSoundTime >= footstepSoundDelay)
                {
                    SoundManager.instance.PlayerSound("PlayerFootstep", transform.position);
                    lastFootstepSoundTime = Time.time;
                }
            }

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * speed * Time.deltaTime);

            // 점프 입력 처리
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // 점프 계산
            }

            // 중력 적용
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }
        else
        {
            //포톤 추가
            //원격 플레이어일 때 수행
            //원격 플레이어의 아바타를 수신받은 위치까지 부드럽게 이동시키자
            myTr.position = Vector3.Lerp(myTr.position, currPos, Time.deltaTime * 10.0f);

            //원격 플레이어의 아바타를 수신받은 각도만큼 부드럽게 회전시키자
            myTr.rotation = Quaternion.Slerp(myTr.rotation, currRot, Time.deltaTime * 10.0f);
        }
    }

    IEnumerator Reload(int delay)
    {
        yield return new WaitForSeconds(2.0f);
        shutgun.GetComponent<ShutGun>().chamber += (int)delay;
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
            //로컬 플레이어의 위치 정보를 송신
            if (stream.isWriting)
        {
            //박싱
            stream.SendNext(myTr.position);
            stream.SendNext(myTr.rotation);
        }
        //원격 플레이어의 위치 정보를 수신
        else
        {
            //언박싱
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }

    }

    [PunRPC]
    public void Anime(int AAA)
    {
        if (AAA == 1 && Time.time - lastSwingSoundTime >= swingSoundDelay)
        {
            anime.SetBool("ATK", true);
            string swingSound = "swing" + UnityEngine.Random.Range(1, 5).ToString();
            SoundManager.instance.PlayerSound(swingSound, transform.position);
            lastSwingSoundTime = Time.time;
        }
        else if(AAA == 2)
        {
            anime.SetTrigger("Use");
            anime.SetInteger("ItemType", 1);
            canuse = false;
        }
        else if (AAA == 3)
        {
            anime.SetTrigger("Use");
            anime.SetInteger("ItemType", 2);
            canuse = false;
        }
        else if(AAA == 4)
        {
            anime.SetTrigger("Use");
            anime.SetInteger("ItemType", 3);
            canuse = false;
        }
        else if (AAA == 5)
        {
            anime.SetTrigger("Use");
            anime.SetInteger("ItemType", 4);
            canuse = false;
        }
        else if (AAA == 6)
        {
            anime.SetFloat("Charging", uIController.charging);
            Debug.Log(uIController.charging);
        }
        else if (AAA == 7)
        {
            anime.SetBool("ATK", false);
        }
        else if (AAA == 7)
        {
            anime.SetBool("ATK", false);
        }
    }

    [PunRPC]
    public void ChagingAnime(float i)
    {
        anime.SetFloat("Charging", i);
    }

    public void Smash()
    {
        if(HandObj.GetComponent<FeildItem>())
        {
            HandObj.GetComponent<FeildItem>().Smash();
        }
    }
    public void SmashEnd()
    {
        if (HandObj.GetComponent<FeildItem>())
        {
            HandObj.GetComponent<FeildItem>().SmashEnd();
        }
    }

    public void CanUse()
    {
        canuse = true;
    }

    void RotateCamera()
    {
        if (pv.isMine)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

            yRotation += mouseX; // 마우스 x축 입력에 따라 수평 회전 값을 조정
            xRotation -= mouseY; // 마우스 y축 입력에 따라 수평 회전 값을 조정

            xRotation = Mathf.Clamp(xRotation, -90f, 80f); //수직 회전 값을 -90~90도로 제한. (위, 아래)
            if(cam != null)
            {
                //cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                transform.rotation = Quaternion.Euler(0, yRotation, 0);
            }
        }
    }

    public void OpenBackPack()
    {
        if (pv.isMine)
        {
            CanMove = !CanMove;
            if(uIController.hideout.GetActive())
            {
                uIController.hideout.SetActive(false);
                return;
            }
            inventory.storage.SetActive(false);
            inventory.pan.SetActive(!CanMove);
            uIController.hideout.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = inventory.pan.GetActive();
        }
    }

    public void CursurSetting()
    {
        if (inventory.pan.GetActive() || uIController.Setting_UI.GetActive())
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
            Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenSetting()
    {
        if (pv.isMine)
        {
            CanMove = !CanMove;
            if (uIController.hideout.GetActive())
            {
                uIController.hideout.SetActive(false);
                return;
            }
            inventory.storage.SetActive(false);
            uIController.Setting_UI.SetActive(!CanMove);
            uIController.hideout.SetActive(false);

            CursurSetting();
            Cursor.visible = uIController.Setting_UI.GetActive();
        }
    }

    public void OpenHideOut()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = !Cursor.visible;
        CanMove = false;
        uIController.hideout.SetActive(true);
    }

    Vector3 itempos;
    Quaternion itemqu;

    bool aaa = false;

    [PunRPC]
    public void HandChainge(int G = -1)
    {
        if (pv.isMine)
        {
            if (G != -1)
            {
                if(!aaa)
                {
                    pv.RPC("ShowHand", PhotonTargets.All, G);
                    aaa = true;
                    return;
                }
                HandObj.SetActive(true);
                pv.RPC("handobjdestroy", PhotonTargets.All);
                itempos = inventory.itemPrefabs[G - 1].GetComponent<DragItem>().model.GetComponent<FeildItem>().HandPos;
                itemqu = Quaternion.Euler(inventory.itemPrefabs[G - 1].GetComponent<DragItem>().model.GetComponent<FeildItem>().HandRo);
                pv.RPC("ShowHand", PhotonTargets.All, G);
            }
            else
            {
                HandObj.SetActive(false);
            }
        }

    }

    [PunRPC]
    void handobjdestroy()
    {
        Destroy(HandObj);
    }

    [PunRPC]
    public void ShowHand(int G = -1)
    {
        string itemname = inventory.itemPrefabs[G - 1].GetComponent<DragItem>().model.name;
        HandObj.SetActive(true);
        GameObject newHandObj = PhotonNetwork.Instantiate(itemname, transform.position, transform.rotation, 0);
        HandObj = newHandObj;
        newHandObj.transform.parent = Hand.transform;
        itempos = newHandObj.GetComponent<FeildItem>().HandPos;
        itemqu = Quaternion.Euler(newHandObj.GetComponent<FeildItem>().HandRo);
        newHandObj.transform.localPosition = itempos;
        newHandObj.transform.localRotation = itemqu;
        newHandObj.GetComponent<FeildItem>().use = true;
        newHandObj.GetComponent<FeildItem>().coll.enabled = false;
        newHandObj.GetComponent<FeildItem>().rigid.isKinematic = true;
        newHandObj.GetComponent<FeildItem>().rigid.useGravity = false;

        newHandObj.name = "newHandObj";

        player_item = newHandObj.GetComponent<FeildItem>().Type;
        if(newHandObj.transform.childCount > 0)
        {
            shutgun = newHandObj.transform.GetChild(0).gameObject;
        }
    }

    public void Destroy()
    {
        if (pv.isMine)
        {
            GameObject _item = inventory.quickslotSpace[uIController.numberPressed - 3].GetChild(0).gameObject;
            rButton.Use(_item);
            canuse = true;
            pv.RPC("handfalse", PhotonTargets.All);
        }
    }
    [PunRPC]
    void handfalse()
    {
        HandObj.SetActive(false);
    }

    
    public void ExitState(bool state)
    {
        pv.RPC("ExiteStateRPC", PhotonTargets.AllBuffered, state);
    }

    [PunRPC]
    void ExiteStateRPC(bool state, PhotonMessageInfo info)
    {
        if (info.sender == pv.owner)
        {
            exit = state;
        }
    }

    public void InvenClear()   // 인벤 비우기 탈출상태거나 다이면 실행
    {
        if (die == true || exit == false)
        {
            Inventory inven = transform.Find("InvenSystem").GetComponent<Inventory>();
            inven.ResetBackPack();
        }
    }

    [PunRPC]
    public void Die()
    {
        // CapsuleCollider coll = GetComponent<CapsulesdCollider>();
        // CharacterController controll = GetComponent<CharacterController>();
        //coll.enabled = false;
        //controll.enabled = false;
        //gameObject.AddComponent<Rigidbody>();

        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        enabled = false;            // 스크립트 비활성화
        ScenesManager scenesManager = GameObject.Find("ScenesManager").GetComponent<ScenesManager>();

        //scenesManager.deathCount++;
        die = true;
        
        StartCoroutine(Player_Die());
    }
    IEnumerator Player_Die()
    {
        yield return new WaitForSeconds(1.0f);
        miniMap.transform.GetChild(0).gameObject.SetActive(false);
        miniMap.transform.GetChild(1).gameObject.SetActive(false);
        miniMap.transform.GetChild(2).gameObject.SetActive(false);
        miniMap.transform.GetChild(3).gameObject.SetActive(false);
        miniMap.transform.GetChild(4).gameObject.SetActive(false);
        stateUI.transform.GetChild(0).gameObject.SetActive(false);
        stateUI.transform.GetChild(1).gameObject.SetActive(false);

        Debug.Log(miniMap.transform.GetChild(2).gameObject);
        Debug.Log(miniMap.transform.GetChild(3).gameObject);
        Debug.Log(miniMap.transform.GetChild(4).gameObject);

        cam.GetComponent<Camera>().enabled = false;
        cam.GetComponent<AudioListener>().enabled = false;
        Cusor.SetActive(false);
        
        Cursor.lockState = CursorLockMode.None;
        observePlayer.SetActive(true);
        RaidManager raidManager = GameObject.Find("RaidManager").GetComponent<RaidManager>();
        raidManager.CheckPlayersDie();
    }

    public void Player_ReSpawn()
    {
        gameObject.SetActive(true);
    }

    bool hitTime = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zombie" && hitTime == true)
        {
            Debug.Log("Hit");
            status.Hit();
            hitTime = false;
            StartCoroutine(HitDelay());
        }
    }

    IEnumerator HitDelay()
    {
        yield return new WaitForSeconds(0.5f);
        hitTime = true;
    }



    [PunRPC]
    public void Hide_Out_HP_Update(int i)
    {
        HideOut_Hp += i;
    }
}
