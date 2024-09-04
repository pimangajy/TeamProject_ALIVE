using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGizmo : MonoBehaviour
{
    [SerializeField]
    float radius;

    [SerializeField]
    Color color;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        Gizmos.DrawSphere(transform.position, radius);
    }
}
