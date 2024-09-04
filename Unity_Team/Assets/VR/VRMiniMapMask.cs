using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMiniMapMask : MonoBehaviour
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



    private void Start()
    {

    }

    void Update()
    {

        // �÷��̾��� Y�� ȸ�� ����
        float playerYRot = player.rotation.eulerAngles.y;
        maskRect.localRotation = Quaternion.Euler(0, 0, playerYRot);
        playerIcon.localRotation = Quaternion.Euler(0, 0, -playerYRot + 45);

        // �̴ϸ� ������
        miniMapRect.anchoredPosition = new Vector3(-player.position.x * miniMapMoveSpeed - miniMapSizeX, -player.position.z * miniMapMoveSpeed + miniMapSizeY, 0);
    }
}
