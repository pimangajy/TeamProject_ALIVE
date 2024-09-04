using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class DB_Test : MonoBehaviour
{
    // "http://localhost/htdoc에 만든 폴더이름/php 파일"로 지정합니다.
    public string saveDataURL = "http://localhost/UnityTest/savedata.php";
    public string loadDataURL = "http://localhost/UnityTest/loaddata.php";

    public void Save()
    {
        StartCoroutine(PostTest("test1", 0.01f, 0.01f, 0.01f));
        Debug.Log("Save");
    }

    IEnumerator PostTest(string name, float x, float y, float z)
    {
        // SQL문에 전달할 인수들을 &로 묶어서 지정해줍니다.
        // 이때, php문의 $_Get[]에서 사용한 변수들로 지정해줘야합니다.
        string data = "name=" + UnityWebRequest.EscapeURL(name) + "&x=" + x + "&y=" + y + "&z=" + z;
        Debug.Log(data);
        string postURL = saveDataURL + "?" + data;
        Debug.Log(postURL);
        UnityWebRequest hs_post = UnityWebRequest.Post(postURL, data);

        yield return hs_post.SendWebRequest();

        if (hs_post.error != null)
        {
            Debug.LogError("There was an error posting the data:" + hs_post.error);
        }
    }
    public void Load()
    {
        StartCoroutine(GetData());
        Debug.Log("Load");
    }

    IEnumerator GetData()
    {
        UnityWebRequest hs_get = UnityWebRequest.Get(loadDataURL);

        yield return hs_get.SendWebRequest();

        if (hs_get.error != null)
        {
            Debug.LogError("There was an error getting the data:" + hs_get.error);
        }
        else
        {
            string data = hs_get.downloadHandler.text;
            Debug.Log(data);
            MatchCollection mc = Regex.Matches(data, @"_");
            Debug.Log(mc.Count);
            if (mc.Count > 0)
            {
                string[] splitData = Regex.Split(data, @"_");
                string result = "";
                for (int i = 0; i < mc.Count; i++)
                {
                    result += splitData[i] + " ";
                    if (i % 4 == 3)
                    {
                        Debug.Log(result);
                        result = "";
                    }
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
