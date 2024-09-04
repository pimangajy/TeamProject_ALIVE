using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_Player : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    MiniMap minimap;
    [SerializeField]
    GameObject my;

    [SerializeField]
    GameObject[] other_player = new GameObject[3];

    Vector3 P1_Pos, P2_Pos, P3_Pos;
    float P1_Distance, P2_Distance, P3_Distance;

    float ratio;

    private void Start()
    {
        ratio = 160.0f / 70.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && other.gameObject != my)
        {
            if (other_player[0] == null)
            {
                other_player[0] = other.gameObject;
            }
            else if (other_player[1] == null && other_player[1] != other_player[0])
            {
                other_player[1] = other.gameObject;
            }
            else if (other_player[2] == null && other_player[2] != other_player[0])
            {
                other_player[2] = other.gameObject;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        for (int i = 0; i < 3; i++)
        {
            if (other.gameObject == other_player[i])
            {
                Vector3 P1_Pos = (other.transform.position - transform.position).normalized;

                float relativeX = (other.transform.position.x - transform.position.x) * ratio;
                float relativeZ = (other.transform.position.z - transform.position.z) * ratio;

                Vector3 pos = new Vector3(relativeX, relativeZ, 0);

                minimap.Other_Player_Pos(i, pos);
            } 
        }
    }
}
