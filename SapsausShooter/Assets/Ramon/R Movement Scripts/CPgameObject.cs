using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPgameObject : MonoBehaviour
{
    Checkpoint checkpontManager;

    void Start()
    {
        checkpontManager = GetComponentInParent<Checkpoint>();
        print(checkpontManager);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            checkpontManager.lastCheckPointPos = transform;
            print("checkpoint touched");
        }
    }
}
