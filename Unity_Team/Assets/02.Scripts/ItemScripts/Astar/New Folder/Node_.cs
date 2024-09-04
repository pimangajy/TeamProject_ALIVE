using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_ : MonoBehaviour
{
    public Vector2Int position; // 격자상의 위치
    public bool walkable = true; // 이동 가능한지 여부
    public float gCost; // 출발점에서 이 노드까지의 비용
    public float hCost; // 이 노드에서 목표까지의 예상 비용
    public Node_ parent; // 경로를 추적하기 위한 부모 노드

    public float fCost => gCost + hCost; // 총 비용

    public SpriteRenderer spriteRenderer; // 노드의 시각적 요소
}
