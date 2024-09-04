using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
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

    [Header("다른 플레이어")]
    public RectTransform P1;
    public RectTransform P2;
    public RectTransform P3;


    private void Start()
    {
        
    }

    void Update()
    {
        // 플레이어의 Y축 회전 각도
        float playerYRot = player.rotation.eulerAngles.y;
        maskRect.rotation = Quaternion.Euler(0, 0, playerYRot);
        playerIcon.rotation = Quaternion.Euler(0, 0, 0);

        // 미니맵 움직임
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

