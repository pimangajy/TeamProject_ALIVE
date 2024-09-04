using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ACC_Create : MonoBehaviour
{
    public InputField idInputField;
    public InputField passwordInputField;
    public InputField nameInputField;

    public void Register()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;
        string name = nameInputField.text;
        StartCoroutine(RegisterCoroutine(id, password, name));
    }

    IEnumerator RegisterCoroutine(string id, string password, string name)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("password", password);
        form.AddField("name", name);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost//TeamProject/ACC_Create.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
        }
    }
}
