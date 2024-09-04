using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObservePlayer : MonoBehaviour
{

    public List<MouseLook> playerCamera;
    public List<Player> playerList;
    int camerainx = 0;       // ī�޶� �ε���
    public Text playerName;

    private void Awake()
    {
        playerCamera = new List<MouseLook>();      
        playerList = new List<Player>();
    }

    private void OnEnable()         // �÷��̾ ������ UI�� ����
    {
        foreach (GameObject _camera in GameObject.FindGameObjectsWithTag("Player"))             // �÷��̾� ã��
        {
            Player player = _camera.GetComponent<Player>();
            MouseLook _playerCamera = _camera.GetComponentInChildren<MouseLook>();               // ī�޶� �ִ� ������Ʈ
            if (_playerCamera != null && player != null)
            {
                if (_playerCamera.lis.enabled == false && _playerCamera.specificCamera.enabled == false)    // ����� �����ʿ� ī�޶� ���������� �ٸ��÷��̾�
                {
                    playerList.Add(player);  // �÷��̾� �г��� �߰�
                    playerCamera.Add(_playerCamera);                                                        
                }
            }                                                     
        }

        camerainx = Random.Range(0, playerCamera.Count);

        playerCamera[camerainx].lis.enabled = true;
        playerCamera[camerainx].specificCamera.enabled = true;
        playerName.text = playerList[camerainx].pv.owner.NickName;
    }

    public void ClickRight()    // ������ ��ư Ŭ��
    {
        playerCamera[camerainx].lis.enabled = false;               // ���� �÷��̾��� ����������ʿ� ī�޶� ����
        playerCamera[camerainx].specificCamera.enabled = false;
        

        camerainx++;   // ī�޶� �ε��� ����
        if (camerainx == playerCamera.Count)   // ������ ���� ����Ʈ�� �����̸� ����Ʈ ó���ε�����
        {
            camerainx = 0;
        }
            
        if (playerList[camerainx].die == false)                     // �÷��̾ ��������� ����
        {
            playerCamera[camerainx].lis.enabled = true;             // ���� �÷��̾��� ī�޶�� ������ �ѱ�
            playerCamera[camerainx].specificCamera.enabled = true;
            playerName.text = playerList[camerainx].pv.owner.NickName;
        }
      
    }

    public void ClickLeft()  // ���� ��ư Ŭ��
    {
        playerCamera[camerainx].lis.enabled = false;                // ���� �÷��̾��� ����������ʿ� ī�޶� ����
        playerCamera[camerainx].specificCamera.enabled = false;
        

        camerainx--;            // ī�޶� �ε��� ����

        if (camerainx < 0)   // ������ ���� 0���� ������ ����Ʈ �� �ε�����
        {
            camerainx = playerCamera.Count - 1;
        }

        if (playerList[camerainx].die == false)                     // �÷��̾ ��������� ����
        {
            playerCamera[camerainx].lis.enabled = true;             // ���� �÷��̾��� ī�޶�� ������ �ѱ�
            playerCamera[camerainx].specificCamera.enabled = true;
            playerName.text = playerList[camerainx].pv.owner.NickName;
        }
      
    }

    private void Update()
    {
        // ���� ���� ���� �÷��̾ �׾����� Ȯ��
        if (playerList[camerainx].die)
        {
            ClickRight();
        }
    }
}
