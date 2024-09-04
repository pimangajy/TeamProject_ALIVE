using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeildManager : MonoBehaviour
{
    public GameObject canvas;
    
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        canvas.SetActive(false);
        
    }

    
    void Update()
    {
        
    }
}
