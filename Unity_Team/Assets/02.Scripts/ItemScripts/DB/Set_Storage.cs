using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Set_Storage : MonoBehaviour
{
    public int photonId; // Example Photon_ID
    public int[] items = new int[50]; // Example array of items

    [SerializeField]
    Player player;

    private void Start()
    {
        photonId = player.pv.viewID;
        StartCoroutine(SendStorageData("http://localhost/TeamProject/Set_Storage.php"));
    }

    public void UpdateStorage()
    {
        StartCoroutine(SendStorageData("http://localhost/TeamProject/Set_Storage.php"));
    }

    IEnumerator SendStorageData(string url)
    {
        // Create a payload object
        var payload = new Payload
        {
            photon_id = photonId,
            items = items
        };

        // Convert the payload object to a JSON string
        string jsonPayload = JsonUtility.ToJson(payload);
        // Debug.Log("Sending JSON: " + jsonPayload);  배열 확인

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
            Debug.Log(photonId + "창고 설정 완료");
        }
    }
    [System.Serializable]
    public class Payload
    {
        public int photon_id;
        public int[] items;
    }
}
