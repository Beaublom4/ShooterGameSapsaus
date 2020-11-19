using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossZombie : MonoBehaviour
{
    public float speed, timeBetweenAttacks;
    public GameObject playerObj, dog;
    public float maxRangeMeleeAttack;
    public Transform meleePlace;
    public Melee[] weaponList;
    public Melee selectedWeapon;
    public GameObject nuclearBarrel;
    public GameObject currentBarrel;
    public Transform barrelSpawnPoint;

    bool isMeleeAttacking;
    NavMeshAgent agent;
    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;

        RandomAttack();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Trigger();
        }
        if (isMeleeAttacking == true)
        {
            agent.destination = playerObj.transform.position;
        }
    }
    public void Trigger()
    {
        RandomAttack();
    }
    public void RandomAttack()
    {
        if(Vector3.Distance(transform.position, playerObj.transform.position) <= maxRangeMeleeAttack)
        {
            int randomNum = Random.Range(3, 4);
            switch (randomNum)
            {
                case 0:
                    MeleeAttack();
                    break;
                case 1:
                    DogAttack();
                    break;
                case 2:
                    ShockWaveAttack();
                    break;
                case 3:
                    ToxicBoyTrow();
                    break;
            }
        }
        else
        {
            int randomNum = Random.Range(2, 3);
            switch (randomNum)
            {
                case 0:
                    DogAttack();
                    break;
                case 1:
                    ShockWaveAttack();
                    break;
                case 2:
                    ToxicBoyTrow();
                    break;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isMeleeAttacking == true)
        {
            if(other.gameObject.tag == "Player")
            {
                isMeleeAttacking = false;
                StartCoroutine(DoMeleeAttack());
            }
        }
    }
    void MeleeAttack()
    {
        print("Melee Attack");
        isMeleeAttacking = true;
        int randomWeapon = Random.Range(0, weaponList.Length);
        selectedWeapon = weaponList[randomWeapon];
        //instantiate weapon in hand / hold weapon anim ... walk anim with weapon
    }
    IEnumerator DoMeleeAttack()
    {
        //melee anim
        isMeleeAttacking = false;
        agent.speed = 0;
        Instantiate(selectedWeapon.hitBoxForEnemy, meleePlace.position, transform.rotation, meleePlace);
        meleePlace.GetChild(0).transform.GetComponent<EnemyHitBoxMelee>().boss = gameObject;
        yield return new WaitForSeconds(1);
        Destroy(meleePlace.GetChild(0).gameObject);
        //cooldown anim
        yield return new WaitForSeconds(timeBetweenAttacks);
        agent.speed = speed;
        RandomAttack();
    }
    void DogAttack()
    {
        print("Dog");
        dog.GetComponent<DogZombie>().Trigger(playerObj);
    }
    void ShockWaveAttack()
    {
        print("Shock wave attack");
        RandomAttack();
    } 
    void ToxicBoyTrow()
    {
        print("Toxic boy trow");
        StartCoroutine(DoToxicBarrelTrow());
        RandomAttack();
    }
    IEnumerator DoToxicBarrelTrow()
    {
        Instantiate(nuclearBarrel, barrelSpawnPoint.transform.position, Quaternion.identity, barrelSpawnPoint);
        currentBarrel = barrelSpawnPoint.GetChild(0).gameObject;
        yield return new WaitForSeconds(2);
        currentBarrel.GetComponent<Rigidbody>().useGravity = true;
        float calculatedForce = Vector3.Distance(currentBarrel.transform.position, playerObj.transform.position);
        currentBarrel.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 2, calculatedForce));
    }
}
