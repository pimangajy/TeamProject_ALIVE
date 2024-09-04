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

    Transform canvers;                 // Ui�� �ҼӵǾ� �ִ� �� ��� Canvers Transfrom
    public Transform previousParent;          // �ش� ������Ʈ�� ������ �Ҽ� �Ǿ��ִ� �θ� Transfrom
    RectTransform rect;                // Ui ��ġ ��� ���� RectTransfrom
    CanvasGroup canversgroup;          // Ui�� ���İ��� ��ȣ�ۿ��� ���� CanversGroup
    Transform parent;

    public GameObject panel;

    public int Pos;

    Vector3 lo;

    // �巡�������� Ȯ��
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
        // �׻� ũ�⸦ �����ϰ� ����
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

    // ���� ������Ʈ�� �巡�� �����Ҷ� 1ȸȣ��
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
                // �ٸ� ������ â�� ������ ����
            }
        }

        drag = true;

        // panel.SetActive(false);

        // �巡�� ������ �θ� ���� ����
        previousParent = transform.parent;

        // ���� �巡�� ���� ������Ʈ�� �ֻ�ܿ� �ö������ 
        transform.SetParent(canvers);  // �θ� ������Ʈ�� canvers�� ����
        transform.SetAsLastSibling();  // ���� �տ� ���̵��� ���� ������ �ڽ����� ����

        // �巡�� ������ ������Ʈ�� �ϳ��� �ƴ� �ڽĵ��� �������� �ֱ� ������ CanversGroup���� ����
        // ���İ��� ���߰� ���� �浹 ó���� �ȵŰ�
        canversgroup.alpha = 0.6f;
        canversgroup.blocksRaycasts = false;
    }

    // ���� ������Ʈ�� �巡�� ���� �� ������ ȣ�� 
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ��ũ������ UI ��ġ�� ���콺 ��ġ�� ����
        rect.position = eventData.position;
    }

    // ���� �巡�׸� �����Ҷ� 1ȸ ȣ��

    public void OnEndDrag(PointerEventData eventData)
    {
        // OnDrop���� �ʰ� �ߵ�!!!!!!!

        // �巡�׸� �����ϸ� �θ� canvers�� �Ǳ� ������
        // �巡�׸� �����Ҷ� �θ� canvers��� �߸� �����ű� ������
        // �巡�װ� ������ �θ�� ����
        if (transform.parent == canvers)
        {
            // �������� �����Ǿ� �ִ� �θ�� ��ġ, �θ� ����
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

    // ���콺 Ŭ���� �̺�Ʈ
    public void OnPointerClick(PointerEventData eventData)
    {

        // ��Ŭ���� Ŭ���� ������Ʈ�� ������ R_Click_Button�� �ѱ�
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            panel.transform.position = eventData.position;
            // ��Ŭ���� ������Ʈ ������ �ѱ�
            panel.GetComponent<R_Click_Button>().ItemSet(gameObject);
            // panel�� ����Ű�鼭 �������� ��Ŭ�� �ص� ��ư ����� �ֽ�ȭ �ǵ��� ��
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
