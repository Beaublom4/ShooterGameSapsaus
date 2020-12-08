using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioActiveZombie : Enemy
{
    public bool playerInRange;
    public float timer, addTimeToTimer;
    public override void Update()
    {
        base.Update();
        if(playerInRange == true)
        {
            if(timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if(timer <= 0)
            {
                DoToxicDamage();
                timer = addTimeToTimer;
            }
        }
    }
    void DoToxicDamage()
    {
        playerObj.GetComponent<HealthManager>().DoDamage(damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = true;   
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
