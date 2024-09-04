using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMiniMapMask : MonoBehaviour
{
    [SerializeField]
    Transform player;   // 참조할 플레이어의 트랜스폼
    public RectTransform maskRect;   // 마스크 트랜스폼
    public RectTransform miniMapRect; // 미니맵 트랜스폼
    public RectTransform playerIcon; // 플레이어 아이콘 트랜스폼
    public PhotonView pv;

    [Header("보정할 미니맵 변수")]
    public float miniMapMoveSpeed;  // 플레이어 속도와 미니맵 움직이는 속도 보정
    public float miniMapSizeX;  // 보정할 미니맵  X
    public float miniMapSizeY;  // 보정할 미니맵  Y 



    private void Start()
    {

    }

    void Update()
    {

        // 플레이어의 Y축 회전 각도
        float playerYRot = player.rotation.eulerAngles.y;
        maskRect.localRotation = Quaternion.Euler(0, 0, playerYRot);
        playerIcon.localRotation = Quaternion.Euler(0, 0, -playerYRot + 45);

        // 미니맵 움직임
        miniMapRect.anchoredPosition = new Vector3(-player.position.x * miniMapMoveSpeed - miniMapSizeX, -player.position.z * miniMapMoveSpeed + miniMapSizeY, 0);
    }
}
