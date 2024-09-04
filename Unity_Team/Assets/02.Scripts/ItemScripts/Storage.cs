using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    // â�� ����
    [SerializeField]
    GameObject storage;
    // â�� ���� ����
    public Transform[] storageSpace;
    // â�� ���� ũ��
     public int storagesize;

    public PhotonView pv;

    private void Start()
    {
        if (storage != null)
        {
            // â�� ũ�� ī��Ʈ
            storagesize = storage.transform.childCount;

            // â�� �迭�� ũ�� �Ҵ�
            storageSpace = new Transform[storagesize];

            // â�� Transform ������ ����
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
