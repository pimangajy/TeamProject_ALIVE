using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Get_BackPack : MonoBehaviour
{
    public int photonId;
    public int[] backpackItems = new int[20]; // 아이템 배열

    [SerializeField]
    Player player;


    private void Start()
    {
        photonId = player.pv.viewID;
        StartCoroutine(GetBackpackCoroutine());
    }

    public void LoadBackpack()
    {
        photonId = player.pv.viewID;
        StartCoroutine(GetBackpackCoroutine());
    }

    IEnumerator GetBackpackCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("photon_id", photonId);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/TeamProject/Get_BacpPack.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            string responseText = www.downloadHandler.text;
             // Debug.Log("Response Text: " + responseText); // JSON 데이터 로그 출력

            try
            {
                var responseJson = JsonUtility.FromJson<Response>(responseText);

                if (responseJson == null)
                {
                    Debug.Log("Failed to parse JSON");
                }

                if (responseJson.status == "success")
                {
                    // Debug.Log("Backpack data loaded successfully");
                    Debug.Log(photonId + " 가방 불러오기 성공");
                    UpdateBackpack(responseJson);
                }
                else
                {
                    Debug.Log(responseJson.message);
                }

                // Debug.Log(responseJson.message);
            }
            catch (System.Exception e)
            {
                Debug.Log("JSON Parse Error: " + e.Message);
            }
        }
    }

    void UpdateBackpack(Response response)
    {
        if (response == null || response.items == null)
        {
            Debug.Log("Response or items is null");
            return;
        }

        backpackItems[0] = response.items.item_1;
        backpackItems[1] = response.items.item_2;
        backpackItems[2] = response.items.item_3;
        backpackItems[3] = response.items.item_4;
        backpackItems[4] = response.items.item_5;
        backpackItems[5] = response.items.item_6;
        backpackItems[6] = response.items.item_7;
        backpackItems[7] = response.items.item_8;
        backpackItems[8] = response.items.item_9;
        backpackItems[9] = response.items.item_10;
        backpackItems[10] = response.items.item_11;
        backpackItems[11] = response.items.item_12;
        backpackItems[12] = response.items.item_13;
        backpackItems[13] = response.items.item_14;
        backpackItems[14] = response.items.item_15;
        backpackItems[15] = response.items.item_16;
        backpackItems[16] = response.items.item_17;
        backpackItems[17] = response.items.item_18;
        backpackItems[18] = response.items.item_19;
        backpackItems[19] = response.items.item_20;
    }

    [System.Serializable]
    public class Response
    {
        public string status;
        public string message;
        public Items items;

        [System.Serializable]
        public class Items
        {
            public int item_1;
            public int item_2;
            public int item_3;
            public int item_4;
            public int item_5;
            public int item_6;
            public int item_7;
            public int item_8;
            public int item_9;
            public int item_10;
            public int item_11;
            public int item_12;
            public int item_13;
            public int item_14;
            public int item_15;
            public int item_16;
            public int item_17;
            public int item_18;
            public int item_19;
            public int item_20;
        }
    }
}
