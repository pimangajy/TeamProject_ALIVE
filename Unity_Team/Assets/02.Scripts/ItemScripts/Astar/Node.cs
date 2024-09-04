using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Node : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Astar_Controll astar;
    Create_Node create_node;

    Image image;

    public int row;
    public int heght;

    public bool Block;
    public bool Start_Pos;
    public bool End_Pos;

    public int num;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        int block = Random.Range(0, 9);
        if(block <= 1)
        {
            Block = true;
        }

        if(Block == true)
        {
            image.color = Color.black;
        }
        
        create_node = GameObject.Find("Astar_Create").GetComponent<Create_Node>();
        astar = GameObject.Find("Astar_Controll").GetComponent<Astar_Controll>();
        num = create_node.num;
    }

    private void Update()
    {
        if(Start_Pos == true)
        {
            image.color = Color.blue;
        }
        if(End_Pos == true)
        {
            image.color = Color.green;
        }
    }

    IEnumerator Search_Node()
    {
        

        


        yield return null;
    }
    

    // 마우스 포인터가 오브젝트에 들어올때
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(Block == false)
        {
            image.color = Color.magenta;
        }
    }

    // 마우스 포인터가 오브젝트를 나갈때
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Block == false)
        {
            image.color = Color.white; 
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Block != true && Start_Pos != true)
        {
            astar.Node_Click(gameObject.GetComponent<Node>());
        }
    }
}
