using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class LoginResponse
{
	public string status;
	public string nickname;
	public string message;
}

public class LoginManager : MonoBehaviour
{
	public InputField logIDInput;        // �α��� ���̵�
	public InputField logPassWordInput;  // �α��� �н�����
	public GameObject createAccPanel; // ȸ������ UI
	public GameObject loginPanel;     // �α��� UI
	public GameObject titleText;
	public GameObject errorBtn;        // ���� ��ư
	public Text errorText;        // ���� �ؽ�Ʈ

	public InputField accIDInput;       // ȸ������ ���̵�
	public InputField accPassWordInput;  // ȸ������ �н�����
	public InputField accNickNameInput;  // ȸ������ �г���




	string LoginURL = "http://192.168.0.53/TeamProject/login.php";        // �α��� php
	string CreateAccURL = "http://192.168.0.53/TeamProject/CreateAcc.php"; // ȸ������ php

	public void CreatNewAccBtn()      // ȸ������ ��ư
    {
		loginPanel.SetActive(false);
		createAccPanel.SetActive(true);
		titleText.SetActive(false);
		accIDInput.text = string.Empty;
		accPassWordInput.text = string.Empty;
		accNickNameInput.text = string.Empty;
	}

	public void BackBtn()                // ȸ������ ui���� �ڷΰ��� ��ư ������ �α��� ui�� ��ȯ
	{
		createAccPanel.SetActive(false);
		loginPanel.SetActive(true);
		titleText.SetActive(true);
		logIDInput.text = string.Empty;
		logPassWordInput.text = string.Empty;
	}

	public void LoginToDB_Btn()      // php�� �α��� ���� ������ ��ư
	{
		StartCoroutine(LoginToDB(logIDInput.text, logPassWordInput.text));
	}

	public void CreateAccToDB_Btn()   // php�� ȸ������ ���� ������ ��ư
	{
		Debug.Log(accIDInput.text);
		Debug.Log(accPassWordInput.text);
		Debug.Log(accNickNameInput.text);
		StartCoroutine(CreateAccToDB(accIDInput.text, accPassWordInput.text, accNickNameInput.text));
	}

	IEnumerator LoginToDB(string username, string password)   // �α��� �ڷ�ƾ
	{
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField("passwordPost", password);

		WWW www = new WWW(LoginURL, form);

		yield return www;

		string response = www.text;
		LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);


		if (loginResponse.status == "login")     // �α��� ����
        {
			PlayerPrefs.SetString("nickName", loginResponse.nickname);
			
			SceneManager.LoadScene(1);
        }
		else if(loginResponse.status == "password") // ��й�ȣ�� Ʋ��
		{
			StartCoroutine(PasswordError());
		}
		else if (loginResponse.status == "user") // ������ ã�� �� ����
		{
			StartCoroutine(UserError());
		}


	}

	IEnumerator CreateAccToDB(string username, string password, string nickname)  // ȸ������ �ڷ�ƾ
	{
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField("passwordPost", password);
		form.AddField("nicknamePost", nickname);

		WWW www = new WWW(CreateAccURL, form);

		yield return www;

		string response = www.text;
		LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);

		if (loginResponse.status == "error")  // ���̵� �Ǵ� �г����� �̹� ��� ���Դϴ�
		{
			StartCoroutine(CreateError());
		}
		else if (loginResponse.status == "success") // ������ ���������� ����
		{
			createAccPanel.SetActive(false);
			loginPanel.SetActive(true);
		}
		else if(loginResponse.status == "empty") // �Է����� ���� ������ �ֽ��ϴ�
		{
			StartCoroutine(EmptyError());
        }
	}

	public void GuestBtn()
    {
		string nickName = "Guest" + Random.Range(1, 9999);
		PlayerPrefs.SetString("nickName", nickName);
		SceneManager.LoadSceneAsync(1);
	}
	IEnumerator CreateError()
    {
		errorBtn.SetActive(true);
		errorText.text = "���̵� �Ǵ� �г����� �̹� ��� ���Դϴ�";
		accIDInput.text = string.Empty;
		accPassWordInput.text = string.Empty;
		accNickNameInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

	IEnumerator EmptyError()
    {
		errorBtn.SetActive(true);
		errorText.text = "�Է����� ���� ������ �ֽ��ϴ�.";
		accIDInput.text = string.Empty;
		accPassWordInput.text = string.Empty;
		accNickNameInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

	IEnumerator PasswordError()
    {
		errorBtn.SetActive(true);
		errorText.text = "��й�ȣ�� Ʋ�Ƚ��ϴ�.";
		logPassWordInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

	IEnumerator UserError()
	{
		errorBtn.SetActive(true);
		errorText.text = "������ ã�� �� �����ϴ�";
		logIDInput.text = string.Empty;
		logPassWordInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

}


