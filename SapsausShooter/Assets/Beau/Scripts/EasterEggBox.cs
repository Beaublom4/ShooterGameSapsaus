using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggBox : MonoBehaviour
{
    public GameObject bigBaby;
    public GameObject babyDeathParticle, bigBabyparticle;
    public GameObject currentParticle;
    public Transform chestToGoToLoc, bigBabySpawnPoint;
    public float chestMoveSpeed, orbMoveSpeed;

    public Gun weapon;
    public float orbSpeed;
    public int babiesToBeEaten;
    public int babysEaten;

    bool moveChestDown;
    bool moveOrb;
    private void Update()
    {
        if(moveChestDown == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, chestToGoToLoc.position, chestMoveSpeed * Time.deltaTime);
            if (transform.position == chestToGoToLoc.position)
            {

            }
        }
        if(moveOrb == true)
        {
            currentParticle.transform.position = Vector3.MoveTowards(currentParticle.transform.position, bigBabySpawnPoint.position, orbMoveSpeed * Time.deltaTime);
            if(currentParticle.transform.position == bigBabySpawnPoint.position)
            {
                moveOrb = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == true)
            return;
        if(other.gameObject.tag == "FreezeCol")
        {
            if(babysEaten < babiesToBeEaten)
            if (other.GetComponentInParent<BabyZombie>())
            {
                GameObject orbie = Instantiate(babyDeathParticle, other.transform.position, other.transform.rotation, transform);
                orbie.GetComponent<BabyDeadOrb>().location = gameObject;
                other.GetComponentInParent<Enemy>().DoDamage(weapon, 2, other.transform.position);
            }
        }
    }
    public void AddBaby()
    {
        babysEaten++;
        if(babysEaten >= babiesToBeEaten)
        {
            GameObject g = Instantiate(bigBabyparticle, transform.position, transform.rotation, null);
            currentParticle = g;
            moveChestDown = true;
            moveOrb = true;
        }
    }
}
