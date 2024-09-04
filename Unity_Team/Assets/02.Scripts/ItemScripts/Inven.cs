using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inven : InventoryManager, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    public int poketNum = 0;

    Image icon;
    RectTransform rect;

    void Start()
    {
        icon = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }


    // ���콺 �����Ͱ� ������Ʈ ������ ��� ������ 1ȸ ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        GameObject data = eventData.pointerDrag;
        
        // �κ�ĭ���� �������� �ƴ� �ٸ� ���𰡰� ����Ǹ� ����
        if(!(data.tag == "Item"))
        {
            return;
        }

        // �巡������ �ƴϰ� ��������� ������Ʈ�� �ڽ��� ���϶�
        if(data != null && transform.childCount == 0)
        {
            // �巡���ϰ� �ִ� ����� �θ� ���� ������Ʈ�� ���� , ��ġ�� �����ϰ�
            data.transform.SetParent(transform);
            data.GetComponent<RectTransform>().position = rect.position;
            data.GetComponent<Item>().backPackPos = poketNum;

            // ���ĭ���� �������� ���ö� ���ĭ������ 0���� �ٲ�
            data.GetComponent<Item>().Equipment_Slot = 0;

        }
        else if(data != null && transform.childCount == 1)
        {
            Transform child = transform.GetChild(0);

            // ���ĭ�� �ִ� �����۰� ������ �����۰� �����Ҷ� ������ �������� ���Ⱑ �ƴ϶�� �Ұ�
            if (data.GetComponent<Item>().Equipment_Slot > 0 && child.GetComponent<Item>().itemType != ItemType.Weapon)
            {
                return;
            }
            // ���ĭ�� �����۰� ����ĭ�� �������� �����Ҷ� ����ĭ�� �������� �����϶�
            else if(data.GetComponent<Item>().Equipment_Slot > 0 && child.GetComponent<Item>().itemType == ItemType.Weapon)
            {
                child.GetComponent<Item>().Equipment_Slot = data.GetComponent<Item>().Equipment_Slot;
            }



            // �θ� �ٲ����� ��ġ ����
            child.GetComponent<Item>().backPackPos = data.GetComponent<Item>().backPackPos;
            // ���� ��ġ ����
            child.SetParent(data.GetComponent<DragItem>().previousParent);

            data.transform.SetParent(transform);
            data.GetComponent<Item>().backPackPos = poketNum;
            // ���ĭ���� �������� ���ö� ���ĭ������ 0���� �ٲ�
            data.GetComponent<Item>().Equipment_Slot = 0;

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
