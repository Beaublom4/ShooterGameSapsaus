using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BashEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (GetComponentInParent<CrushedZombie>().canBash == false)
            {
                GetComponentInParent<CrushedZombie>().StartBash();
            }
        }
    }
}
