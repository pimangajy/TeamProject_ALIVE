using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Canvers : MonoBehaviour, IPointerClickHandler
{
    public GameObject panel;
    Vector3 defaultPos;

    void Awake()
    {
        // ��� �޴� �Ҵ�
        panel = GameObject.Find("Canvas/Panel/R_Click");
    }

    private void Start()
    {
        // ��Ŭ�� �޴��� �⺻ ��ġ 
        if(panel != null)
            defaultPos = panel.transform.position;
    }

    public void R_ClickPos()
    {
        if(panel == null)
        {
            panel = GameObject.Find("Canvas/Panel/R_Click");
            defaultPos = panel.transform.position;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(panel != null)
            {
                panel.SetActive(false);
                panel.transform.position = defaultPos;
            }
        }
    }
}
