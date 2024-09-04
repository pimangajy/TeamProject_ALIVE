using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponSlot : InventoryManager, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public int poketNum;

    Image icon;
    RectTransform rect;

    void Start()
    {
        icon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        inventype = inventoryType.Equipment_Slot;
    }

    // 마우스 포인터가 오브젝트 위에서 드롭 했을때 1회 호출
    public void OnDrop(PointerEventData eventData)
    {
        GameObject data = eventData.pointerDrag;

        // 장비칸에 놓을때 무기가 아니면 리턴
        if(!(data.tag == "Item") || !(eventData.pointerDrag.GetComponent<Item>().itemType == ItemType.Weapon))
        {
            return;
        }
        // 드래그중이고 드롭했을때 오브젝트의 자식이 널일때
        if (data != null && transform.childCount == 0)
        {
            // 드래그하고 있는 대상의 부모를 현재 오브젝트로 설정 , 위치를 동일하게
            data.transform.SetParent(transform);
            data.GetComponent<RectTransform>().position = rect.position;
            data.GetComponent<Item>().Equipment_Slot = poketNum;


        }
        else if (data != null && transform.childCount == 1)
        {
            Transform child = transform.GetChild(0);

            child.GetComponent<Item>().Equipment_Slot = 0;

            data.GetComponent<Item>().Equipment_Slot = poketNum;

            // 부모를 바꿈으로 위치 변경
            child.GetComponent<Item>().backPackPos = data.GetComponent<Item>().backPackPos;
            // 가방 위치 설정
            child.SetParent(data.GetComponent<DragItem>().previousParent);

            data.transform.SetParent(transform);

            child.GetComponent<DragItem>().ImageChainge();
        }
    }

    // 마우스 포인터가 오브젝트에 들어올때
    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.color = Color.red;
    }

    // 마우스 포인터가 오브젝트를 나갈때
    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = Color.white;
    }
}
