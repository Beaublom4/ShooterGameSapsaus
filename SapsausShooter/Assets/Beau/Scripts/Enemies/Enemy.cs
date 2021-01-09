    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float soundVolume = .5f, walkSoundVolume = 0.01f, soundRange = .05f, walkSoundRange = 0.005f;
        public AudioSource trigger, attack, death, walk;
    }

    public float speed;
    public float health;
    public int damage;
    public GameObject hitBox;
    public Animator anim;
    public Collider[] hitBoxes;
    [HideInInspector]public bool isDeath;
    public Sounds sounds;

    public int chanceDrop, chanceDubbleDrop;
    public GameObject[] ammoDrops;
    public Transform ammoDropLoc;

    public int minDropAmount, maxDropAmount;
    public GameObject moneyDropPrefab;

    public bool canMutateToBigZomb;
    [HideInInspector] public bool countTowardsBigZomb;
    public bool addedToList;
    [HideInInspector] public Vector3 main;
    [HideInInspector] public bool isMainBody;
    [HideInInspector] public bool moveTowardMain;

    public float chanceToHoldGun, chanceToHoldMelee;
    public Gun[] gunOptions;
    public Melee[] meleeOptions;
    public Gun holdingGun;
    [HideInInspector]   public Melee holdingMelee;

    public GameObject playerObj;
    [HideInInspector] public bool isAttacking, isWalking;
    [HideInInspector] public NavMeshAgent agent;

    public float hitCooldownTime = 2;
    public bool isColliding;
    bool hitCooldown;

    [HideInInspector] public bool getDamageOverTime;
    [HideInInspector] public float damageOverTime;

    [HideInInspector] public SkinnedMeshRenderer render;
    public float dissolveSpeed;
    public float dissolvingNumber = 2;
    bool dissolving;
    MaterialPropertyBlock block;

    public bool inFreezeRange;
    public float freezeSpeed;
    public float freezeNum;
    public float freezeRenderNumber;
    public float freezeWalkingSpeed;

    public MissionManager missionManagerScript;

    public GameObject hitNumPrefab;
    public Transform dmgTextLoc;


    public virtual void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        render = GetComponentInChildren<SkinnedMeshRenderer>();
        missionManagerScript = GameObject.FindWithTag("MissionManager").GetComponent<MissionManager>();
        block = new MaterialPropertyBlock();
        block.SetFloat("Vector1_4FF20CCE", dissolvingNumber);
        freezeRenderNumber = -1;
        block.SetFloat("Vector1_76374516", freezeRenderNumber);
        render.SetPropertyBlock(block);
    }
    public virtual void Update()
    {
        if (isAttacking == true)
        {
            if(playerObj != null && isDeath == false)
            agent.SetDestination(playerObj.transform.position);
        }
        if(agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agent.velocity.normalized);
        }
        if(getDamageOverTime == true)
        {
            health -= damageOverTime * Time.deltaTime;
            if (isDeath == false)
            {
                isDeath = true;
                StartCoroutine(Dead(2));
                PlayDeathSound(sounds.death);
            }
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
        if(dissolving == true)
        {
            dissolvingNumber -= dissolveSpeed * Time.deltaTime;
            render.GetPropertyBlock(block);
            block.SetFloat("Vector1_4FF20CCE", dissolvingNumber);
            render.SetPropertyBlock(block);
        }
        if(freezeSpeed > 0 && inFreezeRange == true)
        {
            freezeNum += freezeSpeed * Time.deltaTime;
            freezeNum = Mathf.Clamp(freezeNum, 0, 2);

            //zet freeze shit
            freezeRenderNumber += (freezeNum * 4) * Time.deltaTime;
            freezeRenderNumber = Mathf.Clamp(freezeRenderNumber, -1, 3);
            render.GetPropertyBlock(block);
            block.SetFloat("Vector1_76374516", freezeRenderNumber);
            render.SetPropertyBlock(block);

            if (freezeNum < 1)
            {
                anim.SetFloat("speed", 1 - freezeNum);
                freezeWalkingSpeed = (speed * (1 - freezeNum));
                agent.speed = freezeWalkingSpeed;
            }
        }
        if (inFreezeRange == false & freezeNum > 0)
        {
            freezeNum -= .2f * Time.deltaTime;
            freezeNum = Mathf.Clamp(freezeNum, 0, 2);

            //zet freeze shit
            if (freezeNum < 1)
            {
                freezeSpeed = 0;
                freezeRenderNumber -= (.2f * 4) * Time.deltaTime;
                freezeRenderNumber = Mathf.Clamp(freezeRenderNumber, -1, 3);
                render.GetPropertyBlock(block);
                block.SetFloat("Vector1_76374516", freezeRenderNumber);
                render.SetPropertyBlock(block);

                anim.SetFloat("speed", 1 - freezeNum);
                freezeWalkingSpeed = (speed * (1 - freezeNum));
                agent.speed = freezeWalkingSpeed;
            }
        }
    }
    public virtual void Trigger(GameObject player)
    {
        if(sounds.trigger != null)
        PlayTriggerSound(sounds.trigger);
        if (playerObj == null)
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
    public void UnTrigger()
    {
        isWalking = false;
        isAttacking = false;
        //anim.SetBool("Walking", false);
        anim.SetBool("Aggro", false);
    }
    public void GoAttack()
    {
        isAttacking = true;
        if(anim != null)
        anim.SetBool("Walking", true);
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isColliding = true;
            playerObj = collision.gameObject;
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
            if (sounds.attack != null)
                PlayAttackSound(sounds.attack);
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
            if(freezeNum <= 0)
            {
                agent.speed = speed;
            }
            else
            {
                agent.speed = freezeWalkingSpeed;
            }
            hitCooldown = false;
            StartCoroutine(Hit());
        }
    }
    public virtual void PlayTriggerSound(AudioSource source)
    {
        source.volume = Random.Range(sounds.soundVolume - sounds.soundRange, sounds.soundVolume + sounds.soundRange);
        source.pitch = Random.Range(1 - sounds.soundRange, 1 + sounds.soundRange);
        source.Play();
    }
    public virtual void PlayAttackSound(AudioSource source)
    {
        source.volume = Random.Range(sounds.soundVolume - sounds.soundRange, sounds.soundVolume + sounds.soundRange);
        source.pitch = Random.Range(1 - sounds.soundRange, 1 + sounds.soundRange);
        source.Play();
    }
    public virtual void PlayDeathSound(AudioSource source)
    {
        source.volume = Random.Range(sounds.soundVolume - sounds.soundRange, sounds.soundVolume + sounds.soundRange);
        source.pitch = Random.Range(1 - sounds.soundRange, 1 + sounds.soundRange);
        source.Play();
    }
    public void WalkSound()
    {
        sounds.walk.volume = Random.Range(sounds.soundVolume - sounds.walkSoundRange, sounds.soundVolume + sounds.walkSoundRange);
        sounds.walk.pitch = Random.Range(1 - sounds.soundRange, 1 + sounds.soundRange);
        sounds.walk.Play();
    }
    public void HitBoxHit()
    {
        playerObj.GetComponent<HealthManager>().DoDamage(damage);
    }
    public void DoDamage(Weapon weapon, int hitPoint, Vector3 hitLoc)
    {
        if (GetComponentInParent<AreaColScript>())
        {
            if (GetComponentInParent<AreaColScript>().isAlreadyTriggered == false)
            {
                GetComponentInParent<AreaColScript>().TriggerArea(playerObj);
            }
        }
        float range = Vector3.Distance(playerObj.transform.position, transform.position);
        float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
        if (calculatedDamage <= 0)
            calculatedDamage = 0;
        if(hitPoint == 1)
        {
            calculatedDamage = calculatedDamage * weapon.critMultiplier;
        }
        if (weapon.lifeStealPercentage > 0)
        {
            playerObj.GetComponent<HealthManager>().health += calculatedDamage * (weapon.lifeStealPercentage / 100);
        }
        GameObject g = Instantiate(hitNumPrefab, hitLoc, Quaternion.identity, null);
        g.GetComponent<DmgNumberShow>().UpdateNumber(calculatedDamage);
        health -= calculatedDamage;
        if(health <= 0)
        {
            health = 0;
            if(isDeath == false)
            {
                isDeath = true;
                StartCoroutine(Dead(hitPoint));
            }
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
        if (isDeath == false)
            if (freezeNum <= 0)
            {
                agent.speed = speed;
            }
            else
            {
                agent.speed = freezeWalkingSpeed;
            }
    }
    IEnumerator DoSlow(float slowTime, float slowSpeed)
    {
        agent.speed = speed * slowSpeed;
        //slow anim things
        yield return new WaitForSeconds(slowTime);
        if (isDeath == false)
            if (freezeNum <= 0)
            {
                agent.speed = speed;
            }
            else
            {
                agent.speed = freezeWalkingSpeed;
            }
    }
    public virtual IEnumerator Dead(int hitPoint)
    {
        agent.isStopped = true;
        agent.enabled = !enabled;
        foreach(Collider c in hitBoxes)
        {
            c.enabled = !enabled;
        }
        if (hitPoint != 0)
        {
            anim.SetInteger("Dead", hitPoint);
        }
        else
        {
            anim.SetInteger("Dead", 2);
        }
        if(missionManagerScript.killEnemiesMission == true)
        {
            missionManagerScript.currentKillAmount++;
        }
        Drop();
        DropMoney();
        DropWeapon();
        dissolving = true;
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    void Drop()
    {
        int randomNum = Random.Range(0, 100);
        if(randomNum < chanceDrop)
        {
            DropAmmo();
        }
        else if(randomNum > chanceDrop && randomNum < chanceDubbleDrop + chanceDrop)
        {
            Drop();
            DropAmmo();
        }
        else
        {
            return;
        }
    }
    void DropAmmo()
    {
        int randomNum = Random.Range(0, ammoDrops.Length);
        Instantiate(ammoDrops[randomNum], ammoDropLoc.position, Quaternion.Euler(ammoDropLoc.rotation.x, Random.Range(0, 360), ammoDropLoc.rotation.z), ammoDropLoc);
        GameObject g = ammoDropLoc.transform.GetChild(0).gameObject;
        g.GetComponent<Rigidbody>().AddRelativeForce(0, 300, 10);
        g.transform.SetParent(null);
    }
    void DropWeapon()
    {
        if(holdingGun != null)
        {
            Instantiate(holdingGun.weaponPrefab, ammoDropLoc.position, Quaternion.Euler(ammoDropLoc.rotation.x, Random.Range(0, 360), ammoDropLoc.rotation.z), ammoDropLoc);
            GameObject g = ammoDropLoc.transform.GetChild(0).gameObject;
            g.GetComponent<Rigidbody>().AddRelativeForce(0, 300, 10);
            g.transform.SetParent(null);
        }
    }
    void DropMoney()
    {
        int randomNum = Random.Range(minDropAmount, maxDropAmount);
        Instantiate(moneyDropPrefab, ammoDropLoc.position, Quaternion.Euler(ammoDropLoc.rotation.x, Random.Range(0, 360), ammoDropLoc.rotation.z), ammoDropLoc);
        GameObject g = ammoDropLoc.transform.GetChild(0).gameObject;
        g.GetComponent<MoneyDrop>().moneyAmount = randomNum;
        g.GetComponent<Rigidbody>().AddRelativeForce(0, 300, 10);
        g.transform.SetParent(null);
    }
    void CountsToBigZombieCooldown()
    {
        countTowardsBigZomb = true;
    }
}
