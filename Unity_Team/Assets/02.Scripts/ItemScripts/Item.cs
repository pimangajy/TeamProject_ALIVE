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
    // 아이템 이름
    public string itemName;
    // 아이템 아이콘
    public Sprite itemIcon;
    // 아이템 타입
    public ItemType itemType;
    // 아이템이 들어있는 가방 위치
    public int backPackPos;
    // 아이템 고유 번호
    public int numbering;

    [Header("3D 모델")]
    // 아이템 3D모델
    public GameObject model;

    [Header("가격")]
    // 가격
    public int sale_price;

    [Header("음식 아이템")]
    // 포만감
    public float fullness;
    // 수분
    public float hydration;


    [Header("무기 아이템")]
    // 공격력
    public int ATK;
    public Sprite Weapon_Img;
    public int Equipment_Slot;

    [Header("회복")]
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
