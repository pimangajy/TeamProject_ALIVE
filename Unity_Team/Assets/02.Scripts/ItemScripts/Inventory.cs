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

    // ����
    [SerializeField]
    GameObject FieldBackPack;
    // ���� ����
    public Transform[] FieldBackPackSpace;
    // ���� ũ��
    int FieldBackpacksize;

    //-------------------------------------------------------

    // â�� ����
    [SerializeField]
    GameObject backPack;
    // â�� ���� ����
    public Transform[] backPackSpace;
    // â�� ���� ũ��
    int backpacksize;

    //-------------------------------------------------------

    // â��
    [SerializeField]
    public GameObject storage;
    // â�� ����
    public Transform[] storageSpace;
    // â�� ũ��
    int storagesize;

    //-------------------------------------------------------

    // ������
    [SerializeField]
    GameObject quickslot;
    // ������ ����
    public Transform[] quickslotSpace;
    // ������ ũ��
    int quickslotsize;

    //-------------------------------------------------------

    [SerializeField]
    GameObject Equipment_Slot;
    public Transform[] EquipmentSpace;
    int EquipmentSize;

    //-------------------------------------------------------

    // ���濡 �������߰��� ���°�
    public GameObject itemprefab;

    public int[] _items;

    public bool hideOut = true;

    public GameObject pan;

    [Header("PV")]
    public PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();

        // â�� ���� �Ҵ�
        // backPack = GameObject.Find("Player/Canvas/Panel/BackPack/BackPackPanel");

        // â�� �Ҵ�
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
    // ������ ŉ��� ���濡 �̹����� �ٲ�
    public void ItemPlus(GameObject prefab)  // ŉ��� ��������Ʈ�� �ϳ� �޾� ���濡 �ִ´�
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

    // ����� ���� �ʱ�ȭ
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

    // �Ѿ� Ȯ��
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


    // â��� �̵�
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
        // â�� ���� �Ҵ�
        //backPack = GameObject.Find("Player/Canvas/Panel/BackPack/BackPackPanel");

        if (pv.isMine)
        {
            if (backPack != null)
            {
                // ���� ũ�� ī��Ʈ
                backpacksize = backPack.transform.childCount;

                // ���� �迭�� ũ�� �Ҵ�
                backPackSpace = new Transform[backpacksize];

                // ������ Transform ������ ����
                for (int i = 0; i <= backpacksize - 1; i++)
                {
                    backPackSpace[i] = backPack.transform.GetChild(i);
                }
            } 
        }
    }

    public void SetFeildBackPack()
    {
        // ���� �Ҵ�
        //FieldBackPack = GameObject.Find("Player/Canvas/Panel/InventoryPanel");

        if (pv.isMine)
        {
            if (FieldBackPack != null)
            {
                // ���� ũ�� ī��Ʈ
                FieldBackpacksize = FieldBackPack.transform.childCount;

                // ���� �迭�� ũ�� �Ҵ�
                FieldBackPackSpace = new Transform[FieldBackpacksize];

                // ������ Transform ������ ����
                for (int i = 0; i <= FieldBackpacksize - 1; i++)
                {
                    FieldBackPackSpace[i] = FieldBackPack.transform.GetChild(i);
                }
            } 
        }
    }
    public void SetQuickSlot()
    {
        // ���� �Ҵ�
        // quickslot = GameObject.Find("Player/Canvas/Panel/Quck_Slot");

        if (pv.isMine)
        {
            if (quickslot != null)
            {
                // ���� ũ�� ī��Ʈ
                quickslotsize = quickslot.transform.childCount;

                // ���� �迭�� ũ�� �Ҵ�
                quickslotSpace = new Transform[quickslotsize];

                // ������ Transform ������ ����
                for (int i = 0; i <= quickslotsize - 1; i++)
                {
                    quickslotSpace[i] = quickslot.transform.GetChild(i);
                }
            } 
        }
    }
    public void SetEquipmentSlot()
    {
        // ���� �Ҵ�
        // Equipment_Slot = GameObject.Find("Player/Canvas/Panel/PanelEquipment/Equipment_Slot");

        if (pv.isMine)
        {
            if (Equipment_Slot != null)
            {
                // ���� ũ�� ī��Ʈ
                EquipmentSize = Equipment_Slot.transform.childCount;

                // ���� �迭�� ũ�� �Ҵ�
                EquipmentSpace = new Transform[EquipmentSize];

                // ������ Transform ������ ����
                for (int i = 0; i <= EquipmentSize - 1; i++)
                {
                    EquipmentSpace[i] = Equipment_Slot.transform.GetChild(i);
                }
            } 
        }
    }

    public void SetStorage()
    {
        // â�� �Ҵ�
        //storage = GameObject.Find("Player/Canvas/Panel/Storage/SroragePanel");

        if (pv.isMine)
        {
            if (storage != null)
            {
                // â�� ũ�� ī��Ʈ
                storagesize = storage.transform.childCount;

                // â�� �迭�� ũ�� �Ҵ�
                storageSpace = new Transform[storagesize];

                // â�� Transform ������ ����
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
        //Debug.Log("�� �̵�");
        //hideOut = !hideOut;
        //// ���̵��� ����� �Լ���
        //SetBackPack();
        //SetFeildBackPack();
        //SetStorage();
        //GetData();
    }
}
