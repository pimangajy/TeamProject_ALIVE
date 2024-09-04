using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseBoundary : MonoBehaviour
{
    private Rect screenBounds;

    void Start()
    {
        // 초기 화면 경계 설정
        screenBounds = new Rect(0, 0, Screen.width, Screen.height);

        // 마우스 커서를 보이도록 설정
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        // 마우스 위치를 화면 경계 내로 제한
        if (mousePosition.x < screenBounds.xMin)
            mousePosition.x = screenBounds.xMin;
        if (mousePosition.x > screenBounds.xMax)
            mousePosition.x = screenBounds.xMax;
        if (mousePosition.y < screenBounds.yMin)
            mousePosition.y = screenBounds.yMin;
        if (mousePosition.y > screenBounds.yMax)
            mousePosition.y = screenBounds.yMax;

        // 마우스 위치를 업데이트
        Cursor.SetCursor(null, mousePosition, CursorMode.Auto);
    }
}
