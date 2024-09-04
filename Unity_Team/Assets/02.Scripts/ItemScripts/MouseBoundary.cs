using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBoundary : MonoBehaviour
{
    private Rect screenBounds;

    void Start()
    {
        // �ʱ� ȭ�� ��� ����
        screenBounds = new Rect(0, 0, Screen.width, Screen.height);

        // ���콺 Ŀ���� ���̵��� ����
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // ���콺 ��ġ�� ȭ�� ��� ���� ����
        if (mousePosition.x < screenBounds.xMin)
            mousePosition.x = screenBounds.xMin;
        if (mousePosition.x > screenBounds.xMax)
            mousePosition.x = screenBounds.xMax;
        if (mousePosition.y < screenBounds.yMin)
            mousePosition.y = screenBounds.yMin;
        if (mousePosition.y > screenBounds.yMax)
            mousePosition.y = screenBounds.yMax;

        // ���콺 ��ġ�� ������Ʈ
        Cursor.SetCursor(null, mousePosition, CursorMode.Auto);
    }
}
