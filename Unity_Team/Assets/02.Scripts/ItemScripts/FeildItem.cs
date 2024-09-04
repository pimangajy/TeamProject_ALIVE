using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand_ItemType
{
    Weapon,
    Hunger,
    thirst,
    miscellaneous,
    Injector,
    Meditkit,
    Gun,
    Bullet
}

public class FeildItem : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;

    public Hand_ItemType Type;

    public GameObject ItemPrefab;

    public Material mat;

    public int matNum;

    // 손 위치
    public Vector3 HandPos;
    // 각도
    public Vector3 HandRo;

    // 장착시 강조 안되게
    public bool use= false;

    // 장착시에만 ItemPlus()함수 발동되게
    public bool F = false;

    public Rigidbody rigid;
    public BoxCollider coll;

    private void Awake()
    {
        rigid = gameObject.GetComponent<Rigidbody>();
    }


    private void Start()
    {
        if(use == true)
        {
            coll.enabled = false;
        }
    }

    private void Update()
    {
        if(use == true)
        {
            transform.root.GetComponent<Player>();
            coll.isTrigger = true;
        }
    }

    [PunRPC]
    public void GetItem()
    {
        PhotonNetwork.Destroy(gameObject);
       // Destroy(gameObject);
    }

    public void Smash()
    {
        coll.enabled = true;
    }
    public void SmashEnd()
    {
        coll.enabled = false;
    }
}
