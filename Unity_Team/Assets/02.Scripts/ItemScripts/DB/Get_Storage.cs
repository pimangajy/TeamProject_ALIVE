using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Get_Storage : MonoBehaviour
{
    public int photonId;
    public int[] backpackItems = new int[50]; // 아이템 배열

    [SerializeField]
    Player player;


    private void Start()
    {
        photonId = player.pv.viewID;
        StartCoroutine(GetStorageCoroutine());
    }

    public void LoadStorage()
    {
        photonId = player.pv.viewID;
        StartCoroutine(GetStorageCoroutine());
    }

    IEnumerator GetStorageCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("photon_id", photonId);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost/TeamProject/Get_Storage.php", form);
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
                    Debug.Log(photonId + "창고 불러오기 성공");
                    UpdateBackpack(responseJson);
                }
                else
                {
                    // Debug.Log(responseJson.message);
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

        backpackItems[20] = response.items.item_21;
        backpackItems[21] = response.items.item_22;
        backpackItems[22] = response.items.item_23;
        backpackItems[23] = response.items.item_24;
        backpackItems[24] = response.items.item_25;
        backpackItems[25] = response.items.item_26;
        backpackItems[26] = response.items.item_27;
        backpackItems[27] = response.items.item_28;
        backpackItems[28] = response.items.item_29;
        backpackItems[29] = response.items.item_30;

        backpackItems[30] = response.items.item_31;
        backpackItems[31] = response.items.item_32;
        backpackItems[32] = response.items.item_33;
        backpackItems[33] = response.items.item_34;
        backpackItems[34] = response.items.item_35;
        backpackItems[35] = response.items.item_36;
        backpackItems[36] = response.items.item_37;
        backpackItems[37] = response.items.item_38;
        backpackItems[38] = response.items.item_39;
        backpackItems[39] = response.items.item_40;

        backpackItems[40] = response.items.item_41;
        backpackItems[41] = response.items.item_42;
        backpackItems[42] = response.items.item_43;
        backpackItems[43] = response.items.item_44;
        backpackItems[44] = response.items.item_45;
        backpackItems[45] = response.items.item_46;
        backpackItems[46] = response.items.item_47;
        backpackItems[47] = response.items.item_48;
        backpackItems[48] = response.items.item_49;
        backpackItems[49] = response.items.item_50;
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

            public int item_21;
            public int item_22;
            public int item_23;
            public int item_24;
            public int item_25;
            public int item_26;
            public int item_27;
            public int item_28;
            public int item_29;
            public int item_30;

            public int item_31;
            public int item_32;
            public int item_33;
            public int item_34;
            public int item_35;
            public int item_36;
            public int item_37;
            public int item_38;
            public int item_39;
            public int item_40;

            public int item_41;
            public int item_42;
            public int item_43;
            public int item_44;
            public int item_45;
            public int item_46;
            public int item_47;
            public int item_48;
            public int item_49;
            public int item_50;
        }
    }
}
