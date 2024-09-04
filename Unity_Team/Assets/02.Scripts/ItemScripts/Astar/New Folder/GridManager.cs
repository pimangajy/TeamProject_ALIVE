using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject nodePrefab; // Node ������
    public int width = 10; // �׸��� �ʺ�
    public int height = 10; // �׸��� ����
    private Node_[,] grid; // ��带 ������ 2D �迭

    void Start()
    {
        CreateGrid(); // ���� ���� �� �׸��带 ����
    }

    void CreateGrid()
    {
        grid = new Node_[width, height]; // �־��� �ʺ�� ���̷� �׸��带 �ʱ�ȭ
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ������ �� ��ġ�� ��� ������Ʈ ����
                GameObject nodeObject = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity);
                Node_ node = nodeObject.GetComponent<Node_>();
                node.position = new Vector2Int(x, y); // ����� ��ġ ����
                grid[x, y] = node; // �迭�� ��带 ����
                int rand = Random.Range(0, 9);
                if (rand <= 1)
                    node.walkable = false;

                if (node.walkable == false)
                    node.spriteRenderer.color = Color.white;
            }
        }
    }

    public Node_ GetNode(Vector2Int position)  // Node�� row, coll�� ���� ����
    {
        if (position.x >= 0 && position.x < width && position.y >= 0 && position.y < height)
        {
            return grid[position.x, position.y];
        }
        return null;
    }

    public Node_[,] GetGrid()
    {
        return grid;
    }
}
