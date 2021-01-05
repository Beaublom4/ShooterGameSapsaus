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
    SkinnedMeshRenderer render;
    MaterialPropertyBlock block;
    public float dissolvingNumber, freezeRenderNumber;

    bool isMeleeAttacking, lookAtPlayer;
    NavMeshAgent agent;


    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;

        render = GetComponentInChildren<SkinnedMeshRenderer>();
        block = new MaterialPropertyBlock();
        block.SetFloat("Vector1_4FF20CCE", dissolvingNumber);
        freezeRenderNumber = -1;
        block.SetFloat("Vector1_76374516", freezeRenderNumber);
        render.SetPropertyBlock(block);

        agent.SetDestination(playerObj.transform.position);
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
        if(lookAtPlayer == true)
        {
            transform.LookAt(new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z));
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
            int randomNum = Random.Range(0, 4);
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
            int randomNum = Random.Range(0, 3);
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
        dog.GetComponent<DogZombie>().Trigger(playerObj);
    }
    void ShockWaveAttack()
    {
        RandomAttack();
    } 
    void ToxicBoyTrow()
    {
        lookAtPlayer = true;
        StartCoroutine(DoToxicBarrelTrow());
    }
    IEnumerator DoToxicBarrelTrow()
    {
        Instantiate(nuclearBarrel, barrelSpawnPoint.transform.position, barrelSpawnPoint.rotation, barrelSpawnPoint);
        currentBarrel = barrelSpawnPoint.GetChild(0).gameObject;
        yield return new WaitForSeconds(2);
        // trow anim
        currentBarrel.transform.SetParent(null);
        currentBarrel.GetComponent<Rigidbody>().useGravity = true;
        yield return new WaitForEndOfFrame();
        float calculatedForce = Vector3.Distance(currentBarrel.transform.position, playerObj.transform.position) * 12;
        currentBarrel.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 1000, calculatedForce));
        currentBarrel.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(100, 100, 100));
    }
}
