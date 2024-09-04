using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public Dropdown dropdown;        // �ػ� ����
    public Slider soundSlider;       // ���� ���� �����̴�
    public Slider SensitivitySlider;  // �ΰ��� ���� �����̴�
    public GameObject settingsPanel; // ȯ�漳�� UI
    public GameObject BtnPannel;     // ��ư �ǳ�
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
        player.mouseSpeed = SensitivitySlider.value * 600.0f;    // �ΰ��� �Ǹ����� �⺻���� 0.5��
    }


    public void ChangeResolution(int value)       // �ػ� ����
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

    public void setVolume()             // ���� ����
    {
        SoundManager.instance.SetVolume(soundSlider.value);
    }

    public void ReturnGameBtnClick()  // �������� ���ư���
    {
        player.OpenSetting();
    }

    public void SettingBtnClick()   // ���� ���� ��ư Ŭ��
    {
        BtnPannel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void SettingQuitBtnClick()  // ���� ���� ����� ��ư 
    {
        BtnPannel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void SoundBtnClick()         // ��ư Ŭ���� ���� ���
    {
        SoundManager.instance.BtnClick();
    }

    public void QuitGame()  // ���� ������
    {
        PhotonNetwork.LeaveRoom();
        ScenesManager.instance.ExitRoom();
    }

    private void OnEnable()    // ������ �ʱ���·�
    {
        BtnPannel.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
