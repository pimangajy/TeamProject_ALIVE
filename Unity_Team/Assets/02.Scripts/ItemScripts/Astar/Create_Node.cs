using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Node : MonoBehaviour
{
    public Astar_Controll astar_controll;

    public GameObject Node;
    public GameObject Parent;

    public GameObject[][] nodes;

    public int num;
    void Start()
    {
        astar_controll.nodes = new GameObject[num][];

        for (int i =0; i < num; i++)
        {
            astar_controll.nodes[i] = new GameObject[num];

            for (int j = 0; j < num; j++)
            {
                GameObject node = Instantiate(Node);
                node.transform.parent = Parent.transform;
                node.GetComponent<Node>().row = i;
                node.GetComponent<Node>().heght = j;
                astar_controll.nodes[i][j] = node;
            }
        }

        int startpos = num / 2;
        astar_controll.nodes[startpos][0].GetComponent<Node>().Start_Pos = true;
        astar_controll.nodes[startpos][num - 1].GetComponent<Node>().End_Pos = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
