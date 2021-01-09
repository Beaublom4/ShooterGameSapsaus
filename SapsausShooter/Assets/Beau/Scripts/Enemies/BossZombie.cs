using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossZombie : MonoBehaviour
{
    public float speed, chargeSpeed, timeBetweenAttacks;
    public float spinHitDmg, chargeDmg, shockWaveParticleDmg;

    public BossMelleeHitboxScript meleeScript;
    public GameObject playerObj;
    public Transform spinAttackPlace;
    public float rangeForSpinAttack;
    public BoxCollider rushHitBox;
    public ParticleSystem shockWaveParticles;
    public ParticleSystem spitParticles;
    public GameObject nuclearBarrel;
    public GameObject currentBarrel;
    public Transform barrelSpawnPoint;
    SkinnedMeshRenderer render;
    MaterialPropertyBlock block;
    public float dissolvingNumber, freezeRenderNumber;

    public bool isBiteAttacking, isSpinAttacking, lookAtPlayer;
    NavMeshAgent agent;
    Animator anim;

    IEnumerator coroutine;

    public float bossHealth;
    public Slider bossHealthBar;
    public GameObject hitNumPrefab;
    public bool isDead, hasRunned;

    public AudioSource walk, spit, chargeHit, chargeLoop, shockWave, spin;
    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = speed;
        anim = GetComponentInChildren<Animator>();

        //bossHealthBar.maxValue = bossHealth;

        render = GetComponentInChildren<SkinnedMeshRenderer>();
        block = new MaterialPropertyBlock();
        block.SetFloat("Vector1_4FF20CCE", dissolvingNumber);
        block.SetFloat("Vector1_76374516", freezeRenderNumber);
        render.SetPropertyBlock(block);
    }
    private void Update()
    {
        if (isBiteAttacking == true)
        {
            agent.SetDestination(playerObj.transform.position);
        }
        if (isSpinAttacking == true)
        {
            agent.SetDestination(playerObj.transform.position);
            if(Vector3.Distance(spinAttackPlace.position, playerObj.transform.position) < rangeForSpinAttack)
            {
                isSpinAttacking = false;
                coroutine = DoSpinAttack();
                StartCoroutine(coroutine);
            }
        }
        if(lookAtPlayer == true)
        {
            transform.LookAt(new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z));
        }
    }
    public void GetDamage(Weapon weapon, int hitPoint, Vector3 hitLoc)
    {
        if (isDead == true)
            return;
        float range = Vector3.Distance(playerObj.transform.position, transform.position);
        float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
        if (calculatedDamage <= 0)
            calculatedDamage = 0;
        if (hitPoint == 1)
        {
            calculatedDamage = calculatedDamage * weapon.critMultiplier;
        }
        if (weapon.lifeStealPercentage > 0)
        {
            playerObj.GetComponent<HealthManager>().health += calculatedDamage * (weapon.lifeStealPercentage / 100);
        }
        GameObject g = Instantiate(hitNumPrefab, hitLoc, Quaternion.identity, null);
        g.GetComponent<DmgNumberShow>().UpdateNumber(calculatedDamage);
        bossHealth -= calculatedDamage;
        if (bossHealth <= 0)
        {
            bossHealth = 0;

            isDead = true;
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            if (coroutine != null)
                StopCoroutine(coroutine);

            anim.SetTrigger("Dead");
        }
    }
    public IEnumerator Trigger()
    {
        if (hasRunned == false)
        {
            hasRunned = true;
            anim.SetTrigger("Intro");
        }
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timeBetweenAttacks);
        RandomAttack();
    }
    public void RandomAttack()
    {
        if (isDead == true)
            return;
        int randomNum = Random.Range(0,4);
        switch (randomNum)
        {
            case 0:
                print("Spin attack");
                SpinAttack();
                break;
            case 1:
                print("Bite Attack");
                BiteAttack();
                break;
            case 2:
                print("Shockwave attack");
                coroutine = ShockWaveAttack();
                StartCoroutine(coroutine);
                break;
            case 3:
                print("ToxicTrow attack");
                ToxicBoyTrow();
                break;
        }
    }
    void SpinAttack()
    {
        anim.SetBool("Walking", true);
        agent.speed = speed;
        isSpinAttacking = true;
    }
    IEnumerator DoSpinAttack()
    {
        //instantiate hitbox
        spin.Play();
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        anim.SetTrigger("Spin");
        anim.SetBool("Walking", false);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timeBetweenAttacks);
        StartCoroutine(Trigger());
    }
    public void HitWithSpin()
    {
        playerObj.GetComponent<HealthManager>().DoDamage(spinHitDmg);
    }
    void BiteAttack()
    {
        chargeLoop.Play();
        isBiteAttacking = true;
        agent.speed = chargeSpeed;
        Invoke("SetHitBoxTrue", 2);
        anim.SetBool("Charge", true);
        anim.SetBool("Walking", false);
    }
    void SetHitBoxTrue()
    {
        rushHitBox.enabled = enabled;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger == false)
        {
            if (
                other.gameObject.tag == "Floor" || other.gameObject.tag == "Enemy" || other.gameObject.tag == "FreezeCol" || other.gameObject.tag == "Weapon" || other.gameObject.tag == "PickUpCol" || other.gameObject.GetComponent<Terrain>())
            {
                return;
            }
            if (other.gameObject.tag == "Player")
            {
                other.gameObject.GetComponent<HealthManager>().DoDamage(chargeDmg);
                if(coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StopDashAttack();
                StartCoroutine(coroutine);
            }
            else
            {
                coroutine = HitSomething();
                StartCoroutine(coroutine);
            }
        }
    }
    IEnumerator HitSomething()
    {
        yield return new WaitForSeconds(1);
        coroutine = StopDashAttack();
        StartCoroutine(coroutine);
    }
    IEnumerator StopDashAttack()
    {
        chargeHit.Play();
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        rushHitBox.enabled = !enabled;
        anim.SetBool("Charge", false);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timeBetweenAttacks);
        StartCoroutine(Trigger());
    }
    IEnumerator ShockWaveAttack()
    {
        shockWave.Play();
        anim.SetTrigger("Shockwave");
        anim.SetBool("Walking", false);
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        shockWaveParticles.Play();
        yield return new WaitForSeconds(timeBetweenAttacks);
        StartCoroutine(Trigger());
    } 
    void ToxicBoyTrow()
    {
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        lookAtPlayer = true;
        coroutine = DoToxicBarrelTrow();
        StartCoroutine(coroutine);
    }
    IEnumerator DoToxicBarrelTrow()
    {
        anim.SetTrigger("Spit");
        yield return new WaitForSeconds(.3f);
        spit.Play();
        spitParticles.Play();
        for (int i = 0; i < barrelSpawnPoint.childCount; i++)
        {
            GameObject currentBarrel = Instantiate(nuclearBarrel, barrelSpawnPoint.transform.position, barrelSpawnPoint.GetChild(i).rotation, null);
            float calculatedForce = Vector3.Distance(currentBarrel.transform.position, playerObj.transform.position) * 12;
            currentBarrel.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 1000, calculatedForce));
            currentBarrel.GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(100, 100, 100));
        }
        StartCoroutine(Trigger());
    }
    public void WalkSound()
    {
        walk.pitch = Random.Range(0.9f, 1.1f);
        walk.Play();
    }
}
