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
    private bool isTriggerPressed = false;  // 트리거가 눌렸는지 추적

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.selectExited.AddListener(OnSelectExited);
    }

    void Update()
    {
        // 총이 잡혀있고 트리거 버튼이 눌렸을 때
        if (isHeld && controller.activateAction.action.ReadValue<float>() > 0.1f)
        {
            if (!isTriggerPressed)
            {
                Shoot();
                isTriggerPressed = true;  // 트리거가 눌렸음을 기록
            }
        }
        else
        {
            isTriggerPressed = false;  // 트리거가 눌리지 않았을 때 상태 리셋
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
