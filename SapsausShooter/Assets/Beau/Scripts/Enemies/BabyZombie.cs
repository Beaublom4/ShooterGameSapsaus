using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyZombie : Enemy
{
    public bool isTrown;
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if(collision.gameObject.tag == "Floor")
        {
            if(isTrown == true)
            {
                isTrown = false;
                GetComponent<Enemy>().isAttacking = true;
                GetComponent<NavMeshAgent>().enabled = enabled;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
