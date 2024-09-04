using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Set_BackPack : MonoBehaviour
{

    public int photonId; // Example Photon_ID
    public int[] items = new int[20]; // Example array of items

    [SerializeField]
    Player player;

    private void Start()
    {
        photonId = player.pv.viewID;
        StartCoroutine(SendBackpackData("http://localhost/TeamProject/Set_BackPack.php"));
    }

    public void UpdateBackpack()
    {
        StartCoroutine(SendBackpackData("http://localhost/TeamProject/Set_BackPack.php"));
    }

    IEnumerator SendBackpackData(string url)
    {
        // Create a payload object
        var payload = new Payload
        {
            photon_id = photonId,
            items = items
        };

        // Convert the payload object to a JSON string
        string jsonPayload = JsonUtility.ToJson(payload);
        // Debug.Log("Sending JSON: " + jsonPayload); 배열 정보

        // Create a UnityWebRequest with the JSON payload
        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonPayload);
        www.uploadHandler = new UploadHandlerRaw(jsonToSend);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            // Debug.Log("Response: " + www.downloadHandler.text);
            Debug.Log(photonId + " 가방 세팅 완료");
        }
    }
    [System.Serializable]
    public class Payload
    {
        public int photon_id;
        public int[] items;
    }
}
