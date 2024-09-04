using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    

    private void Awake()
    {
       
    }

    private void Start()
    {
        StartCoroutine(QuitGame());
    }

    void Update()
    {
      
    }

    IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(3.0f);
        ScenesManager.instance.ExitRoom();
    }
}
