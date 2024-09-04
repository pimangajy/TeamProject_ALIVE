using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMiniMap : MonoBehaviour
{
    public Camera playerCamera; // �÷��̾� ī�޶� �̰��� �Ҵ��ϼ���.

    void Update()
    {
        if (playerCamera != null)
        {
            // ĵ������ ī�޶� �������� ȸ��
            transform.LookAt(playerCamera.transform);

            // ī�޶�� ������ �������� ȸ���ϱ� ���� up ���͸� ����
            // LookAt�� ��ü�� �� ������ ī�޶��� up ����� �������� ���� �� �����Ƿ�
            transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.transform.position, playerCamera.transform.up);
        }
    }
}
