using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    public InputField idInputField;
    public InputField passwordInputField;
    public Text resultText;

    public void Register()
    {
        string id = idInputField.text;
        string password = passwordInputField.text;
        StartCoroutine(RegisterCoroutine(id, password));
    }

    IEnumerator RegisterCoroutine(string id, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("id", id);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://localhost//TeamProject/Login.php", form);
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
