using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class R_Click_Button : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField]
    Inventory inventory;
    [Space(30)]

    public Status status;

    // 각 버튼 
    public GameObject use;
    public GameObject equip;
    public GameObject drop;

    // 우클릭시 받아올 아이템 정보
    Item itembutton;

    // 버리기 버튼 용 아이템 저장
    GameObject Item;

    public GameObject panel;
    Vector3 defaultPos;

    public PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        // panel = GameObject.Find("Player/Canvas/Panel/R_Click");
        //status = GameObject.FindFirstObjectByType<Status>();
    }

    private void Start()
    {
        if (pv.isMine)
        {
            defaultPos = panel.transform.position; 
        }
    }

    public void ItemSet(GameObject _Item)
    {
        if (pv.isMine)
        {
            // 아이콘 위에서 우클릭한 오브젝트의 정보를 받아옴
            itembutton = _Item.GetComponent<Item>();

            Item = _Item; 
        }
    }

    public void Use(GameObject _item)
    {
        if (pv.isMine)
        {
            Debug.Log(_item);
            if (_item.GetComponent<Item>().itemType == ItemType.Hunger)
            {
                // 포만감
                status.ItemUseFullness(_item.GetComponent<Item>().fullness);
            }
            else if(_item.GetComponent<Item>().itemType == ItemType.thirst)
            {
                // 갈증
                status.ItemUseHydration(_item.GetComponent<Item>().hydration);
            }
            else if (_item.GetComponent<Item>().itemType == ItemType.recovery)
            {
                // 회복량
                status.ItemUseRecorvery(_item.GetComponent<Item>().recovery);
            }

            Destroy(_item);
            panel.SetActive(false); 
        }
    }
    public void Use()
    {
        if (pv.isMine)
        {
            if (itembutton.itemType == ItemType.Hunger)
            {
                status.ItemUseFullness(itembutton.fullness);
            }
            else
            {
                status.ItemUseHydration(itembutton.hydration);
            }
            Destroy(Item);
            panel.SetActive(false);
            panel.transform.position = defaultPos; 
        }
    }

    public void Equip()
    {
        if (pv.isMine)
        {
            for (int i = 0; i < inventory.EquipmentSpace.Length; i++)
            {
                if (inventory.EquipmentSpace[i].childCount == 0)
                {
                    itembutton.transform.SetParent(inventory.EquipmentSpace[i]);
                    Item.GetComponent<DragItem>().ImageChainge();
                }
            }
            panel.SetActive(false);
            panel.transform.position = defaultPos; 
        }
    }

    public void Drop()
    {
        if (pv.isMine)
        {
            GameObject pos = GameObject.FindGameObjectWithTag("Item_Drop_Pos");
            PhotonNetwork.Instantiate(itembutton.model.name, pos.transform.position, pos.transform.rotation,0);
            Destroy(Item);
            panel.SetActive(false);
            panel.transform.position = defaultPos; 
        }
    }

    private void OnEnable()
    {
        if (pv.isMine)
        {
            // 활성화 되면서 아이템에 맞는 버튼 활성화
            if (itembutton.itemType == ItemType.Weapon)
            {
                use.SetActive(false);
                equip.SetActive(true);
                drop.SetActive(true);
            }
            else if (itembutton.itemType == ItemType.Hunger || itembutton.itemType == ItemType.thirst)
            {
                use.SetActive(true);
                equip.SetActive(false);
                drop.SetActive(true);
            }
            else
            {
                use.SetActive(false);
                equip.SetActive(false);
                drop.SetActive(true);
            } 
        }
    }


}
