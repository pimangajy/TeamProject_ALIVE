using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : Item, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    Storage storage;

    Transform canvers;                 // Ui가 소속되어 있는 최 상단 Canvers Transfrom
    public Transform previousParent;          // 해당 오브젝트가 직전에 소속 되어있던 부모 Transfrom
    RectTransform rect;                // Ui 위치 제어를 위한 RectTransfrom
    CanvasGroup canversgroup;          // Ui의 알파값과 상호작용을 위한 CanversGroup
    Transform parent;

    public GameObject panel;

    public int Pos;

    Vector3 lo;

    // 드래그중인지 확인
    bool drag = false;

    void Awake()
    {
        canvers = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canversgroup = GetComponent<CanvasGroup>();
        panel = GameObject.Find("Canvas/Panel/R_Click");
    }

    private void OnEnable()
    {
        panel = GameObject.Find("Canvas/Panel/R_Click");
    }

    private void Start()
    {
        // defaultPos = panel.transform.position;
        lo = rect.localPosition;
        lo.z = 0;
    }

    void Update()
    {
        // 항상 크기를 일정하게 유지
        if(!drag)
        {
            rect.offsetMax = new Vector2(-10, -10);
            rect.offsetMin = new Vector2(10, 10);
            //rect.anchoredPosition3D = lo;
        }
    }

    public void ImageChainge()
    {
        if(transform.parent != null)
        {
            GameObject p = transform.parent.gameObject;
            if(p.GetComponent<InventoryManager>().inventype == InventoryManager.inventoryType.Equipment_Slot)
            {
                GetComponent<Image>().sprite = Weapon_Img;
            }else { GetComponent<Image>().sprite = itemIcon; }

        }
    }

    // 현재 오브젝트를 드래그 시작할때 1회호출
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(gameObject.transform.root.gameObject.GetComponent<Player>())
        {
            inventory = gameObject.transform.root.gameObject.GetComponent<Player>().inventory;
        }else
        {
            storage = gameObject.transform.root.GetChild(0).GetComponent<Storage>();
            if(parent != null)
            {
                // 다른 유저의 창고 아이템 삭제
            }
        }

        drag = true;

        // panel.SetActive(false);

        // 드래그 직전에 부모 정보 받음
        previousParent = transform.parent;

        // 현재 드래그 중인 오브젝트가 최상단에 올라오도록 
        transform.SetParent(canvers);  // 부모 오브젝트를 canvers로 설정
        transform.SetAsLastSibling();  // 가장 앞에 보이도록 가장 마지막 자식으로 설정

        // 드래그 가능한 오브젝트가 하나다 아닌 자식들을 가질수도 있기 때문에 CanversGroup으로 통제
        // 알파값을 낮추고 광선 충돌 처리가 안돼게
        canversgroup.alpha = 0.6f;
        canversgroup.blocksRaycasts = false;
    }

    // 현재 오브젝트다 드래그 중일 때 프래임 호출 
    public void OnDrag(PointerEventData eventData)
    {
        // 현재 스크린상의 UI 위치를 마우스 위치로 설정
        rect.position = eventData.position;
    }

    // 현재 드래그를 종료할때 1회 호출

    public void OnEndDrag(PointerEventData eventData)
    {
        // OnDrop보다 늦게 발동!!!!!!!

        // 드래그를 시작하면 부모가 canvers로 되기 때문에
        // 드래그를 종료할때 부모가 canvers라면 잘못 놓은거기 때문에
        // 드래그가 시작한 부모로 설정
        if (transform.parent == canvers)
        {
            // 마지막에 설정되어 있던 부모로 위치, 부모 설정
            //transform.SetParent(previousParent);
            //rect.position = previousParent.GetComponent<RectTransform>().position;
            if(inventory != null)
            {
                for (int i = 0; i < inventory.FieldBackPackSpace.Length; i++)
                {
                    if (inventory.FieldBackPackSpace[i].childCount == 0)
                    {
                        previousParent = inventory.FieldBackPackSpace[i];
                        transform.SetParent(previousParent);
                        rect.position = previousParent.GetComponent<RectTransform>().position;
                        break;
                    }
                }
            }else if(storage != null)
            {
                for (int i = 0; i < storage.storagesize; i++)
                {
                    if (storage.storageSpace[i].childCount == 0)
                    {
                        previousParent = storage.storageSpace[i];
                        transform.SetParent(previousParent);
                        rect.position = previousParent.GetComponent<RectTransform>().position;
                        parent = transform.parent;
                        break;
                    }
                }
            }
            
        }
        
        canversgroup.alpha = 1.0f;
        canversgroup.blocksRaycasts = true;

        drag = false;

        if(transform.parent.gameObject.GetComponent<StorageSpace>())
        {
            Debug.Log("AAA");
        }

        ImageChainge();

    }

    // 마우스 클릭시 이벤트
    public void OnPointerClick(PointerEventData eventData)
    {

        // 우클릭시 클릭한 오브젝트의 정보를 R_Click_Button에 넘김
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            panel.transform.position = eventData.position;
            // 우클릭한 오브젝트 정보를 넘김
            panel.GetComponent<R_Click_Button>().ItemSet(gameObject);
            // panel을 껏다키면서 연속으로 우클릭 해도 버튼 목록이 최신화 되도록 함
            panel.SetActive(false);
            panel.SetActive(true);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            panel.SetActive(false);
            // panel.transform.position = defaultPos;
        }
    }
}
