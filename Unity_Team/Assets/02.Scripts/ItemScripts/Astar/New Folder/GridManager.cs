using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject nodePrefab; // Node 프리팹
    public int width = 10; // 그리드 너비
    public int height = 10; // 그리드 높이
    private Node_[,] grid; // 노드를 저장할 2D 배열

    void Start()
    {
        CreateGrid(); // 게임 시작 시 그리드를 생성
    }

    void CreateGrid()
    {
        grid = new Node_[width, height]; // 주어진 너비와 높이로 그리드를 초기화
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // 격자의 각 위치에 노드 오브젝트 생성
                GameObject nodeObject = Instantiate(nodePrefab, new Vector3(x, y, 0), Quaternion.identity);
                Node_ node = nodeObject.GetComponent<Node_>();
                node.position = new Vector2Int(x, y); // 노드의 위치 설정
                grid[x, y] = node; // 배열에 노드를 저장
                int rand = Random.Range(0, 9);
                if (rand <= 1)
                    node.walkable = false;

                if (node.walkable == false)
                    node.spriteRenderer.color = Color.white;
            }
        }
    }

    public Node_ GetNode(Vector2Int position)  // Node의 row, coll로 변경 가능
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
