using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TableType
{
    Work_Table,
    Storage,
    Medical_Table,
    Kitchen,
    Medical_Center,
    Escape_from_Tarkov,
    CommandSenter
}

public class HideOut_Object : MonoBehaviour
{
    [SerializeField]
    public Hide_Out_Setting.LevelData levelData;
    [SerializeField]
    public Hide_Out_Setting hide_Out_Setting;

    public TableType tabletype;


    [SerializeField]
    public Storage storage;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        hide_Out_Setting = GameObject.Find("BaseController").GetComponent<Hide_Out_Setting>();
    }

    public void CommandSenter(int platerID)
    {
        hide_Out_Setting.CommandSenter(platerID);
    }

    public void Heal(int platerID)
    {
        hide_Out_Setting.Heal(platerID);
    }
    public void Kichin(int platerID)
    {
        hide_Out_Setting.Kichin(platerID);
    }

    public void OpenWork(int platerID)
    {
        PhotonView playerpv = PhotonView.Find(platerID);
        GameObject playerobj = playerpv.gameObject;
        Player player = playerobj.GetComponent<Player>();
        Transform uicon = playerobj.transform.Find("UIManager");
        UIController uiController = uicon.GetComponent<UIController>();

        player.OpenHideOut();
        uiController.hideout.SetActive(true);
        uiController.medittable.SetActive(false);
        uiController.worktable.SetActive(true);
    }

    public void OpenMedic(int platerID)
    {
        PhotonView playerpv = PhotonView.Find(platerID);
        GameObject playerobj = playerpv.gameObject;
        Player player = playerobj.GetComponent<Player>();
        Transform uicon = playerobj.transform.Find("UIManager");
        UIController uiController = uicon.GetComponent<UIController>();

        player.OpenHideOut();
        uiController.hideout.SetActive(true);
        uiController.worktable.SetActive(false);
        uiController.medittable.SetActive(true);
    }

    public void Escape_from_Tarkov()
    {
        levelData = hide_Out_Setting.levelData;
    }

    public void Hide_Out_LevelUp(int kitchen, int medical, int base_)
    {
        Debug.Log("Hide_Out_LevelUp");
        hide_Out_Setting.OnButtonPress(kitchen, medical, base_);
    }
}
