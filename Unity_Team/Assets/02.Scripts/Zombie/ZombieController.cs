
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class ZombieController : MonoBehaviour, IComparable<ZombieController>
{
    public string Name { get; set; }
    public int Age { get; set; }

    public int CompareTo(ZombieController other)
    {
        if (other == null) return 1;

        return this.Age.CompareTo(other.Age);
    }

    private int lastAttackerViewID; // 마지막으로 공격한 플레이어의 ViewID
    public float detectionRadius = 10f; // 색적 범위
    public float attackRadius = 2f; // 공격 가능 거리
    public float wanderRadius = 5f; // 배회 반경

    public float minWanderTimer = 3f; // 최소 배회 시간
    public float maxWanderTimer = 6f; // 최대 배회 시간

    private float lastSoundTime;
    private float soundTime = 3.0f;

    [Header("공격 소리")]
    public AudioClip[] atk;

    [Header("죽는 소리")]
    public AudioClip[] die;

    [Header("발견 소리")]
    public AudioClip[] find;

    public GameObject player;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    public PhotonView pv;
    [SerializeField]
    private AudioSource source;


    public float timer;
    private Vector3 initialPosition;
    public bool isDead = false; // 죽음 상태를 나타내는 변수
    

    public float HP = 100f;
    public float ATK = 10f;

    // 좀비의 상태를 나타내는 열거형 변수
    enum State { Idle, Wandering, Chasing, Attacking, Dying }
    [SerializeField]
    private State currentState = State.Idle; // 초기 상태는 Idle

    //위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정
    Transform myTr;   
    Vector3 currPos = Vector3.zero;
    Quaternion currRot = Quaternion.identity;

    public List<GameObject> players = new List<GameObject>();
    public float distanceToPlayer;
    public bool move;

    
    private void Awake()
    {
        myTr = GetComponent<Transform>();
        currPos = myTr.position;
        currRot = myTr.rotation;
        HP = 100;
        players.Clear();
    }

    private void OnEnable()
    {
        initialPosition = transform.position;
        myTr = GetComponent<Transform>();
        currPos = myTr.position;
        currRot = myTr.rotation;
    }

    //private void Start()
    //{
    //    initialPosition = transform.position;
    //}



    GameObject addplayer;
    //public void Addplayer(GameObject player_)
    //{
    //    addplayer = player_;
    //    pv.RPC("prcAddPlayer", PhotonTargets.All);
    //}

    public void Addplayer(GameObject player_)
    {
        if (player_ == null) return;  // null 체크 추가
        addplayer = player_;
        pv.RPC("prcAddPlayer", PhotonTargets.All, player_.GetComponent<PhotonView>().viewID);  // ViewID를 통해 동기화
    }

    //[PunRPC]
    //public void prcAddPlayer()
    //{
    //        for (int i = 0; i < players.Count; i++)
    //        {
    //            if (players[i] == addplayer)
    //            {
    //                return;
    //            }
    //        }
    //        players.Add(addplayer);
    //}

    [PunRPC]
    public void prcAddPlayer(int playerViewID)
    {
        GameObject playerObj = PhotonView.Find(playerViewID).gameObject;

        if (playerObj == null) return;  // null 체크

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == playerObj)
            {
                return;  // 이미 리스트에 있는 경우
            }
        }

        players.Add(playerObj);

        // 리스트에서 null 값 제거
        players.RemoveAll(item => item == null);
    }


    GameObject replayer;
    public void Removeplayer(GameObject player_)
    {
        replayer = player_;
        pv.RPC("rpcRemoePlayer", PhotonTargets.All);
    }
    [PunRPC]
    public void rpcRemoePlayer()
    {
        if(players.Count != 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i] == addplayer)
                {
                    players.Remove(replayer);
                    players.Sort();

                    player = null;
                }
            }
        }
    }
    private void Update()
    {
        move = agent.isStopped;

        if (PhotonNetwork.isMasterClient)
        {
            if (isDead == false)
            {
                if (player != null && player.GetComponent<Player>().die == false)
                {
                    distanceToPlayer = Vector3.Distance(transform.position, players[0].transform.position);
                    //pv.RPC("Anime", PhotonTargets.All, 3);
                    animator.SetBool("isChasing", true);
                    animator.SetBool("isWandering", false);

                    if (distanceToPlayer <= attackRadius)
                    {
                        int attack = UnityEngine.Random.Range(1, 4);
                        animator.SetInteger("AttackType", attack);
                        //StartCoroutine(ZombieAttack());
                    }
                    else
                    {
                        //pv.RPC("Anime", PhotonTargets.All, 6);
                        animator.SetInteger("AttackType", 0);
                        animator.SetBool("isChasing", true);
                        animator.SetBool("isWandering", false);
                        Player_Tracking();
                        if (Time.time >= lastSoundTime + soundTime)
                        {
                            FindSound();
                            lastSoundTime = Time.time;
                        }
                    }

                    if (player.gameObject.GetComponent<Player>().die == true)
                    {
                        Debug.Log("Player Kill");
                        replayer = player;
                        pv.RPC("rpcRemoePlayer", PhotonTargets.All);
                        player = null;
                    }
                }

                if (players.Count == 0)
                {
                    timer -= Time.deltaTime;
                    if (timer <= 0)
                    {
                        timer = UnityEngine.Random.Range(2.0f, 5.0f);
                        StartCoroutine(Prowl());
                    }
                    distanceToPlayer = 100.0f;
                    player = null;
                }

                if (players.Count > 0)
                {
                    player = players[0];
                }
            }
        }
        else if (!pv.isMine)
        {

            myTr.position = Vector3.Lerp(myTr.position, currPos, Time.deltaTime * 10.0f);

            myTr.rotation = Quaternion.Slerp(myTr.rotation, currRot, Time.deltaTime * 10.0f);
        }

    }



    public void Player_Tracking()
    {
        agent.speed = 3.5f;
        agent.destination = player.transform.position;
        transform.LookAt(player.transform);
    }
    // 좀비가 죽었을 때 호출되는 함수
    public void Die()
    {
        Debug.Log("Zombie Die True");
        isDead = true; // 죽음 상태 설정
        pv.RPC("ZombieKill", PhotonTargets.AllBuffered, lastAttackerViewID);
        ZombieSpawner spawn = GameObject.Find("ZombieSpawner").GetComponent<ZombieSpawner>();
        spawn.DieZombie(this.gameObject);

    }

    [PunRPC]
    public void Anime(int i)
    {
        if(i == 1)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isWandering", false);
        }
        else if(i ==2)
        {
            animator.SetBool("isChasing", false);
            animator.SetBool("isWandering", true);
        }
        else if (i == 3)
        {
            animator.SetBool("isChasing", true);
            animator.SetBool("isWandering", false);
        }
        else if (i == 4)
        {
            int attack = UnityEngine.Random.Range(1, 4);
            animator.SetInteger("AttackType", attack);
        }
        else if (i == 5)
        {
            animator.SetTrigger("isDead");
            isDead = true; // 죽음 상태 설정
        }
        else if(i == 6)
        {
            animator.SetInteger("AttackType", 0);
            animator.SetBool("isChasing", false);
            animator.SetBool("isWandering", false);
        }
    }

    IEnumerator Prowl()
    {
        animator.SetInteger("AttackType", 0);
        animator.SetBool("isChasing", false);
        animator.SetBool("isWandering", true);
        agent.speed = 3.0f;
        float rand = UnityEngine.Random.Range(1.0f, 4.0f);
        Vector3 newPos = RandomNavSphere(initialPosition, wanderRadius, -1); // 새로운 배회 목적지 설정
        agent.SetDestination(newPos); // 네비게이션 에이전트 목적지 설정
        yield return new WaitForSeconds(rand);
        animator.SetBool("isWandering", false); // 배회 애니메이션 중지
        initialPosition = transform.position;
        agent.speed = 0; // 네비게이션 에이전트 멈춤
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            //박싱
            stream.SendNext(myTr.position);
            stream.SendNext(myTr.rotation);
        }
      
        else
        {
            //언박싱
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }

    }

    public void OnAttackAnimationStart()
    {
        agent.isStopped = true; 
    }
    public void OnAttackAnimationEnd()
    {
        agent.isStopped = false; // 네비게이션 에이전트 이동 재개
        agent.speed = 3.5f; // 기본 속도 설정
    }


    public void TakeDamage(float damage, int attackerViewID)
    {
        if (isDead) return;

        HP -= damage;
        lastAttackerViewID = attackerViewID; // 마지막으로 공격한 플레이어의 ViewID 저장

        if (HP <= 0)
        {
            Die();
            animator.SetTrigger("isDead");
        }
    }

    [PunRPC]
    void ZombieKill(int attackerViewID)       // 나를 죽인 플레이어의 킬수와 점수 증가
    {
        GameObject attacker = PhotonView.Find(attackerViewID).gameObject;
        if (attacker != null)
        {
            Player player = attacker.GetComponent<Player>();
            if (player != null)
            {
                player.zombileKillCount++;
                player.Score += 1000;
            }
        }
    }

    // 무작위 네비게이션 위치를 찾는 함수
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist; // 원점에서 일정 거리 내의 랜덤 방향 벡터 생성
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask); // 네비게이션 가능한 위치 찾기
        return navHit.position; // 찾은 위치 반환
    }




    // 공격 시작 시 콜라이더 활성화
    public BoxCollider[] boxcoll;
    public void Smash()
    {
        for (int i = 0; i < boxcoll.Length; i++)
        {
            boxcoll[i].enabled = true;
        }
    }

    // 공격 종료 시 콜라이더 비활성화
    public void SmashEnd()
    {
        for (int i = 0; i < boxcoll.Length; i++)
        {
            boxcoll[i].enabled = false;
        }

    }

    public void atkSound()
    {
        string atkSound = "ZombieAtk" + UnityEngine.Random.Range(1, 4).ToString();
        SoundManager.instance.ZombieSound(atkSound, transform.position);
    }

    public void DieSound()
    {
        string dieSound = "ZombieDie" + UnityEngine.Random.Range(1, 4).ToString();
        SoundManager.instance.ZombieSound(dieSound, transform.position);
    }

    public void FindSound()
    {
        string findSound = "ZombieFind" + UnityEngine.Random.Range(1, 4).ToString();
        SoundManager.instance.ZombieSound(findSound, transform.position);
    }
}
