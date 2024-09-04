using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas_Instance : MonoBehaviour
{
    public static Canvas_Instance instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }else
        {
            Destroy(gameObject);
        }    
    }
}
