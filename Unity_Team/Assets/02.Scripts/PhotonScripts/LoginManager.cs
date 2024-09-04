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
	public InputField logIDInput;        // 로그인 아이디
	public InputField logPassWordInput;  // 로그인 패스워드
	public GameObject createAccPanel; // 회원가입 UI
	public GameObject loginPanel;     // 로그인 UI
	public GameObject titleText;
	public GameObject errorBtn;        // 에러 버튼
	public Text errorText;        // 에러 텍스트

	public InputField accIDInput;       // 회원가입 아이디
	public InputField accPassWordInput;  // 회원가입 패스워드
	public InputField accNickNameInput;  // 회원가입 닉네임




	string LoginURL = "http://192.168.0.53/TeamProject/login.php";        // 로그인 php
	string CreateAccURL = "http://192.168.0.53/TeamProject/CreateAcc.php"; // 회원가입 php

	public void CreatNewAccBtn()      // 회원가입 버튼
    {
		loginPanel.SetActive(false);
		createAccPanel.SetActive(true);
		titleText.SetActive(false);
		accIDInput.text = string.Empty;
		accPassWordInput.text = string.Empty;
		accNickNameInput.text = string.Empty;
	}

	public void BackBtn()                // 회원가입 ui에서 뒤로가기 버튼 누르면 로그인 ui로 전환
	{
		createAccPanel.SetActive(false);
		loginPanel.SetActive(true);
		titleText.SetActive(true);
		logIDInput.text = string.Empty;
		logPassWordInput.text = string.Empty;
	}

	public void LoginToDB_Btn()      // php로 로그인 정보 보내는 버튼
	{
		StartCoroutine(LoginToDB(logIDInput.text, logPassWordInput.text));
	}

	public void CreateAccToDB_Btn()   // php로 회원가입 정보 보내는 버튼
	{
		Debug.Log(accIDInput.text);
		Debug.Log(accPassWordInput.text);
		Debug.Log(accNickNameInput.text);
		StartCoroutine(CreateAccToDB(accIDInput.text, accPassWordInput.text, accNickNameInput.text));
	}

	IEnumerator LoginToDB(string username, string password)   // 로그인 코루틴
	{
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField("passwordPost", password);

		WWW www = new WWW(LoginURL, form);

		yield return www;

		string response = www.text;
		LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);


		if (loginResponse.status == "login")     // 로그인 성공
        {
			PlayerPrefs.SetString("nickName", loginResponse.nickname);
			
			SceneManager.LoadScene(1);
        }
		else if(loginResponse.status == "password") // 비밀번호가 틀림
		{
			StartCoroutine(PasswordError());
		}
		else if (loginResponse.status == "user") // 유저를 찾을 수 없음
		{
			StartCoroutine(UserError());
		}


	}

	IEnumerator CreateAccToDB(string username, string password, string nickname)  // 회원가입 코루틴
	{
		WWWForm form = new WWWForm();
		form.AddField("usernamePost", username);
		form.AddField("passwordPost", password);
		form.AddField("nicknamePost", nickname);

		WWW www = new WWW(CreateAccURL, form);

		yield return www;

		string response = www.text;
		LoginResponse loginResponse = JsonUtility.FromJson<LoginResponse>(response);

		if (loginResponse.status == "error")  // 아이디 또는 닉네임이 이미 사용 중입니다
		{
			StartCoroutine(CreateError());
		}
		else if (loginResponse.status == "success") // 계정이 성공적으로 생성
		{
			createAccPanel.SetActive(false);
			loginPanel.SetActive(true);
		}
		else if(loginResponse.status == "empty") // 입력하지 않은 정보가 있습니다
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
		errorText.text = "아이디 또는 닉네임이 이미 사용 중입니다";
		accIDInput.text = string.Empty;
		accPassWordInput.text = string.Empty;
		accNickNameInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

	IEnumerator EmptyError()
    {
		errorBtn.SetActive(true);
		errorText.text = "입력하지 않은 정보가 있습니다.";
		accIDInput.text = string.Empty;
		accPassWordInput.text = string.Empty;
		accNickNameInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

	IEnumerator PasswordError()
    {
		errorBtn.SetActive(true);
		errorText.text = "비밀번호가 틀렸습니다.";
		logPassWordInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

	IEnumerator UserError()
	{
		errorBtn.SetActive(true);
		errorText.text = "유저를 찾을 수 없습니다";
		logIDInput.text = string.Empty;
		logPassWordInput.text = string.Empty;
		yield return new WaitForSeconds(1.5f);
		errorBtn.SetActive(false);
	}

}


