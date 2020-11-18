using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultZombie : Enemy
{
    public GameObject bigZombie;
    public int wantedEnemiesInRange;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    [HideInInspector]  public bool addedToList;
    [HideInInspector] public GameObject main;
    [HideInInspector] public bool moveTowardMain;
    public float mutateAnimTime;
    public override void Update()
    {
        base.Update();
        if(moveTowardMain == true)
        {
            transform.LookAt(main.transform);
            transform.position = Vector3.Lerp(transform.position, main.transform.position, 4 * Time.deltaTime);
            if(Vector3.Distance(main.transform.position, transform.position) <= 1)
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && !other.isTrigger)
        {
            if (other.GetComponent<DefaultZombie>())
            {
                if(other.GetComponent<DefaultZombie>().addedToList == false)
                {
                    other.GetComponent<DefaultZombie>().addedToList = true;
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
            if (other.GetComponent<DefaultZombie>())
            {
                if (other.GetComponent<DefaultZombie>().addedToList == true)
                {
                    other.GetComponent<DefaultZombie>().addedToList = false;
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
            g.GetComponent<DefaultZombie>().main = gameObject;
            g.GetComponent<DefaultZombie>().moveTowardMain = true;
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
