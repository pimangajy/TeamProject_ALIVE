using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Weapon,
    Hunger,
    thirst,
    miscellaneous,
    recovery
};

public class Item : MonoBehaviour
{
    // ������ �̸�
    public string itemName;
    // ������ ������
    public Sprite itemIcon;
    // ������ Ÿ��
    public ItemType itemType;
    // �������� ����ִ� ���� ��ġ
    public int backPackPos;
    // ������ ���� ��ȣ
    public int numbering;

    [Header("3D ��")]
    // ������ 3D��
    public GameObject model;

    [Header("����")]
    // ����
    public int sale_price;

    [Header("���� ������")]
    // ������
    public float fullness;
    // ����
    public float hydration;


    [Header("���� ������")]
    // ���ݷ�
    public int ATK;
    public Sprite Weapon_Img;
    public int Equipment_Slot;

    [Header("ȸ��")]
    public float recovery;

    public Item() { }

    public Item(string name, Sprite icon, ItemType type, int pos)
    {
        itemName = name;
        itemIcon = icon;
        itemType = type;
        backPackPos = pos;
    }
    public Item(string name, Sprite icon, ItemType type)
    {
        itemName = name;
        itemIcon = icon;
        itemType = type;
    }

    public Item(int i)
    {
        backPackPos = i;
    }
}
