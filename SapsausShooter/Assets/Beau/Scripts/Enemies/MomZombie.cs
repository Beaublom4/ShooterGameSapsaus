using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MomZombie : Enemy
{
    public GameObject babyZombie, carriedBaby;
    public GameObject currentBaby;
    public Transform trowPos, spawnArea;
    public Vector3 trowVelocity;
    public override void Trigger(GameObject player)
    {
        base.Trigger(player);
        Invoke("TrowBaby", 3);
    }
    void TrowBaby()
    {
        Destroy(carriedBaby);
        Instantiate(babyZombie, trowPos.position, transform.rotation, trowPos);
        currentBaby = trowPos.GetChild(0).gameObject;
        currentBaby.transform.parent = spawnArea;
        currentBaby.GetComponent<Enemy>().playerObj = playerObj;

        currentBaby.GetComponent<BabyZombie>().isTrown = true;
        currentBaby.GetComponent<Enemy>().isAttacking = false;
        currentBaby.GetComponent<NavMeshAgent>().enabled = !enabled;
        currentBaby.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        currentBaby.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        currentBaby.GetComponent<Rigidbody>().AddRelativeForce(trowVelocity);
    }
}
