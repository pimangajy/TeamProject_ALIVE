using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    public GridManager gridManager; // GridManager 레퍼런스
    public Vector2Int start; // 출발 지점
    public Vector2Int goal; // 목표 지점
    public float delay = 0.5f; // 단계별 진행 속도 (초)

    private List<Node_> OpenList; // 탐색할 노드를 저장하는 리스트
    private HashSet<Node_> ClosedList; // 탐색이 완료된 노드를 저장하는 집합

    void Start()
    {
        StartCoroutine(FindPathCoroutine(start, goal, goal)); // 단계별 경로 찾기 시작
    }

    public bool a = true;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (start == goal)
                End();
            goal += new Vector2Int(0, 1);
            if (start == goal)
                End();
            StartCoroutine(FindPathCoroutine(start, goal, goal + new Vector2Int(0, -1)));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (start == goal)
                End();
            goal += new Vector2Int(0, -1);
            if (start == goal)
                End();
            StartCoroutine(FindPathCoroutine(start, goal, goal + new Vector2Int(0, +1)));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (start == goal)
                End();
            goal += new Vector2Int(1, 0);
            if (start == goal)
                End();
            StartCoroutine(FindPathCoroutine(start, goal, goal + new Vector2Int(-1, 0)));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (start == goal)
                End();
            goal += new Vector2Int(-1, 0);
            if (start == goal)
                End();
            StartCoroutine(FindPathCoroutine(start, goal, goal + new Vector2Int(+1, 0)));
        }

    }

    private IEnumerator FindPathCoroutine(Vector2Int start, Vector2Int goal, Vector2Int g)
    {
        OpenList = new List<Node_>(); // OpenList 초기화
        ClosedList = new HashSet<Node_>(); // ClosedList 초기화

        Node_ startNode = gridManager.GetNode(start); // 출발 지점 노드
        Node_ goalNode = gridManager.GetNode(goal); // 목표 지점 노드
        Node_ goal__ = gridManager.GetNode(g);

        startNode.spriteRenderer.color = Color.black;
        goal__.spriteRenderer.color = Color.black;

        OpenList.Add(startNode); // 출발 노드를 OpenList에 추가

        while (OpenList.Count > 0 )
        {
            Node_ currentNode = GetLowestFCostNode(OpenList); // f 값이 가장 낮은 노드 선택
            if (currentNode == goalNode)
            {
                RetracePath(startNode, goalNode); // 목표에 도달한 경우 경로 추적
                yield break; // Coroutine 종료
            }

            OpenList.Remove(currentNode); // 현재 노드를 OpenList에서 제거
            ClosedList.Add(currentNode); // 현재 노드를 ClosedList에 추가

            foreach (Node_ neighbor in GetNeighbors(currentNode))  // 이웃 노드수 만큼 반복
            {
                if (!neighbor.walkable || ClosedList.Contains(neighbor))
                    continue;

                float newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !OpenList.Contains(neighbor))
                {
                    neighbor.gCost = newGCost; // gCost 업데이트
                    neighbor.hCost = GetDistance(neighbor, goalNode); // hCost 업데이트
                    neighbor.parent = currentNode; // 부모 노드 설정

                    if (!OpenList.Contains(neighbor))
                        OpenList.Add(neighbor); // OpenList에 노드 추가
                }
            }

            // 각 단계 후 상태 시각화
            VisualizeCurrentState(currentNode, goalNode);
            a = false;

            // 지정된 지연 시간 후 다음 단계로 이동
            yield return null;
        }
    }

    Node_ GetLowestFCostNode(List<Node_> nodes)
    {
        Node_ lowest = nodes[0];
        foreach (Node_ node in nodes)
        {
            if (node.fCost < lowest.fCost || (node.fCost == lowest.fCost && node.hCost < lowest.hCost))
                lowest = node;
        }
        return lowest;
    }

    List<Node_> GetNeighbors(Node_ node)
    {
        List<Node_> neighbors = new List<Node_>();  // 리스트 하나 생성
        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
        };

        foreach (Vector2Int direction in directions)  // for 4번으로 가능
        {
            Node_ neighbor = gridManager.GetNode(node.position + direction);
            if (neighbor != null)
                neighbors.Add(neighbor);  // 주변 이웃 노드 추가 가능한 노드 추가
        }

        return neighbors;  // 이웃 노드 리스트 반환
    }

    float GetDistance(Node_ nodeA, Node_ nodeB)
    {
        return Mathf.Abs(nodeA.position.x - nodeB.position.x) + Mathf.Abs(nodeA.position.y - nodeB.position.y);
    }

    void RetracePath(Node_ startNode, Node_ endNode)
    {
        List<Node_> path = new List<Node_>();
        Node_ currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        //foreach (Node_ node in path)
        //{
        //    node.spriteRenderer.color = Color.yellow; // 경로를 빨간색으로 표시
        //}
        path[0].spriteRenderer.color = Color.yellow; // 경로를 빨간색으로 표시
        start = path[0].position;
        if (start == goal)
            End();
    }

    void VisualizeCurrentState(Node_ node_, Node_ gaol)
    {

        gaol.spriteRenderer.color = Color.blue;
        //node_.spriteRenderer.color = Color.red;
    }

    public void End()
    {
        Debug.Log("End");
    }
}
