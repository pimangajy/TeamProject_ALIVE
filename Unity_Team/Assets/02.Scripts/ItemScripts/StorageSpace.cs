using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StorageSpace : InventoryManager, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    [SerializeField]
    Storage storage;

    [SerializeField]
    bool Space;

    [SerializeField]
    GameObject[] Items;

    public int poketNum = 0;

    Image icon;
    RectTransform rect;

    PhotonView pv;

    void Start()
    {
        icon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        pv = GetComponent<PhotonView>();
    }


    // 마우스 포인터가 오브젝트 위에서 드롭 했을때 1회 호출
    public void OnDrop(PointerEventData eventData)
    {
        GameObject data = eventData.pointerDrag;

        // 인벤칸위에 아이템이 아닌 다른 무언가가 드랍되면 리턴
        if (!(data.tag == "Item"))
        {
            return;
        }

        pv.RPC("CreateItem", PhotonTargets.Others, data.GetComponent<DragItem>().numbering);
        // CreateItem(data.GetComponent<DragItem>().numbering);
         OnStorage(data);
    }

    [PunRPC]
    public void CreateItem(int numbering)
    {
        GameObject i = Instantiate(Items[numbering - 1]);
        OnStorage(i);
        //Destroy(i);
    }

    public void OnStorage(GameObject data)
    {
        Debug.Log(data);
        // 드래그중이 아니고 드롭했을때 오브젝트의 자식이 널일때
        if (data != null && transform.childCount == 0)
        {
            // 드래그하고 있는 대상의 부모를 현재 오브젝트로 설정 , 위치를 동일하게
            data.transform.SetParent(transform);
            data.GetComponent<RectTransform>().position = rect.position;
            data.GetComponent<Item>().backPackPos = poketNum;

            // 장비칸에서 가방으로 들어올때 장바칸슬롯을 0으로 바꿈
            data.GetComponent<Item>().Equipment_Slot = 0;

            data.GetComponent<Image>().sprite = data.GetComponent<DragItem>().itemIcon;

        }
        else if (data != null && transform.childCount == 1)
        {
            Debug.Log("위치 교체");
            Transform child = transform.GetChild(0);

            // 장비칸에 있던 아이템과 가방의 아이템과 스왑할때 가방의 아이템이 무기가 아니라면 불가
            if (data.GetComponent<Item>().Equipment_Slot > 0 && child.GetComponent<Item>().itemType != ItemType.Weapon)
            {
                return;
            }
            // 장비칸의 아이템과 가방칸에 아이템을 스왑할때 가방칸의 아이템이 무기일때
            else if (data.GetComponent<Item>().Equipment_Slot > 0 && child.GetComponent<Item>().itemType == ItemType.Weapon)
            {
                child.GetComponent<Item>().Equipment_Slot = data.GetComponent<Item>().Equipment_Slot;
            }

            // 부모를 바꿈으로 위치 변경
            child.GetComponent<Item>().backPackPos = data.GetComponent<Item>().backPackPos;
            child.SetParent(data.transform.parent);
            // 가방 위치 설정
            child.SetParent(data.GetComponent<DragItem>().previousParent);

            data.transform.SetParent(transform);
            data.GetComponent<Item>().backPackPos = poketNum;
            // 장비칸에서 가방으로 들어올때 장바칸슬롯을 0으로 바꿈
            data.GetComponent<Item>().Equipment_Slot = 0;

            child.GetComponent<DragItem>().ImageChainge();
        }
    }

    // 마우스 포인터가 오브젝트에 들어올때
    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.color = Color.red;
        storage.Call();
    }

    // 마우스 포인터가 오브젝트를 나갈때
    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = Color.white;
        storage.Call();
    }
}
