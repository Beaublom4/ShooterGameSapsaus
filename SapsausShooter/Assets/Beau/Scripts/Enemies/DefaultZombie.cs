using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DefaultZombie : Enemy
{
    public GameObject bigZombie;
    public int wantedEnemiesInRange;
    public List<GameObject> enemiesInRange = new List<GameObject>();
    public Transform bigBoySpawnPoint;
    public GameObject bigBoyParticles;
    public bool MainAtLoc;
    bool hasRunned;

    public float mergeVolume, mergeChange;
    public AudioSource merge;
    public LayerMask hittableLayer;
    public override void Update()
    {
        base.Update();
        if (MainAtLoc == true && hasRunned == false)
        {
            hasRunned = true;
            merge.volume = Random.Range(mergeVolume - mergeChange, mergeVolume + mergeChange);
            merge.pitch = Random.Range(1 - mergeChange, 1 + mergeChange);
            merge.Play();
            Instantiate(bigZombie, transform.position, transform.rotation, bigBoySpawnPoint);
            GameObject particles = Instantiate(bigBoyParticles, transform.position, transform.rotation, null);
            Destroy(particles, 5);
            GameObject bigBoy = bigBoySpawnPoint.GetChild(0).gameObject;
            bigBoy.GetComponent<Enemy>().Trigger(playerObj);
            bigBoy.transform.SetParent(null);
            foreach (GameObject g in enemiesInRange)
            {
                Destroy(g);
            }
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isBigZombie == true)
        {
            return;
        }
        if (enemiesInRange.Count >= wantedEnemiesInRange)
        {
            return;
        }

        if(other.gameObject.GetComponent<Enemy>())
        {
            if(other.gameObject.GetComponent<Enemy>().canMutateToBigZomb == false)
            return;
        }
        if (other.gameObject.tag == "Enemy" && !other.isTrigger)
        {
            if (other.GetComponentInParent<Enemy>().countTowardsBigZomb == true)
            {
                if (other.GetComponentInParent<Enemy>().addedToList == false)
                {
                    other.GetComponentInParent<Enemy>().addedToList = true;
                    enemiesInRange.Add(other.GetComponentInParent<Enemy>().gameObject);
                    CheckIfListFull();
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isBigZombie == true)
        {
            return;
        }
        if (other.gameObject.tag == "Enemy" && !other.isTrigger)
        {
            if (other.GetComponentInParent<Enemy>().countTowardsBigZomb == true)
            {
                if (other.GetComponentInParent<Enemy>().addedToList == true)
                {
                    other.GetComponentInParent<Enemy>().addedToList = false;
                    enemiesInRange.Remove(other.GetComponentInParent<Enemy>().gameObject);
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
    float x;
    float y;
    float z;
    void UpdateList()
    {
        foreach (GameObject g in enemiesInRange)
        {
            if (g == null)
                return;
            x += g.transform.position.x;
            y += g.transform.position.y;
            z += g.transform.position.z;
        }
        x /= enemiesInRange.Count;
        y /= enemiesInRange.Count;
        z /= enemiesInRange.Count;

        Vector3 midPos = new Vector3(x, y, z);
        main = midPos;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, midPos, out hit, 1000, hittableLayer, QueryTriggerInteraction.Ignore))
        {
            float dist = Vector3.Distance(transform.position, midPos);
            midPos = hit.point;
        }

        foreach (GameObject g in enemiesInRange)
        {
            g.GetComponent<NavMeshAgent>().speed = 0;
            g.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
            g.GetComponent<Enemy>().main = midPos;
            g.GetComponent<Enemy>().moveTowardMain = true;
            g.GetComponent<Animator>().SetTrigger("Merge");
        }
        GetComponent<NavMeshAgent>().speed = 0;
        GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        print("Mutate");
        anim.SetTrigger("Merge");
        isMainBody = true;
        moveTowardMain = true;
    }
}
