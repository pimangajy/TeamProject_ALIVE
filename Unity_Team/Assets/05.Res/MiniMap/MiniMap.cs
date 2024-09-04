using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField]
    Transform player;   // ������ �÷��̾��� Ʈ������
    public RectTransform maskRect;   // ����ũ Ʈ������
    public RectTransform miniMapRect; // �̴ϸ� Ʈ������
    public RectTransform playerIcon; // �÷��̾� ������ Ʈ������
    public PhotonView pv;

    [Header("������ �̴ϸ� ����")]
    public float miniMapMoveSpeed;  // �÷��̾� �ӵ��� �̴ϸ� �����̴� �ӵ� ����
    public float miniMapSizeX;  // ������ �̴ϸ�  X
    public float miniMapSizeY;  // ������ �̴ϸ�  Y 

    [Header("�ٸ� �÷��̾�")]
    public RectTransform P1;
    public RectTransform P2;
    public RectTransform P3;


    private void Start()
    {
        
    }

    void Update()
    {
        // �÷��̾��� Y�� ȸ�� ����
        float playerYRot = player.rotation.eulerAngles.y;
        maskRect.rotation = Quaternion.Euler(0, 0, playerYRot);
        playerIcon.rotation = Quaternion.Euler(0, 0, 0);

        // �̴ϸ� ������
        miniMapRect.anchoredPosition = new Vector3(-player.position.x * miniMapMoveSpeed - miniMapSizeX, -player.position.z * miniMapMoveSpeed + miniMapSizeY, 0);
    }

    public void Other_Player_Pos(int p, Vector3 pos)
    {
        if (p == 1)
        {
            P1.anchoredPosition = pos;  
        }
        else if(p == 2)
        {
            P2.anchoredPosition = pos;
        }
        else if (p == 2)
        {
            P3.anchoredPosition = pos;
        }
    }

}

