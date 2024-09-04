using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager instance;
    public int deathCount;
    public AsyncOperation ao;
    bool ending = false;
    


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void LoadStage(int level)
    {
        StartCoroutine(LoadStagecor(level));
    }

    public bool firstBase = true;
    IEnumerator LoadStagecor(int level)  // ¾À ÀüÈ¯ ÄÚ·çÆ¾
    {
        PhotonNetwork.isMessageQueueRunning = false;
        PhotonNetwork.LoadLevel(4);  // ·Îµù¾À

        yield return new WaitForSeconds(2.0f);
        ao = PhotonNetwork.LoadLevelAsync(level);
        yield return ao;
        PhotonNetwork.isMessageQueueRunning = true;


        if (level == 2)
        {
            Hide_Out_Setting hide_Out_Setting = GameObject.Find("BaseController").GetComponent<Hide_Out_Setting>();

            if (firstBase == true)
            {
                firstBase = false;
            }
            else
            {
                hide_Out_Setting.hide_out_Attack(deathCount);
                if (hide_Out_Setting.levelData.Base <= 0 && ending == false)
                {
                    ending = true;
                    StartCoroutine(LoadStagecor(5));
                }
            }

            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                player.GetComponent<Player>().uIController.hide_out_hp.value = hide_Out_Setting.levelData.Base;
            }
        }

        if (level == 3)
        {
            foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                 player.GetComponent<Player>().uIController.realTime = 1800;
            } 
        }
    }

    public void ExitRoom()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync(1);
    }

}
