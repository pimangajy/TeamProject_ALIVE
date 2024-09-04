using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    Text loading;
    
    void Start()
    {
        loading = GetComponent<Text>();
        StartCoroutine(loadingUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator loadingUpdate()
    {
        while (true)
        {
            loading.text = "Loading.";
            yield return new WaitForSeconds(0.6f);
            loading.text = "Loading..";
            yield return new WaitForSeconds(0.6f);
            loading.text = "Loading...";
            yield return new WaitForSeconds(0.6f);
        }
    }
}
