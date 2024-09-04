using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_crafting : MonoBehaviour
{
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    Storage storage;
    [SerializeField]
    HideOut_Object hideout;

    public Transform[] Slots;
    public GameObject[] ingredients;

    public Transform Item;
    public GameObject _Item;

    public bool UpGrade;
    public bool Medical_Table;
    public bool Kichin;
    public bool Base;


    private void Awake()
    {
        for(int i = 0; i < Slots.Length; i++)
        {
            GameObject item = Instantiate(ingredients[i]);
            item.transform.SetParent(Slots[i]);
            item.GetComponent<Image>().sprite = item.GetComponent<Item>().itemIcon;
        }
        if(!UpGrade)
        {
            GameObject _item = Instantiate(_Item);
            _item.transform.SetParent(Item);
            _item.GetComponent<Image>().sprite = _item.GetComponent<Item>().itemIcon;
        }
    }

    public void Crafting()
    {
        Debug.Log("Crafting");
        int count = 0;
        int maxcount = Slots.Length;
        int[] chekingnum = new int[maxcount];
        GameObject[] Consumption_item = new GameObject[Slots.Length];

        for(int i = 0; i < chekingnum.Length; i++)
        {
            chekingnum[i] = -1;
        }
        bool skip = false;
        for (int i = 0; i < Slots.Length; i++)
        {
            Debug.Log( (i + 1) +  " 번째 재료 확인 시작");
            for (int j = 0; j < inventory.FieldBackPackSpace.Length; j++)
            {
                Debug.Log("가방 탐색중");
                skip = false;
                
                if (inventory.FieldBackPackSpace[j].childCount > 0 && Slots[i].GetChild(0).GetComponent<Item>().numbering == inventory.FieldBackPackSpace[j].GetChild(0).GetComponent<Item>().numbering)
                {
                    for(int k = 0; k < chekingnum.Length; k++)
                    {
                        if(chekingnum[k] == j)
                        {
                            Debug.Log(chekingnum[k] + " " + j + " 증복!");
                            skip = true;
                            break;
                        }
                    }
                    if (skip)
                        continue;

                    //if (chekingnum[0] == j)
                    //{
                    //    Debug.Log(chekingnum[0] + " " + j + " 증복!");
                    //    continue;
                    //}
                    //else if (chekingnum[1] == j)
                    //{
                    //    Debug.Log(chekingnum[1] + " " + j + " 증복!");
                    //    continue;
                    //}
                    //else if (chekingnum[2] == j)
                    //{
                    //    Debug.Log(chekingnum[2] + " " + j + " 증복!");
                    //    continue;
                    //}
                    //else if (chekingnum[3] == j)
                    //{
                    //    Debug.Log(chekingnum[3] + " " + j + " 증복!");
                    //    continue;
                    //}
                    Debug.Log("재료 찾음 " + j + " " + inventory.FieldBackPackSpace[j].GetChild(0).gameObject);
                    Debug.Log("-==============================================================================-");
                    Consumption_item[i] = inventory.FieldBackPackSpace[j].GetChild(0).gameObject;
                    chekingnum[count] = j;
                    count++;
                    for(int a = 0; a < chekingnum.Length; a++)
                    {
                        Debug.Log(chekingnum[a]);
                    }
                    Debug.Log("=================================================================================");
                    break;
                }
            }
        }

        if(count == maxcount)
        {
            for (int i = 0; i < Slots.Length; i++)
            {
                Debug.Log("삭제할 아이템" + Consumption_item[i]);
                Destroy(Consumption_item[i]);
            }
            if(UpGrade)
            {
                if (Medical_Table)
                {
                    hideout = GameObject.Find("Medical Place").GetComponent<HideOut_Object>();
                    hideout.Hide_Out_LevelUp(1,0,0);
                    Debug.Log("medical levelup");
                }
                else if(Kichin)
                {
                    hideout = GameObject.Find("Kitchen").GetComponent<HideOut_Object>();
                    hideout.Hide_Out_LevelUp(0,1,0);
                    Debug.Log("kitchen levelup");
                }else if(Base)
                {
                    hideout = GameObject.Find("Kitchen").GetComponent<HideOut_Object>();
                    hideout.Hide_Out_LevelUp(0, 0, 50);
                    Debug.Log("Base repair");
                }
            }else
                inventory.ItemPlus(_Item);
        }
    }
}
