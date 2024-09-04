using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Save_ID : MonoBehaviour
{
    Login login;

    public string name;
    public int[] BackPack = new int[20];

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }



    public void MyDestroy()
    {
        Destroy(gameObject);
    }

}
