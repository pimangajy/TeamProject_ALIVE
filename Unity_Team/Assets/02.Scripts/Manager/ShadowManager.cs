using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    // 특정 오브젝트의 그림자를 비활성화하는 함수
    public void DisableShadows(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    // 예제: 씬 내의 모든 오브젝트의 그림자 비활성화
    void Start()
    {
        // 씬 내의 모든 Renderer 컴포넌트를 가져옴
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}
