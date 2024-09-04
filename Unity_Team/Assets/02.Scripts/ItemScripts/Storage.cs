using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    // 창고 가방
    [SerializeField]
    GameObject storage;
    // 창고 가방 공간
    public Transform[] storageSpace;
    // 창고 가방 크기
     public int storagesize;

    public PhotonView pv;

    private void Start()
    {
        if (storage != null)
        {
            // 창고 크기 카운트
            storagesize = storage.transform.childCount;

            // 창고 배열에 크기 할당
            storageSpace = new Transform[storagesize];

            // 창고 Transform 정보를 넣음
            for (int i = 0; i <= storagesize - 1; i++)
            {
                storageSpace[i] = storage.transform.GetChild(i);
            }
        }
    }

    public void setPV(PhotonView p)
    {
        pv = p;
    }

    public void Call()
    {
       // pv.RPC("ChaingeSpace", PhotonTargets.All);
    }

    [PunRPC]
    public void ChaingeSpace()
    {
        for (int i = 0; i < storagesize; i++)
        {
            bool HasChilds = CheckChild(storageSpace[i]);

            if (HasChilds)
            {
                //OnStorage(storageSpace[i].GetChild(0).gameObject);
            }
            else
            {
                Destroy(storageSpace[i].GetChild(0));
            }
        }
    }

    public bool CheckChild(Transform t)
    {
        return t.childCount > 0;
    }
}
