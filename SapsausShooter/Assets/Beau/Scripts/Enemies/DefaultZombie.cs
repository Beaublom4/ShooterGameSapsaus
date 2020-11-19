using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultZombie : Enemy
{
    public GameObject bigZombie;
    public int wantedEnemiesInRange;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public float mutateAnimTime;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !other.isTrigger)
        {
            if (other.GetComponent<Enemy>().countTowardsBigZomb == true)
            {
                if (other.GetComponent<Enemy>().addedToList == false)
                {
                    other.GetComponent<Enemy>().addedToList = true;
                    enemiesInRange.Add(other.gameObject);
                    CheckIfListFull();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !other.isTrigger)
        {
            if (other.GetComponent<Enemy>().countTowardsBigZomb == true)
            {
                if (other.GetComponent<Enemy>().addedToList == true)
                {
                    other.GetComponent<Enemy>().addedToList = false;
                    enemiesInRange.Remove(other.gameObject);
                }
            }
        }
    }
    void CheckIfListFull()
    {
        if(enemiesInRange.Count >= wantedEnemiesInRange)
        {
            UpdateList();
        }
    }
    void UpdateList()
    { 
        foreach(GameObject g in enemiesInRange)
        {
            g.GetComponent<NavMeshAgent>().speed = 0;
            g.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            g.GetComponent<Enemy>().main = gameObject;
            g.GetComponent<Enemy>().moveTowardMain = true;
        }
        StartCoroutine(Mutate());
    }
    IEnumerator Mutate()
    {
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        print("Mutate");
        //mutate anim
        yield return new WaitForSeconds(mutateAnimTime);
        Instantiate(bigZombie, transform.position, transform.rotation, null);
        Destroy(this.gameObject);
    }
}
