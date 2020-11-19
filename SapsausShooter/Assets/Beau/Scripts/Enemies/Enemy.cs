using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public int damage;
    public float deathAnimTime = 2;
    public Animator anim;

    public bool canMutateToBigZomb;
    [HideInInspector] public bool countTowardsBigZomb;
    public bool addedToList;
    [HideInInspector] public GameObject main;
    [HideInInspector] public bool moveTowardMain;

    public float chanceToHoldGun, chanceToHoldMelee;
    public Gun[] gunOptions;
    public Melee[] meleeOptions;
    public Gun holdindGun;
    public Melee holdingMelee;

    public GameObject playerObj;
    public bool isAttacking, isWalking;
    [HideInInspector] public NavMeshAgent agent;

    public float hitCooldownTime = 2;
    public bool isColliding;
    bool hitCooldown;

    public Transform shootPos;
    public bool playerInShootingRange;
    public float shootTimer;

    [HideInInspector] public bool getDamageOverTime;
    [HideInInspector] public float damageOverTime;
    public virtual void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        SpawnWithWeapon();
    }
    public virtual void Update()
    {
        if (isAttacking == true)
        {
            if(playerObj != null)
            agent.SetDestination(playerObj.transform.position);
        }
        if(getDamageOverTime == true)
        {
            health -= damageOverTime * Time.deltaTime;
        }
        if (moveTowardMain == true)
        {
            transform.LookAt(main.transform);
            transform.position = Vector3.Lerp(transform.position, main.transform.position, 4 * Time.deltaTime);
            if (Vector3.Distance(main.transform.position, transform.position) <= 1)
            {
                Destroy(gameObject);
            }
        }
        if(playerInShootingRange == true)
        {
            if(shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else if(shootTimer <= 0)
            {
                Shoot();
                shootTimer = holdindGun.fireRate;
            }
        }
    }
    public virtual void Trigger(GameObject player)
    {
        if(playerObj == null)
        {
            playerObj = player;
        }
        if (canMutateToBigZomb == true)
        {
            Invoke("CountsToBigZombieCooldown", 3);
        }
        if(isWalking == false)
        {
            isWalking = true;
            anim.SetBool("Aggro", true);
            float length = anim.GetCurrentAnimatorClipInfo(0).Length;
            print(length);
            Invoke("GoAttack", length);
        }
    }
    public void GoAttack()
    {
        isAttacking = true;
        anim.SetBool("Walking", true);
    }
    public void SpawnWithWeapon()
    {
        if (chanceToHoldGun != 0 || chanceToHoldMelee != 0)
        {
            if (chanceToHoldGun > 0 || chanceToHoldMelee > 0)
            {
                int randomNumber = Random.Range(0, 100);
                print(randomNumber);
                if (randomNumber <= chanceToHoldGun)
                {
                    int weaponNumber = Random.Range(0, gunOptions.Length);
                    holdindGun = gunOptions[weaponNumber];
                }
                else if (randomNumber > chanceToHoldGun && randomNumber <= (chanceToHoldGun + chanceToHoldMelee))
                {
                }
                else
                {
                }
            }
        }
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = true;
            StartCoroutine(Hit(collision.gameObject));
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = false;
        }
    }
    public virtual IEnumerator Hit(GameObject player)
    {
        if (isColliding == true && hitCooldown == false)
        {
            hitCooldown = true;
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            anim.SetTrigger("Attack");
            player.GetComponent<HealthManager>().DoDamage(damage);
            yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
            agent.speed = speed;
            hitCooldown = false;
            StartCoroutine(Hit(player));
        }
    }
    public void DoDamage(Weapon weapon)
    {
        float range = Vector3.Distance(playerObj.transform.position, transform.position);
        float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
        health -= calculatedDamage;
        if(health <= 0)
        {
            health = 0;
            StartCoroutine(Dead());
        }
        if(weapon.damageOverTime != 0)
        {
            float getDamageOverTime = weapon.damageOverTime / weapon.damageOverTimeTime;
            StartCoroutine(DoDamageOvertime(getDamageOverTime, weapon.damageOverTimeTime));
        }
        if(weapon.canStun == true)
        {
            StartCoroutine(DoStun(weapon.stunTime));
        }
        else if(weapon.canSlow == true)
        {
            StartCoroutine(DoSlow(weapon.slowTime, weapon.slowTimesNumber));
        }
    }
    IEnumerator DoDamageOvertime(float weaponDamageOverTime, float damageOverTimeTime)
    {
        damageOverTime = weaponDamageOverTime;
        getDamageOverTime = true;
        yield return new WaitForSeconds(damageOverTimeTime);
        getDamageOverTime = false;
        damageOverTime = 0;
    }
    IEnumerator DoStun(float stunTime)
    {
        agent.speed = 0;
        //stun anim
        yield return new WaitForSeconds(stunTime);
        agent.speed = speed;
    }
    IEnumerator DoSlow(float slowTime, float slowSpeed)
    {
        agent.speed = speed * slowSpeed;
        //slow anim things
        yield return new WaitForSeconds(slowTime);
        agent.speed = speed;
    }
    IEnumerator Dead()
    {
        //death animation
        print(gameObject.name + " died");
        yield return new WaitForSeconds(deathAnimTime);
        Destroy(gameObject);
    }
    void CountsToBigZombieCooldown()
    {
        countTowardsBigZomb = true;
    }
    void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(shootPos.position, playerObj.transform.position - transform.position, out hit, 1000, -5, QueryTriggerInteraction.Ignore))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                playerObj.GetComponent<HealthManager>().DoDamageWithGun(holdindGun, gameObject);
            }
        }
    }
    public void PlayerInShootingRange()
    {
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        playerInShootingRange = true;
        transform.LookAt(playerObj.transform);
    }
    public void PlayerOutOfShootingRange()
    {
        shootTimer = holdindGun.fireRate;
        agent.speed = speed;
    }
}
