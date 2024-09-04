using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMiniMap : MonoBehaviour
{
    public Camera playerCamera; // 플레이어 카메라를 이곳에 할당하세요.

    void Update()
    {
        if (playerCamera != null)
        {
            // 캔버스를 카메라 방향으로 회전
            transform.LookAt(playerCamera.transform);

            // 카메라와 동일한 방향으로 회전하기 위해 up 벡터를 조정
            // LookAt은 객체의 위 방향이 카메라의 up 방향과 동일하지 않을 수 있으므로
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position, playerCamera.transform.up);
        }
    }
}
