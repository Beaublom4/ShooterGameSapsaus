using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPgameObject : MonoBehaviour
{
    private Checkpoint cp;

    void Start()
    {
        cp = GameObject.FindGameObjectWithTag("Checkpoint").GetComponent<Checkpoint>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cp.lastCheckPointPos = transform.position;
        }
    }
}
