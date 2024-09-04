using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DragWindow : MonoBehaviour, IBeginDragHandler, IDragHandler
{

    [SerializeField]
    GameObject Panel;

    private void Start()
    {
        Panel = GameObject.Find("Canvas/Panel");

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(Panel.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnLoadeScnene(Scene scene, LoadSceneMode mode)
    {
        Panel = GameObject.Find("Canvas/Panel");

    }
}
