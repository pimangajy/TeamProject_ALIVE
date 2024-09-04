using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    public GridManager gridManager; // GridManager ���۷���
    public Vector2Int start; // ��� ����
    public Vector2Int goal; // ��ǥ ����
    public float delay = 0.5f; // �ܰ躰 ���� �ӵ� (��)

    private List<Node_> OpenList; // Ž���� ��带 �����ϴ� ����Ʈ
    private HashSet<Node_> ClosedList; // Ž���� �Ϸ�� ��带 �����ϴ� ����

    void Start()
    {
        StartCoroutine(FindPathCoroutine(start, goal, goal)); // �ܰ躰 ��� ã�� ����
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
        OpenList = new List<Node_>(); // OpenList �ʱ�ȭ
        ClosedList = new HashSet<Node_>(); // ClosedList �ʱ�ȭ

        Node_ startNode = gridManager.GetNode(start); // ��� ���� ���
        Node_ goalNode = gridManager.GetNode(goal); // ��ǥ ���� ���
        Node_ goal__ = gridManager.GetNode(g);

        startNode.spriteRenderer.color = Color.black;
        goal__.spriteRenderer.color = Color.black;

        OpenList.Add(startNode); // ��� ��带 OpenList�� �߰�

        while (OpenList.Count > 0 )
        {
            Node_ currentNode = GetLowestFCostNode(OpenList); // f ���� ���� ���� ��� ����
            if (currentNode == goalNode)
            {
                RetracePath(startNode, goalNode); // ��ǥ�� ������ ��� ��� ����
                yield break; // Coroutine ����
            }

            OpenList.Remove(currentNode); // ���� ��带 OpenList���� ����
            ClosedList.Add(currentNode); // ���� ��带 ClosedList�� �߰�

            foreach (Node_ neighbor in GetNeighbors(currentNode))  // �̿� ���� ��ŭ �ݺ�
            {
                if (!neighbor.walkable || ClosedList.Contains(neighbor))
                    continue;

                float newGCost = currentNode.gCost + GetDistance(currentNode, neighbor);
                if (newGCost < neighbor.gCost || !OpenList.Contains(neighbor))
                {
                    neighbor.gCost = newGCost; // gCost ������Ʈ
                    neighbor.hCost = GetDistance(neighbor, goalNode); // hCost ������Ʈ
                    neighbor.parent = currentNode; // �θ� ��� ����

                    if (!OpenList.Contains(neighbor))
                        OpenList.Add(neighbor); // OpenList�� ��� �߰�
                }
            }

            // �� �ܰ� �� ���� �ð�ȭ
            VisualizeCurrentState(currentNode, goalNode);
            a = false;

            // ������ ���� �ð� �� ���� �ܰ�� �̵�
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
        List<Node_> neighbors = new List<Node_>();  // ����Ʈ �ϳ� ����
        Vector2Int[] directions = {
            new Vector2Int(1, 0), new Vector2Int(-1, 0),
            new Vector2Int(0, 1), new Vector2Int(0, -1)
        };

        foreach (Vector2Int direction in directions)  // for 4������ ����
        {
            Node_ neighbor = gridManager.GetNode(node.position + direction);
            if (neighbor != null)
                neighbors.Add(neighbor);  // �ֺ� �̿� ��� �߰� ������ ��� �߰�
        }

        return neighbors;  // �̿� ��� ����Ʈ ��ȯ
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
        //    node.spriteRenderer.color = Color.yellow; // ��θ� ���������� ǥ��
        //}
        path[0].spriteRenderer.color = Color.yellow; // ��θ� ���������� ǥ��
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
