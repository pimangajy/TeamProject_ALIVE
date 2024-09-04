using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField]
    Player player_;
    [SerializeField]
    Status status;
    [SerializeField]
    R_Click_Button r_Click_Button;
    [Space(30)]


    public GameObject[] itemPrefabs;
    //-------------------------------------------------------

    // 가방
    [SerializeField]
    GameObject FieldBackPack;
    // 가방 공간
    public Transform[] FieldBackPackSpace;
    // 가방 크기
    int FieldBackpacksize;

    //-------------------------------------------------------

    // 창고 가방
    [SerializeField]
    GameObject backPack;
    // 창고 가방 공간
    public Transform[] backPackSpace;
    // 창고 가방 크기
    int backpacksize;

    //-------------------------------------------------------

    // 창고
    [SerializeField]
    public GameObject storage;
    // 창고 공간
    public Transform[] storageSpace;
    // 창고 크기
    int storagesize;

    //-------------------------------------------------------

    // 퀵슬롯
    [SerializeField]
    GameObject quickslot;
    // 퀵슬롯 공간
    public Transform[] quickslotSpace;
    // 퀵슬롯 크기
    int quickslotsize;

    //-------------------------------------------------------

    [SerializeField]
    GameObject Equipment_Slot;
    public Transform[] EquipmentSpace;
    int EquipmentSize;

    //-------------------------------------------------------

    // 가방에 아이템추가시 들어가는거
    public GameObject itemprefab;

    public int[] _items;

    public bool hideOut = true;

    public GameObject pan;

    [Header("PV")]
    public PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        // 창고 가방 할당
        // backPack = GameObject.Find("Player/Canvas/Panel/BackPack/BackPackPanel");

        // 창고 할당
        //  storage = GameObject.Find("Player/Canvas/Panel/Storage/SroragePanel");

        // pan = GameObject.Find("Player/Canvas/Panel");
    }

    void Start()
    {
        if (pv.isMine)
        {
            SetBackPack();
            SetFeildBackPack();
            // SetStorage();
            SetQuickSlot();
            SetEquipmentSlot();
            pan.SetActive(false);
        }else
        {
            pan.SetActive(false);
        }
    }

    //private void FixedUpdate()
    //{
    //    SetFeildBackPack();
    //    if(FeildBackPack != null)
    //        GetData();
    //}

    //public void ItemGet(GameObject prefab)
    //{
    //    pv.RPC("ItemPlus", PhotonTargets.AllBuffered, prefab);
    //}

    [PunRPC]
    // 아이템 흭득시 가방에 이미지를 바꿈
    public void ItemPlus(GameObject prefab)  // 흭득시 스프라이트를 하나 받아 가방에 넣는다
    {
        if (pv.isMine)
        {
            if (hideOut)
            {
                for (int i = 0; i <= backpacksize - 1; i++)
                {
                    if (backPackSpace[i].childCount == 0)
                    {
                        GameObject item = Instantiate(prefab, transform.position, Quaternion.identity);

                        item.transform.SetParent(backPackSpace[i]);
                        item.GetComponent<Image>().sprite = item.GetComponent<Item>().itemIcon;
                        item.GetComponent<Image>().color = Color.white;
                        item.GetComponent<Item>().backPackPos = backPackSpace[i].GetComponent<Inven>().poketNum;

                        _items[i] = item.GetComponent<Item>().numbering;


                        return;
                    }
                }
            }
            else
            {
                for (int i = 0; i <= FieldBackpacksize - 1; i++)
                {
                    if (FieldBackPackSpace[i].childCount == 0)
                    {
                        GameObject item = Instantiate(prefab, transform.position, Quaternion.identity);

                        item.transform.SetParent(FieldBackPackSpace[i]);
                        item.GetComponent<Image>().sprite = item.GetComponent<Item>().itemIcon;
                        item.GetComponent<Image>().color = Color.white;
                        item.GetComponent<Item>().backPackPos = FieldBackPackSpace[i].GetComponent<Inven>().poketNum;

                        _items[i] = item.GetComponent<Item>().numbering;


                        return;
                    }
                }
            }
        }

    }

    // 사망시 가방 초기화
    public void ResetBackPack()
    {
        for (int i = 0; i < FieldBackpacksize; i++)
        {
            if (FieldBackPackSpace[i].transform.childCount > 0)
            {
                Destroy(FieldBackPackSpace[i].GetChild(0).gameObject);
            }
        }
        for(int i = 0; i < quickslotsize; i++)
        {
            if (quickslotSpace[i].transform.childCount > 0)
            {
                Destroy(quickslotSpace[i].GetChild(0).gameObject);
            }
        }
        for (int i = 0; i < EquipmentSize; i++)
        {
            if (EquipmentSpace[i].transform.childCount > 0)
            {
                Destroy(EquipmentSpace[i].GetChild(0).gameObject);
            }
        }
    }

    // 총알 확인
    public bool CheckBullet()
    {
        for (int i = 0; i < FieldBackpacksize; i++)
        {
            if (FieldBackPackSpace[i].transform.childCount > 0 && FieldBackPackSpace[i].GetChild(0).GetComponent<DragItem>().numbering == 20)
            {
                Destroy(FieldBackPackSpace[i].GetChild(0).gameObject);
                return true;
            }
        }

        return false;
    }


    // 창고로 이동
    public IEnumerator MoveStorage()
    {
        if (pv.isMine)
        {
            for (int i = 0; i <= backpacksize - 1; i++)
            {
                if (backPackSpace[i].childCount > 0)
                {
                    for (int j = 0; j < storageSpace.Length; j++)
                    {
                        if (storageSpace[j].childCount == 0)
                        {
                            backPackSpace[i].GetChild(0).transform.SetParent(storageSpace[j]);
                            yield return new WaitForSeconds(0.1f);
                            break;
                        }
                    }
                }
            } 
        }
    }
    public void All_In()
    {
        StartCoroutine(MoveStorage());
    }


    public IEnumerator MoveBackPack()
    {
        if (pv.isMine)
        {
            for (int i = 0; i <= storagesize - 1; i++)
            {
                if (storageSpace[i].childCount > 0)
                {
                    for (int j = 0; j < backpacksize; j++)
                    {
                        if (backPackSpace[j].childCount == 0)
                        {
                            storageSpace[i].GetChild(0).transform.SetParent(backPackSpace[j]);
                            yield return new WaitForSeconds(0.1f);
                            break;
                        }
                    }
                }
            } 
        }
    }
    public void All_Out()
    {
        if (pv.isMine)
        {
            StartCoroutine(MoveBackPack()); 
        }
    }

    public void SetBackPack()
    {
        // 창고 가방 할당
        //backPack = GameObject.Find("Player/Canvas/Panel/BackPack/BackPackPanel");

        if (pv.isMine)
        {
            if (backPack != null)
            {
                // 가방 크기 카운트
                backpacksize = backPack.transform.childCount;

                // 가방 배열에 크기 할당
                backPackSpace = new Transform[backpacksize];

                // 가방의 Transform 정보를 넣음
                for (int i = 0; i <= backpacksize - 1; i++)
                {
                    backPackSpace[i] = backPack.transform.GetChild(i);
                }
            } 
        }
    }

    public void SetFeildBackPack()
    {
        // 가방 할당
        //FieldBackPack = GameObject.Find("Player/Canvas/Panel/InventoryPanel");

        if (pv.isMine)
        {
            if (FieldBackPack != null)
            {
                // 가방 크기 카운트
                FieldBackpacksize = FieldBackPack.transform.childCount;

                // 가방 배열에 크기 할당
                FieldBackPackSpace = new Transform[FieldBackpacksize];

                // 가방의 Transform 정보를 넣음
                for (int i = 0; i <= FieldBackpacksize - 1; i++)
                {
                    FieldBackPackSpace[i] = FieldBackPack.transform.GetChild(i);
                }
            } 
        }
    }
    public void SetQuickSlot()
    {
        // 가방 할당
        // quickslot = GameObject.Find("Player/Canvas/Panel/Quck_Slot");

        if (pv.isMine)
        {
            if (quickslot != null)
            {
                // 가방 크기 카운트
                quickslotsize = quickslot.transform.childCount;

                // 가방 배열에 크기 할당
                quickslotSpace = new Transform[quickslotsize];

                // 가방의 Transform 정보를 넣음
                for (int i = 0; i <= quickslotsize - 1; i++)
                {
                    quickslotSpace[i] = quickslot.transform.GetChild(i);
                }
            } 
        }
    }
    public void SetEquipmentSlot()
    {
        // 가방 할당
        // Equipment_Slot = GameObject.Find("Player/Canvas/Panel/PanelEquipment/Equipment_Slot");

        if (pv.isMine)
        {
            if (Equipment_Slot != null)
            {
                // 가방 크기 카운트
                EquipmentSize = Equipment_Slot.transform.childCount;

                // 가방 배열에 크기 할당
                EquipmentSpace = new Transform[EquipmentSize];

                // 가방의 Transform 정보를 넣음
                for (int i = 0; i <= EquipmentSize - 1; i++)
                {
                    EquipmentSpace[i] = Equipment_Slot.transform.GetChild(i);
                }
            } 
        }
    }

    public void SetStorage()
    {
        // 창고 할당
        //storage = GameObject.Find("Player/Canvas/Panel/Storage/SroragePanel");

        if (pv.isMine)
        {
            if (storage != null)
            {
                // 창고 크기 카운트
                storagesize = storage.transform.childCount;

                // 창고 배열에 크기 할당
                storageSpace = new Transform[storagesize];

                // 창고 Transform 정보를 넣음
                for (int i = 0; i <= storagesize - 1; i++)
                {
                    storageSpace[i] = storage.transform.GetChild(i);
                }
            } 
        }
    }

    public void SetData()
    {
        if (pv.isMine)
        {
            if (hideOut)
            {
                for (int i = 0; i < backpacksize; i++)
                {
                    _items[i] = 0;
                    if (backPackSpace[i].childCount > 0)
                    {
                        _items[i] = backPackSpace[i].GetChild(0).GetComponent<Item>().numbering;
                        Destroy(backPackSpace[i].GetChild(0).gameObject);
                    }
                }
            }
            else
            {
                for (int i = 0; i < FieldBackpacksize; i++)
                {
                    _items[i] = 0;
                    if (FieldBackPackSpace[i].childCount > 0)
                    {
                        _items[i] = FieldBackPackSpace[i].GetChild(0).GetComponent<Item>().numbering;
                        Destroy(FieldBackPackSpace[i].GetChild(0).gameObject);
                    }
                }
            } 
        }

    }

    public void GetData()
    {
        if (pv.isMine)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                GameObject Item;
                int itemprefabnum = 0;
                switch (_items[i])
                {
                    case 0:
                        break;
                    case 1:
                        itemprefabnum = 1;
                        break;
                    case 2:
                        itemprefabnum = 2;
                        break;
                    case 3:
                        itemprefabnum = 3;
                        break;
                    case 4:
                        itemprefabnum = 4;
                        break;
                    case 5:
                        itemprefabnum = 5;
                        break;
                }
                if (itemprefabnum > 0)
                {
                    if (hideOut)
                    {
                        Item = Instantiate(itemPrefabs[itemprefabnum - 1]);
                        Item.transform.SetParent(backPackSpace[i]);
                        Item.GetComponent<Image>().sprite = Item.GetComponent<Item>().itemIcon;
                    }
                    else
                    {
                        Item = Instantiate(itemPrefabs[itemprefabnum - 1]);
                        Item.transform.SetParent(FieldBackPackSpace[i]);
                        Item.GetComponent<Image>().sprite = Item.GetComponent<Item>().itemIcon;
                    }

                }
            } 
        }
    }

    public void OnLoadeScnene(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("씬 이동");
        //hideOut = !hideOut;
        //// 씬이동시 실행된 함수들
        //SetBackPack();
        //SetFeildBackPack();
        //SetStorage();
        //GetData();
    }
}
