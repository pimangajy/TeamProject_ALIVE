using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node_ : MonoBehaviour
{
    public Vector2Int position; // ���ڻ��� ��ġ
    public bool walkable = true; // �̵� �������� ����
    public float gCost; // ��������� �� �������� ���
    public float hCost; // �� ��忡�� ��ǥ������ ���� ���
    public Node_ parent; // ��θ� �����ϱ� ���� �θ� ���

    public float fCost => gCost + hCost; // �� ���

    public SpriteRenderer spriteRenderer; // ����� �ð��� ���
}
