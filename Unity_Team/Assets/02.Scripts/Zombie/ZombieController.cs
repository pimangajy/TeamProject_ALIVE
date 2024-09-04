
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

    private int lastAttackerViewID; // ���������� ������ �÷��̾��� ViewID
    public float detectionRadius = 10f; // ���� ����
    public float attackRadius = 2f; // ���� ���� �Ÿ�
    public float wanderRadius = 5f; // ��ȸ �ݰ�

    public float minWanderTimer = 3f; // �ּ� ��ȸ �ð�
    public float maxWanderTimer = 6f; // �ִ� ��ȸ �ð�

    private float lastSoundTime;
    private float soundTime = 3.0f;

    [Header("���� �Ҹ�")]
    public AudioClip[] atk;

    [Header("�״� �Ҹ�")]
    public AudioClip[] die;

    [Header("�߰� �Ҹ�")]
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
    public bool isDead = false; // ���� ���¸� ��Ÿ���� ����
    

    public float HP = 100f;
    public float ATK = 10f;

    // ������ ���¸� ��Ÿ���� ������ ����
    enum State { Idle, Wandering, Chasing, Attacking, Dying }
    [SerializeField]
    private State currentState = State.Idle; // �ʱ� ���´� Idle

    //��ġ ������ �ۼ����� �� ����� ���� ���� �� �ʱⰪ ����
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
        if (player_ == null) return;  // null üũ �߰�
        addplayer = player_;
        pv.RPC("prcAddPlayer", PhotonTargets.All, player_.GetComponent<PhotonView>().viewID);  // ViewID�� ���� ����ȭ
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

        if (playerObj == null) return;  // null üũ

        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == playerObj)
            {
                return;  // �̹� ����Ʈ�� �ִ� ���
            }
        }

        players.Add(playerObj);

        // ����Ʈ���� null �� ����
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
    // ���� �׾��� �� ȣ��Ǵ� �Լ�
    public void Die()
    {
        Debug.Log("Zombie Die True");
        isDead = true; // ���� ���� ����
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
            isDead = true; // ���� ���� ����
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
        Vector3 newPos = RandomNavSphere(initialPosition, wanderRadius, -1); // ���ο� ��ȸ ������ ����
        agent.SetDestination(newPos); // �׺���̼� ������Ʈ ������ ����
        yield return new WaitForSeconds(rand);
        animator.SetBool("isWandering", false); // ��ȸ �ִϸ��̼� ����
        initialPosition = transform.position;
        agent.speed = 0; // �׺���̼� ������Ʈ ����
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        if (stream.isWriting)
        {
            //�ڽ�
            stream.SendNext(myTr.position);
            stream.SendNext(myTr.rotation);
        }
      
        else
        {
            //��ڽ�
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
        agent.isStopped = false; // �׺���̼� ������Ʈ �̵� �簳
        agent.speed = 3.5f; // �⺻ �ӵ� ����
    }


    public void TakeDamage(float damage, int attackerViewID)
    {
        if (isDead) return;

        HP -= damage;
        lastAttackerViewID = attackerViewID; // ���������� ������ �÷��̾��� ViewID ����

        if (HP <= 0)
        {
            Die();
            animator.SetTrigger("isDead");
        }
    }

    [PunRPC]
    void ZombieKill(int attackerViewID)       // ���� ���� �÷��̾��� ų���� ���� ����
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

    // ������ �׺���̼� ��ġ�� ã�� �Լ�
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = UnityEngine.Random.insideUnitSphere * dist; // �������� ���� �Ÿ� ���� ���� ���� ���� ����
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask); // �׺���̼� ������ ��ġ ã��
        return navHit.position; // ã�� ��ġ ��ȯ
    }




    // ���� ���� �� �ݶ��̴� Ȱ��ȭ
    public BoxCollider[] boxcoll;
    public void Smash()
    {
        for (int i = 0; i < boxcoll.Length; i++)
        {
            boxcoll[i].enabled = true;
        }
    }

    // ���� ���� �� �ݶ��̴� ��Ȱ��ȭ
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
