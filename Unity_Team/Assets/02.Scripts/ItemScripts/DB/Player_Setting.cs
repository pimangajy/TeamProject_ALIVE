using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_Setting : MonoBehaviour
{
    public int photonId; // Example Photon_ID

    [SerializeField]
    Player player;

    void Start()
    {
        if(PhotonNetwork.isMasterClient)
        {
            StartCoroutine(CreateBackpackAndStorage("http://192.168.0.53/TeamProject/Player_Spawn.php"));
        }
    }

    // 베이스 정보 생성
    IEnumerator CreateBackpackAndStorage(string url)
    {
        // Create a form and add the photon_id
        WWWForm form = new WWWForm();

        // Create a UnityWebRequest with the form data
        UnityWebRequest www = UnityWebRequest.Post(url, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            // Debug.Log("Response: " + www.downloadHandler.text);
            Debug.Log(photonId + " 데이타베이스 생성 완료");
        }
    }

    private void OnApplicationQuit()
    {
        StartCoroutine(DeleteAllDataCoroutine());
    }

    IEnumerator DeleteAllDataCoroutine()
    {
        UnityWebRequest www = UnityWebRequest.Post("http://localhost/TeamProject/Delete_All.php", new WWWForm());
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string responseText = www.downloadHandler.text;
            Debug.Log("Response Text: " + responseText);

            try
            {
                var responseJson = JsonUtility.FromJson<Response>(responseText);
                if (responseJson.status == "success")
                {
                    Debug.Log("All data deleted successfully");
                }
                else
                {
                    Debug.Log("Failed to delete all data: " + responseJson.message);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log("JSON Parse Error: " + e.Message);
            }
        }
    }

    [System.Serializable]
    public class Response
    {
        public string status;
        public string message;
    }
}
