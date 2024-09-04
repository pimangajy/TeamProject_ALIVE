using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Hide_Out_Setting : MonoBehaviour
{
    static public Hide_Out_Setting instance = null;

    public LevelData levelData;

    public int localhp;

    private string url = "http://192.168.0.53/TeamProject/Hide_Out_Setting.php";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);

            StartCoroutine(GetData()); 
        
    }
    public void Getdata()
    {
        StartCoroutine(GetData());
    }

    public void hide_out_Attack(int count)
    {
        if(PhotonNetwork.isMasterClient)
        {
            Debug.Log("Death Count : " + count);
            int minusBase = 35 + (10 * count);
            OnButtonPress(base_ : -minusBase);
        }
    }


    public void CommandSenter(int platerID)
    {
        PhotonView playerpv = PhotonView.Find(platerID);
        Player player = playerpv.gameObject.GetComponent<Player>();
    }

    public void Heal(int platerID)
    {
        PhotonView playerpv = PhotonView.Find(platerID);
        Player player = playerpv.gameObject.GetComponent<Player>();
        Status status = player.transform.Find("StatusSystem").GetComponent<Status>();

        if (player.useHeal == false)
        {
            Debug.Log("Heal Use");
            status.Hp += (10 + (levelData.Medical * 10));
            player.useHeal = true;
        }
    }
    public void Kichin(int platerID)
    {
        PhotonView playerpv = PhotonView.Find(platerID);
        Player player = playerpv.gameObject.GetComponent<Player>();
        Status status = player.transform.Find("StatusSystem").GetComponent<Status>();

        if (player.useKichin == false)
        {
            Debug.Log("Kitchen Use");
            status.Hunger += (20 + (levelData.Kitchen * 10));
            status.thirst += (20 + (levelData.Kitchen * 10));
            player.useKichin = true;
        }
    }

    public void OnButtonPress(int kitchen = 0, int medical = 0, int base_ = 0)
    {
        levelData.Kitchen += kitchen;
        levelData.Medical += medical;
        levelData.Base += base_;
        Debug.Log(levelData.Kitchen + " " + levelData.Medical + " " + levelData.Base);
        StartCoroutine(SendData(levelData.Kitchen, levelData.Medical, levelData.Base));
    }

    // 데이터를 가져오는 코루틴
    IEnumerator GetData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                // 서버에서 받은 JSON 데이터 파싱
                string jsonResult = www.downloadHandler.text;
                Debug.Log(jsonResult);
                LevelData levelData = JsonUtility.FromJson<LevelData>(jsonResult);
                Debug.Log("Workbench: " + levelData.Kitchen);
                Debug.Log("Medical: " + levelData.Medical);
                Debug.Log("Base: " + levelData.Base);
                localhp = levelData.Base;
            }
        }
    }

    // 데이터를 보내는 코루틴
    IEnumerator SendData(int workbench = 0, int medical = 0, int base_ = 0)
    {
        WWWForm form = new WWWForm();
        form.AddField("Kitchen", workbench);
        form.AddField("Medical", medical);
        form.AddField("Base", base_);

        using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    [System.Serializable]
    public class LevelData
    {
        public int Kitchen = 1;
        public int Medical = 1;
        public int Base = 100;
    }
}
