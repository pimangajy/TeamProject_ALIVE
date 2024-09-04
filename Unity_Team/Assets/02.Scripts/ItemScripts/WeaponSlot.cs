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

    // ���콺 �����Ͱ� ������Ʈ ������ ��� ������ 1ȸ ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        GameObject data = eventData.pointerDrag;

        // ���ĭ�� ������ ���Ⱑ �ƴϸ� ����
        if(!(data.tag == "Item") || !(eventData.pointerDrag.GetComponent<Item>().itemType == ItemType.Weapon))
        {
            return;
        }
        // �巡�����̰� ��������� ������Ʈ�� �ڽ��� ���϶�
        if (data != null && transform.childCount == 0)
        {
            // �巡���ϰ� �ִ� ����� �θ� ���� ������Ʈ�� ���� , ��ġ�� �����ϰ�
            data.transform.SetParent(transform);
            data.GetComponent<RectTransform>().position = rect.position;
            data.GetComponent<Item>().Equipment_Slot = poketNum;


        }
        else if (data != null && transform.childCount == 1)
        {
            Transform child = transform.GetChild(0);

            child.GetComponent<Item>().Equipment_Slot = 0;

            data.GetComponent<Item>().Equipment_Slot = poketNum;

            // �θ� �ٲ����� ��ġ ����
            child.GetComponent<Item>().backPackPos = data.GetComponent<Item>().backPackPos;
            // ���� ��ġ ����
            child.SetParent(data.GetComponent<DragItem>().previousParent);

            data.transform.SetParent(transform);

            child.GetComponent<DragItem>().ImageChainge();
        }
    }

    // ���콺 �����Ͱ� ������Ʈ�� ���ö�
    public void OnPointerEnter(PointerEventData eventData)
    {
        icon.color = Color.red;
    }

    // ���콺 �����Ͱ� ������Ʈ�� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = Color.white;
    }
}
