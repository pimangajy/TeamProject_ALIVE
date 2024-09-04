using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Trriger : MonoBehaviour
{
    GameObject player;
    private void Update()
    {
        if(transform.parent.gameObject.GetComponent<ZombieController>().isDead == true)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.GetComponent<Player>() && other.gameObject.GetComponent<Player>().die == false)
    //    {
    //        player = other.gameObject;
    //        transform.parent.gameObject.GetComponent<ZombieController>().Addplayer(player);
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.GetComponent<Player>())
    //    {
    //        player = other.gameObject;
    //        transform.parent.gameObject.GetComponent<ZombieController>().Removeplayer(player);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Player playerComponent = other.gameObject.GetComponent<Player>();

        if (playerComponent != null && !playerComponent.die)  // null 체크 추가
        {
            player = other.gameObject;
            transform.parent.gameObject.GetComponent<ZombieController>().Addplayer(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player playerComponent = other.gameObject.GetComponent<Player>();

        if (playerComponent != null)  // null 체크 추가
        {
            player = other.gameObject;
            transform.parent.gameObject.GetComponent<ZombieController>().Removeplayer(player);
        }
    }

}
