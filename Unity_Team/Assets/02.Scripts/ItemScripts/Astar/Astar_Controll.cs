using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar_Controll : MonoBehaviour
{
    public GameObject[][] nodes;
    public Create_Node create_node;

    List<Node> openList;

    int num;
    private void Start()
    {
        num = create_node.num;
    }

    public void Node_Click(Node node)
    {
        

        if (CheckNode(node.row + 1, node.heght))
        {

        }
        if (CheckNode(node.row - 1, node.heght))
        {

        }
        if (CheckNode(node.row, node.heght + 1))
        {

        }
        if (CheckNode(node.row, node.heght - 1))
        {

        }
    }
    private bool CheckNode(int row, int col)
    {
        if (row < 0 || row >= num)
            return false;

        if (col < 0 || col >= num)
            return false;

        return true;
    }

    public void Neighbours()
    {

    }
}
