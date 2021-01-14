using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasterEggBox : MonoBehaviour
{
    public GameObject bigBaby, orbding;
    public Transform spawnLoc;
    public GameObject babyDeathParticle;
    public Transform chestToGoToLoc;
    public float chestMoveSpeed;

    public Gun weapon;
    public int babiesToBeEaten;
    public int babysEaten;

    bool moveChestDown;
    private void Update()
    {
        //if(moveChestDown == true)
        //{
        //    transform.position = Vector3.MoveTowards(transform.position, chestToGoToLoc.position, chestMoveSpeed * Time.deltaTime);
        //    if (transform.position == chestToGoToLoc.position)
        //    {

        //    }
        //}
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
            GameObject g = Instantiate(bigBaby, spawnLoc.position, spawnLoc.rotation, null);
            orbding.SetActive(true);
            orbding.GetComponent<Animator>().SetTrigger("Orb");
            Destroy(orbding, 5.4f);
            moveChestDown = true;
        }
    }
}
