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


    // ���콺 �����Ͱ� ������Ʈ ������ ��� ������ 1ȸ ȣ��
    public void OnDrop(PointerEventData eventData)
    {
        GameObject data = eventData.pointerDrag;

        // �κ�ĭ���� �������� �ƴ� �ٸ� ���𰡰� ����Ǹ� ����
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
        // �巡������ �ƴϰ� ��������� ������Ʈ�� �ڽ��� ���϶�
        if (data != null && transform.childCount == 0)
        {
            // �巡���ϰ� �ִ� ����� �θ� ���� ������Ʈ�� ���� , ��ġ�� �����ϰ�
            data.transform.SetParent(transform);
            data.GetComponent<RectTransform>().position = rect.position;
            data.GetComponent<Item>().backPackPos = poketNum;

            // ���ĭ���� �������� ���ö� ���ĭ������ 0���� �ٲ�
            data.GetComponent<Item>().Equipment_Slot = 0;

            data.GetComponent<Image>().sprite = data.GetComponent<DragItem>().itemIcon;

        }
        else if (data != null && transform.childCount == 1)
        {
            Debug.Log("��ġ ��ü");
            Transform child = transform.GetChild(0);

            // ���ĭ�� �ִ� �����۰� ������ �����۰� �����Ҷ� ������ �������� ���Ⱑ �ƴ϶�� �Ұ�
            if (data.GetComponent<Item>().Equipment_Slot > 0 && child.GetComponent<Item>().itemType != ItemType.Weapon)
            {
                return;
            }
            // ���ĭ�� �����۰� ����ĭ�� �������� �����Ҷ� ����ĭ�� �������� �����϶�
            else if (data.GetComponent<Item>().Equipment_Slot > 0 && child.GetComponent<Item>().itemType == ItemType.Weapon)
            {
                child.GetComponent<Item>().Equipment_Slot = data.GetComponent<Item>().Equipment_Slot;
            }

            // �θ� �ٲ����� ��ġ ����
            child.GetComponent<Item>().backPackPos = data.GetComponent<Item>().backPackPos;
            child.SetParent(data.transform.parent);
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
        storage.Call();
    }

    // ���콺 �����Ͱ� ������Ʈ�� ������
    public void OnPointerExit(PointerEventData eventData)
    {
        icon.color = Color.white;
        storage.Call();
    }
}
