﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MomZombie : Enemy
{
    public GameObject babyZombiePrefab, carriedBaby;
    GameObject currentBaby;
    public Transform trowPos, spawnArea;
    public Vector3 trowVelocity;
    IEnumerator coroutine;
    public override void Trigger(GameObject player)
    {
        base.Trigger(player);
        if (gameObject.activeSelf == true)
        {
            coroutine = TrowBaby();
            StartCoroutine(coroutine);
        }
    }
    IEnumerator TrowBaby()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        if (isDeath == false)
        {
            Destroy(carriedBaby);
            Instantiate(babyZombiePrefab, trowPos.position, transform.rotation, trowPos);
            carriedBaby = null;
            currentBaby = trowPos.GetChild(0).gameObject;
            currentBaby.GetComponent<BabyZombie>().isTrown = false;
            currentBaby.GetComponent<BabyZombie>().heldByMother = true;
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
        if (carriedBaby != null)
        {
            print("dikke kut");
            StopCoroutine(coroutine);
            Destroy(currentBaby);
            GameObject g = Instantiate(babyZombiePrefab, trowPos.position, transform.rotation, trowPos);
            g.GetComponent<Enemy>().Trigger(playerObj);
            g.transform.parent = spawnArea;
        }
        return base.Dead(hitPoint);
    }
}
