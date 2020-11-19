using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossZombie : MonoBehaviour
{
    public float speed;
    public GameObject playerObj;
    public float maxRangeMeleeAttack;

    bool walkTowardsPlayer;
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
        if (walkTowardsPlayer == true)
        {
            agent.destination = playerObj.transform.position;
        }
    }
    public void Trigger()
    {
        RandomAttack();
    }
    void RandomAttack()
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
    void MeleeAttack()
    {
        print("Melee Attack");
        walkTowardsPlayer = true;
    }
    IEnumerator DoMeleeAttack()
    {
        yield return new WaitForSeconds(1);
    }
    void DogAttack()
    {
        print("Dog");
    }
    void ShockWaveAttack()
    {
        print("Shock wave attack");
    } 
    void ToxicBoyTrow()
    {
        print("Toxic boy trow");
    }
}
