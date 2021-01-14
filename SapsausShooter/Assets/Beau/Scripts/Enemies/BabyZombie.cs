using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BabyZombie : Enemy
{
    public bool isTrown = true, isSpitting;
    public Transform spitPoint;
    public GameObject spitPrefab;
    public Vector3 spitDirection;
    public Collider col;
    public GameObject freezeCol;
    public bool heldByMother = false;

    public float timer, spitDelay;
    public float spitVolume, spitChange;
    public AudioSource spit;

    public override void Start()
    {
        base.Start();
        timer = spitDelay;
    }
    public override void Trigger(GameObject player)
    {
        base.Trigger(player);
        isTrown = false;
        heldByMother = true;
    }
    public override void Update()
    {
        base.Update();
        if(isSpitting == true)
        {
            if (isDeath == true) return;
            transform.LookAt(playerObj.transform);
            if (timer >= 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer < 0)
            {
                timer = spitDelay * 1 + (8 * freezeNum);

                anim.SetTrigger("Attack");
                spit.volume = Random.Range(spitVolume - spitChange, spitVolume + spitChange);
                spit.pitch = Random.Range(1 - spitChange, 1 + spitChange);
                spit.Play();
                GameObject g = Instantiate(spitPrefab, spitPoint) as GameObject;
                g.GetComponent<SpitBall>().damage = damage;
                g.transform.SetParent(null);
                g.GetComponent<Rigidbody>().AddRelativeForce(spitDirection);
            }
        }
    }

    public override void OnCollisionEnter(Collision collision)
    {
        if (heldByMother == true)
        {
            if (collision.gameObject.tag == "Floor")
            {
                if (isTrown == false)
                {
                    GetComponent<NavMeshAgent>().enabled = enabled;
                    col.enabled = enabled;
                    isTrown = true;
                    isAttacking = true;
                    anim.SetBool("Walking", true);
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    freezeCol.tag = "FreezeCol";
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDeath == true) return;
        if(isTrown == true && other.gameObject.tag == "Player")
        {
            isAttacking = false;
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            col.enabled = enabled;
            agent.isStopped = true;
            anim.SetBool("Walking", false);
            isSpitting = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isDeath == true) return;
        if (isTrown == true && other.gameObject.tag == "Player")
        {
            agent.speed = speed;
            agent.isStopped = false;
            isAttacking = true;
            anim.SetBool("Walking", true);
            isSpitting = false;
        }
    }
}
