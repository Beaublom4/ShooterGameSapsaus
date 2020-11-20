using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public int damage;
    public GameObject hitBox;
    public Animator anim;

    public bool canMutateToBigZomb;
    [HideInInspector] public bool countTowardsBigZomb;
    public bool addedToList;
    [HideInInspector] public Vector3 main;
    [HideInInspector] public bool isMainBody;
    [HideInInspector] public bool moveTowardMain;

    public float chanceToHoldGun, chanceToHoldMelee;
    public Gun[] gunOptions;
    public Melee[] meleeOptions;
    [HideInInspector] public Gun holdingGun;
    [HideInInspector]   public Melee holdingMelee;

    public GameObject playerObj;
    [HideInInspector] public bool isAttacking, isWalking;
    [HideInInspector] public NavMeshAgent agent;

    public float hitCooldownTime = 2;
    public bool isColliding;
    bool hitCooldown;

    public Transform shootPos;
    [HideInInspector] public bool playerInShootingRange;
    [HideInInspector] public float shootTimer;
    GameObject player;

    [HideInInspector] public bool getDamageOverTime;
    [HideInInspector] public float damageOverTime;
    public virtual void Start()
    {
        player = GameObject.Find("Player");
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
            transform.LookAt(main);
            transform.position = Vector3.Lerp(transform.position, main, 4 * Time.deltaTime);
            if (Vector3.Distance(main, transform.position) <= 1 && isMainBody == false)
            {
                Destroy(gameObject);
            }
            else if(Vector3.Distance(main, transform.position) <= 1 && isMainBody == true)
            {
                GetComponent<DefaultZombie>().MainAtLoc = true;
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
                shootTimer = holdingGun.fireRate;
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
            float length;
            if (anim != null)
            {
                anim.SetBool("Aggro", true);
                length = anim.GetCurrentAnimatorClipInfo(0).Length;
            }
            else
            {
                length = 0;
            }
            Invoke("GoAttack", length);
        }
    }
    public void GoAttack()
    {
        isAttacking = true;
        if(anim != null)
        anim.SetBool("Walking", true);
    }
    public void SpawnWithWeapon()
    {
        if (chanceToHoldGun != 0 || chanceToHoldMelee != 0)
        {
            if (chanceToHoldGun > 0 || chanceToHoldMelee > 0)
            {
                int randomNumber = Random.Range(0, 100);
                if (randomNumber <= chanceToHoldGun)
                {
                    int weaponNumber = Random.Range(0, gunOptions.Length);
                    holdingGun = gunOptions[weaponNumber];
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
            player = collision.gameObject;
            StartCoroutine(Hit());
        }
    }
    public void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = false;
        }
    }
    public virtual IEnumerator Hit()
    {
        if (isColliding == true && hitCooldown == false)
        {
            hitCooldown = true;
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            hitBox.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            hitBox.SetActive(false);
            if (anim != null)
            {
                anim.SetTrigger("Attack");
                yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0).Length);
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
            agent.speed = speed;
            hitCooldown = false;
            StartCoroutine(Hit());
        }
    }
    public void HitBoxHit()
    {
        player.GetComponent<HealthManager>().DoDamage(damage);
    }
    public void DoDamage(Weapon weapon, int hitPoint)
    {
        float range = Vector3.Distance(playerObj.transform.position, transform.position);
        float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
        if(hitPoint == 1)
        {
            calculatedDamage = calculatedDamage * weapon.critMultiplier;
        }
        health -= calculatedDamage;
        if(health <= 0)
        {
            health = 0;
            StartCoroutine(Dead(hitPoint));
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
    public virtual IEnumerator Dead(int hitPoint)
    {
        agent.isStopped = true;
        if (hitPoint != 0)
        {
            anim.SetInteger("Dead", hitPoint);
        }
        else
        {
            anim.SetInteger("Dead", 2);
        }
        //death animation
        print(gameObject.name + " died");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
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
                playerObj.GetComponent<HealthManager>().DoDamageWithGun(holdingGun, gameObject);
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
        shootTimer = holdingGun.fireRate;
        agent.speed = speed;
    }
}
