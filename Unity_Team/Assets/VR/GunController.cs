using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunController : MonoBehaviour
{
    public ActionBasedController controller;
    public GameObject bulletPrefab;
    public Transform barrelEnd;
    public float bulletSpeed = 20f;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;
    private bool isTriggerPressed = false;  // Ʈ���Ű� ���ȴ��� ����

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void Update()
    {
        // ���� �����ְ� Ʈ���� ��ư�� ������ ��
        if (isHeld && controller.activateAction.action.ReadValue<float>() > 0.1f)
        {
            if (!isTriggerPressed)
            {
                Shoot();
                isTriggerPressed = true;  // Ʈ���Ű� �������� ���
            }
        }
        else
        {
            isTriggerPressed = false;  // Ʈ���Ű� ������ �ʾ��� �� ���� ����
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = barrelEnd.forward * bulletSpeed;
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        isHeld = true;
    }

    private void OnSelectExited(SelectExitEventArgs args)
    {
        isHeld = false;
    }
}
