using UnityEngine;

public class ShadowManager : MonoBehaviour
{
    // Ư�� ������Ʈ�� �׸��ڸ� ��Ȱ��ȭ�ϴ� �Լ�
    public void DisableShadows(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    // ����: �� ���� ��� ������Ʈ�� �׸��� ��Ȱ��ȭ
    void Start()
    {
        // �� ���� ��� Renderer ������Ʈ�� ������
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }
}
