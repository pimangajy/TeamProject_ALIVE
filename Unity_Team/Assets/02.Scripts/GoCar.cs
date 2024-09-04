using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoCar : MonoBehaviour
{
    public bool exit = false;
    public GameObject sedan;
    public GameObject camera;
    private float speed;

    private void Awake()
    {
        speed = 10 * Time.deltaTime;
    }


    private void Update()
    {
        if (exit)
        {
            camera.SetActive(true);
            sedan.transform.localPosition += Vector3.forward * speed;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ExitState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ExitState(false);
        }
    }
}


