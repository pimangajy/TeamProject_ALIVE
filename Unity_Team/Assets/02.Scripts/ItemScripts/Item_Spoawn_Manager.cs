using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Spoawn_Manager : MonoBehaviour
{
    [SerializeField]
    Transform[] Pos;
    [SerializeField]
    GameObject[] Item;

    [SerializeField]
    int[] num;
    [SerializeField]
    GameObject Items;

    int noneItem;

    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Pos = new Transform[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                Pos[i] = transform.GetChild(i);
            }

            int numplus = 0;
            for (int i = 0; i < num.Length; i++)
            {
                numplus += num[i];
            }
            if (numplus > 100)
            {
                Debug.LogError("100 Over");
            }
            else
            {
                noneItem = 100 - numplus;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                Percentage(i);
            } 
        }
    }

    public void Percentage(int j)
    {
        int rand = Random.Range(1, 101);
        int numplus = 0;

        for(int i = 0; i < num.Length; i++)
        {
            numplus += num[i];
            if(rand <= numplus)
            {
                GameObject ITEM = PhotonNetwork.Instantiate(Item[i].gameObject.name, Pos[j].position, Quaternion.identity,0);
                //ITEM.transform.SetParent(transform.GetChild(j));
                ITEM.transform.parent = Items.transform;
                return;
            }
        }
    }
}
