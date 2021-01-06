using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MomZombie : Enemy
{
    public GameObject babyZombiePrefab, carriedBaby;
    GameObject currentBaby;
    public Transform trowPos, spawnArea;
    public Vector3 trowVelocity;
    public override void Trigger(GameObject player)
    {
        base.Trigger(player);
        Invoke("TrowBaby", anim.GetCurrentAnimatorStateInfo(0).length);
    }
    void TrowBaby()
    {
        if (isDeath == false)
        {
            Destroy(carriedBaby);
            Instantiate(babyZombiePrefab, trowPos.position, transform.rotation, trowPos);
            currentBaby = trowPos.GetChild(0).gameObject;
            currentBaby.transform.parent = spawnArea;
            currentBaby.GetComponent<Enemy>().playerObj = playerObj;

            currentBaby.GetComponent<Animator>().SetTrigger("Aggro");
            currentBaby.GetComponent<NavMeshAgent>().enabled = !enabled;
            currentBaby.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            currentBaby.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            currentBaby.GetComponent<Rigidbody>().AddRelativeForce(trowVelocity);
        }
    }
    public override IEnumerator Dead(int hitPoint)
    {
        isDeath = true;
        if(carriedBaby != null)
        {
            GameObject g = Instantiate(babyZombiePrefab, trowPos.position, transform.rotation, trowPos);
            g.transform.parent = spawnArea;
        }
        return base.Dead(hitPoint);
    }
}
