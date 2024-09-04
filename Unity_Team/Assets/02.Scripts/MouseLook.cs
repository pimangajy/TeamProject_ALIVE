using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MouseLook : MonoBehaviour
{
    int test;

    [SerializeField]
    UIController uIController;
    [SerializeField]
    Inventory inventory;
    [SerializeField]
    Player player;

    public GameObject storage;
    

    public float rayDistance = 100f; // ���� �Ÿ�
    public Material[] outlineMaterial;  // ������ �� ����� OutlineMaterial
    public Material[] originalMaterial;  // ������ Material �����
    public Renderer hitRenderer;  // �浿 ������Ʈ�� ���������� �����

    public Camera specificCamera;
    public AudioListener lis;

    PhotonView pv;
    public PhotonView invenView;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        lis = GetComponent<AudioListener>();

        if (!pv.isMine)
        {
            specificCamera.enabled = false;
            lis.enabled = false;
        }
    }

    private void Start()
    {
        // PhotonView invenView = inventory.GetComponent<PhotonView>();
    }

    void Update()
    {

        if (specificCamera != null)
        {
            Vector3 screenCenter = new Vector3(specificCamera.pixelWidth / 2, specificCamera.pixelHeight / 2, 0);

            // ȭ�� ��ǥ�� ���� ��ǥ�� ��ȯ
            Ray ray = specificCamera.ScreenPointToRay(screenCenter);
            // ī�޶��� �������� ���� ����
            //Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hit;

            // ���� ����� ǥ��
            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red);

            // ���̸� �߻��Ͽ� �浹 ����
            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                // ���� �浹�߰� �̹� hitRenderer�ȿ� �������� ����ִٸ�
                if (hitRenderer != null && hitRenderer != hit.collider.gameObject.GetComponent<Renderer>())
                {
                    if (hitRenderer.transform.gameObject.tag == "Item")
                    {
                        // ���� ����ִ� �������� ���� �������� �ٲ� ���� ����
                        hitRenderer.material = originalMaterial[hitRenderer.GetComponent<FeildItem>().matNum];
                    }
                }

                // �浹�� ������Ʈ ����
                hitRenderer = hit.collider.gameObject.GetComponent<Renderer>();

                // ������Ʈ�� �浹�߰� �±װ� �������̰� use�� false���
                if (hit.transform.gameObject.tag == "Item" && !hit.collider.gameObject.GetComponent<FeildItem>().use)
                {
                    // �浹�� ������Ʈ ����
                    hitRenderer = hit.collider.gameObject.GetComponent<Renderer>();
                    // ���׸����� �ٲ�
                    hitRenderer.material = outlineMaterial[hitRenderer.gameObject.GetComponent<FeildItem>().matNum];

                    if (Input.GetKeyDown(KeyCode.F) )
                    {
                        hit.transform.gameObject.GetComponent<FeildItem>().F = true;
                        //invenView.RPC("ItemPlus", PhotonTargets.AllBuffered, hit.transform.GetComponent<FeildItem>().ItemPrefab);
                        inventory.ItemPlus(hit.transform.GetComponent<FeildItem>().ItemPrefab);
                        PhotonView itemID = hit.transform.gameObject.GetComponent<PhotonView>();
                        if(itemID.isMine)
                        {
                            PhotonNetwork.Destroy(itemID.gameObject);
                        }else
                        {
                            pv.RPC("DestroyItem", PhotonTargets.MasterClient, itemID.viewID);
                        }
                    }
                }

                if (hit.transform.gameObject.tag == "Gun")
                {
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        hit.transform.gameObject.GetComponent<FeildItem>().F = true;
                        //invenView.RPC("ItemPlus", PhotonTargets.AllBuffered, hit.transform.GetComponent<FeildItem>().ItemPrefab);
                        inventory.ItemPlus(hit.transform.GetComponent<FeildItem>().ItemPrefab);
                        PhotonView itemID = hit.transform.gameObject.GetComponent<PhotonView>();
                        if (itemID.isMine)
                        {
                            PhotonNetwork.Destroy(itemID.gameObject);
                        }
                        else
                        {
                            pv.RPC("DestroyItem", PhotonTargets.MasterClient, itemID.viewID);
                        }
                    }

                }

                //// �浹�� ������Ʈ�� �ױװ� Door���
                //if (hit.transform.gameObject.tag == "Door")
                //{
                //    if (Input.GetKeyDown(KeyCode.F))
                //    {
                //        hit.transform.gameObject.GetComponent<DoorMech>().OpenDoor();
                //    }
                //}

                if (hit.transform.gameObject.tag == "Exit" && (Input.GetKeyDown(KeyCode.F)))
                {
                    pv.RPC("EscapeRaid", PhotonTargets.AllBuffered);
                }


                if (hit.transform.gameObject.tag == "Hide_Out")
                {
                    uIController.open_Storage = true;
                    HideOut_Object hideOut = hit.transform.parent.gameObject.GetComponent<HideOut_Object>();
                    if (hideOut.tabletype == TableType.Escape_from_Tarkov && PhotonNetwork.isMasterClient)
                    {
                        uIController.open_Storage_txt.text = "Escape";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            PhotonView playerid = player.GetComponent<PhotonView>();
                            ScenesManager.instance.LoadStage(3);
                            ScenesManager scenesManager = GameObject.Find("ScenesManager").gameObject.GetComponent<ScenesManager>();
                            scenesManager.deathCount = 0;
                            uIController.open_Storage = false;
                        }
                    }
                    else if (hideOut.tabletype == TableType.Storage)
                    {
                        uIController.open_Storage_txt.text = "Storage";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            Storage storagpanel = hideOut.storage;
                            storagpanel.setPV(player.GetComponent<PhotonView>());
                            player.OpenBackPack();
                            storage.SetActive(true);
                            uIController.open_Storage = false;
                        }
                    }
                    else if (hideOut.tabletype == TableType.Work_Table)
                    {
                        uIController.open_Storage_txt.text = "Work_Table";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            PhotonView playerid = player.GetComponent<PhotonView>();
                            hideOut.OpenWork(playerid.viewID);
                            uIController.open_Storage = false;
                        }
                    }
                    else if (hideOut.tabletype == TableType.Kitchen)
                    {
                        uIController.open_Storage_txt.text = "Kitchen";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            PhotonView playerid = player.GetComponent<PhotonView>();
                            hideOut.Kichin(playerid.viewID);
                            uIController.open_Storage = false;
                        }
                    }
                    else if (hideOut.tabletype == TableType.Medical_Center)
                    {
                        uIController.open_Storage_txt.text = "Medical_Center";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            PhotonView playerid = player.GetComponent<PhotonView>();
                            hideOut.Heal(playerid.viewID);
                            uIController.open_Storage = false;
                        }
                    }
                    else if (hideOut.tabletype == TableType.Medical_Table)
                    {
                        uIController.open_Storage_txt.text = "Medical_Table";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            PhotonView playerid = player.GetComponent<PhotonView>();
                            hideOut.OpenMedic(playerid.viewID);
                            uIController.open_Storage = false;
                        }
                    }
                    else if (hideOut.tabletype == TableType.CommandSenter)
                    {
                        uIController.open_Storage_txt.text = "Command";
                        if (Input.GetKeyDown(KeyCode.F))
                        {
                            PhotonView playerid = player.GetComponent<PhotonView>();
                            hideOut.CommandSenter(playerid.viewID);
                            Debug.Log(hideOut.hide_Out_Setting.levelData.Base);
                            hideOut.hide_Out_Setting.Getdata();
                            uIController.hide_out_hp.value = hideOut.hide_Out_Setting.localhp;
                        }
                    }
                }
                else
                {
                    uIController.open_Storage = false;
                }
            }
            else
            {
                // ���̰� �ƹ��͵� �������� �ʾ��� �� ���� ����
                if (hitRenderer != null)
                {
                    if (hitRenderer.transform.gameObject.tag == "Item")
                        hitRenderer.material = originalMaterial[hitRenderer.GetComponent<FeildItem>().matNum];

                    uIController.open_Storage = false;
                    hitRenderer = null;
                }
            }
        }
    }

    [PunRPC]
    public void EscapeRaid()
    {
        RaidManager raidManager =  GameObject.Find("RaidManager").GetComponent<RaidManager>();
        Debug.Log("!!!");
        raidManager.ActivateEscape();
    }

  

    [PunRPC]
    public void DestroyItem(int ID)
    {
        PhotonView id = PhotonView.Find(ID);
        if(id != null)
        {
            id.gameObject.GetComponent<FeildItem>().GetItem();
            // id.gameObject.SetActive(false);
           // PhotonNetwork.Destroy(id.gameObject);
        }
    }

    

    void ActivateEscape()
    {
      
    }

   
}
