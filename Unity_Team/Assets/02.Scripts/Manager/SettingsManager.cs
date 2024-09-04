using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Dropdown dropdown;        // 해상도 조절
    public Slider soundSlider;       // 사운드 조절 슬라이더
    public Slider SensitivitySlider;  // 민감도 조절 슬라이더
    public GameObject settingsPanel; // 환경설정 UI
    public GameObject BtnPannel;     // 버튼 판넬
    Player player;

    public static SettingsManager instance;

    private void Awake()
    {
        player = transform.root.transform.root.GetComponent<Player>();
    }

    void Start()
    {

    }

    public void SensitivitySet()
    {
        if(SensitivitySlider.value == 0)
        {
            SensitivitySlider.value = 0.05f;
        }
        player.mouseSpeed = SensitivitySlider.value * 600.0f;    // 민감도 실린더의 기본값은 0.5로
    }


    public void ChangeResolution(int value)       // 해상도 설정
    {
        if (value == 0)
        {
            Screen.SetResolution(1920, 1080, true);
            Debug.Log("Screen.SetResolution(1920, 1080, true);");       
        }
        else if (value == 1)
        {
            Screen.SetResolution(1600, 900, true);
        }
        else if (value == 2)
        {
            Screen.SetResolution(1280, 720, true);
        }
    }

    public void setVolume()             // 볼륨 설정
    {
        SoundManager.instance.SetVolume(soundSlider.value);
    }

    public void ReturnGameBtnClick()  // 게임으로 돌아가기
    {
        player.OpenSetting();
    }

    public void SettingBtnClick()   // 게임 설정 버튼 클릭
    {
        BtnPannel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void SettingQuitBtnClick()  // 게임 설정 나기기 버튼 
    {
        BtnPannel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void SoundBtnClick()         // 버튼 클릭시 사운드 재생
    {
        SoundManager.instance.BtnClick();
    }

    public void QuitGame()  // 게임 나가기
    {
        PhotonNetwork.LeaveRoom();
        ScenesManager.instance.ExitRoom();
    }

    private void OnEnable()    // 켜질때 초기상태로
    {
        BtnPannel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
