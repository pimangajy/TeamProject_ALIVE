using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int playerID;

    private void Start()
    {
        Invoke("DestroyBullet", 1.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        ZombieController zombie = collision.gameObject.GetComponent<ZombieController>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage, gameObject.transform.root.GetComponent<Player>().pv.viewID);
        }

        if (collision.gameObject.tag != "Item")
        {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ZombieController zombie = other.gameObject.GetComponent<ZombieController>();
        if (zombie != null)
        {
            zombie.TakeDamage(damage, playerID);
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }
}
