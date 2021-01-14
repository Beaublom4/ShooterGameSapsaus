using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class BigBabyMiniBoss : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        public float soundVolume = .5f, walkSoundVolume = 0.01f, soundRange = .05f, walkSoundRange = 0.005f;
        public AudioSource trigger, attack, death, walk, gettingFreezed;
    }
    [System.Serializable]
    public class NormalStats
    {
        public float speed, spitDamage, missleDamage, shootRate, missileSpeed, spitTimes;
    }
    [System.Serializable]
    public class EnragedStats
    {
        public float wantedHealthPercentage;
        public float speed, spitDamage, missleDamage, shootRate, missileSpeed, spitTimes;
    }
    public NormalStats normalStats;
    public EnragedStats enragedStats;

    public float speed, spitDamage, launcherDamage, shootRate, spitTimes, missleSpeed;
    public GameObject spitPrefab, misslePrefab;
    public Transform shootPos;
    public Vector3 spitDirection, missleDirection;
    public AudioSource spitSound;
    public GameObject hitNumPrefab;
    public Transform dmgTextLoc;
    public Collider[] hitBoxes;
    public Collider rangeCol;
    public GameObject[] drops;
    public AchievementPopUp achievementScript;

    public GameObject healthBar;

    public float health;
    public bool isDeath;

    float timer;

    public float dissolvingNumber;
    bool playerInRange, dissolving, walkToPlayer, spitObLookAt;
    [HideInInspector] public GameObject player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;

    Renderer render;
    MaterialPropertyBlock block;

    public Sounds sounds;

    public float appearSpeed;
    public float appearNumber;

    public bool inFreezeRange;
    public FreezeHitBox freezeScript;
    public Gun freezeWeapon;
    public float freezeSpeed;
    public float freezeNum;
    public float freezeRenderNumber;
    public float freezeWalkingSpeed;
    public bool didFreezeSound;

    public bool appear;

    void Stats(float _speed, float _spitDamage, float _launcherDamage, float _shootRate, float _spitTimes, float _missleSpeed)
    {
        speed = _speed;
        spitDamage = _spitDamage;
        launcherDamage = _launcherDamage;
        shootRate = _shootRate;
        spitTimes = _spitTimes;
        missleSpeed = _missleSpeed;
    }
    private void Start()
    {
        healthBar = GameObject.FindGameObjectWithTag("MiniBossHealthBar").transform.GetChild(0).gameObject;
        healthBar.GetComponentInChildren<Slider>().maxValue = health;
        healthBar.GetComponentInChildren<Slider>().value = health;
        healthBar.GetComponentInChildren<TextMeshProUGUI>().text = ((health / healthBar.GetComponentInChildren<Slider>().maxValue) * 100).ToString("F0") + "%";
        healthBar.SetActive(true);

        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        achievementScript = GameObject.FindGameObjectWithTag("achievement").GetComponent<AchievementPopUp>();

        render = GetComponentInChildren<SkinnedMeshRenderer>();
        block = new MaterialPropertyBlock();
        freezeRenderNumber = -1;
        block.SetFloat("Vector1_76374516", freezeRenderNumber);
        render.SetPropertyBlock(block);

        Invoke("StartAppear", 1.5f);
    }
    void StartAppear()
    {
        appear = true;
    }
    void Trigger()
    {
        Stats(normalStats.speed, normalStats.spitDamage, normalStats.missleDamage, normalStats.shootRate, normalStats.spitTimes, normalStats.missileSpeed);
        walkToPlayer = true;
        rangeCol.enabled = true;
        agent.speed = speed;
        anim.SetBool("Walking", true);
    }
    private void Update()
    {
        if(appear == true)
        {
            appearNumber += appearSpeed * Time.deltaTime;
            render.GetPropertyBlock(block);
            block.SetFloat("Vector1_4FF20CCE", appearNumber);
            render.SetPropertyBlock(block);
            if(appearNumber >= 5)
            {
                appear = false;
                appearNumber = 5;
                Trigger();
            }
        }

        if (isDeath == true)
            return;
        if(walkToPlayer == true)
        {
            agent.SetDestination(player.transform.position);
        }
        if(playerInRange == true)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
        if(spitObLookAt == true)
        {
            shootPos.LookAt(player.transform.position);
        }
        if(dissolving == true)
        {
            dissolvingNumber -= 1 * Time.deltaTime;
            render.GetPropertyBlock(block);
            block.SetFloat("Vector1_4FF20CCE", dissolvingNumber);
            render.SetPropertyBlock(block);
        }
        if (freezeSpeed > 0 && inFreezeRange == true)
        {
            freezeNum += freezeSpeed * Time.deltaTime;
            freezeNum = Mathf.Clamp(freezeNum, 0, 2);

            //zet freeze shit
            freezeRenderNumber += (freezeNum * 4) * Time.deltaTime;
            freezeRenderNumber = Mathf.Clamp(freezeRenderNumber, -1, 3);
            render.GetPropertyBlock(block);
            block.SetFloat("Vector1_76374516", freezeRenderNumber);
            render.SetPropertyBlock(block);

            if (freezeNum > 1)
            {
                if (didFreezeSound == false)
                {
                    didFreezeSound = true;
                    sounds.gettingFreezed.Play();
                }
                DoDamage(freezeWeapon, 2, new Vector3(0, -100, 0));
            }
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
                if (didFreezeSound == true)
                {
                    didFreezeSound = false;
                }

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
    void RandomAttack()
    {
        if (isDeath == true)
            return;
        int randomNum = Random.Range(0, 2);
        switch (randomNum)
        {
            case 0:
                anim.SetBool("Walking", true);
                walkToPlayer = true;
                rangeCol.enabled = true;
                agent.speed = speed;
                break;
            case 1:
                DoMissile();
                break;
        }
    }
    IEnumerator DoSpit()
    {
        anim.SetBool("Walking", false);
        rangeCol.enabled = false;
        agent.speed = 0;
        agent.velocity = Vector3.zero;

        for (int i = 0; i < spitTimes; i++)
        {
            if (isDeath == true)
                break;
            anim.SetTrigger("Attack");
            GameObject g = Instantiate(spitPrefab, shootPos);
            g.transform.SetParent(null);
            g.GetComponent<SpitBall>().damage = spitDamage;
            g.GetComponent<Rigidbody>().AddRelativeForce(spitDirection);
            yield return new WaitForSeconds(shootRate);
        }

        Invoke("RandomAttack", 3);
    }
    void DoMissile()
    {
        anim.SetBool("Walking", false);
        rangeCol.enabled = false;
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        anim.SetTrigger("Attack");

        GameObject g = Instantiate(misslePrefab, shootPos.position, transform.rotation, null);
        g.GetComponent<BigBabyMissle>().damage = launcherDamage;
        g.GetComponent<BigBabyMissle>().player = player;
        g.GetComponent<BigBabyMissle>().speed = missleSpeed;
        g.GetComponent<Rigidbody>().AddRelativeForce(missleDirection);

        Invoke("RandomAttack", 5);
    }
    public void DoDamage(Weapon weapon, int hitPoint, Vector3 hitLoc)
    {
        float range = Vector3.Distance(player.transform.position, transform.position);
        float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
        if (calculatedDamage <= 0)
            calculatedDamage = 0;
        if (MainMenuManager.devMode == true)
            calculatedDamage *= 10;
        if (hitPoint == 1)
        {
            calculatedDamage = calculatedDamage * weapon.critMultiplier;
        }
        if (weapon.lifeStealPercentage > 0)
        {
            player.GetComponent<HealthManager>().health += calculatedDamage * (weapon.lifeStealPercentage / 100);
        }
        GameObject g = Instantiate(hitNumPrefab, hitLoc, Quaternion.identity, null);
        g.GetComponent<DmgNumberShow>().UpdateNumber(calculatedDamage);
        health -= calculatedDamage;

        if ((health / healthBar.GetComponentInChildren<Slider>().maxValue) * 100 <= enragedStats.wantedHealthPercentage)
        {
            print("Boss is enraged");
            Stats(enragedStats.speed, enragedStats.spitDamage, enragedStats.missleDamage, enragedStats.shootRate, enragedStats.missileSpeed, enragedStats.spitTimes);
        }

        healthBar.GetComponentInChildren<Slider>().value = health;
        healthBar.GetComponentInChildren<TextMeshProUGUI>().text = ((health / healthBar.GetComponentInChildren<Slider>().maxValue) * 100).ToString("F0") + "%";

        if (health <= 0)
        {
            health = 0;
            if (isDeath == false)
            {
                isDeath = true;
                StartCoroutine(Dead(hitPoint));
            }
        }
    }
    public virtual IEnumerator Dead(int hitPoint)
    {
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        agent.enabled = !enabled;
        foreach (Collider c in hitBoxes)
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

        if (MainMenuManager.devMode == false)
        {
            if (PlayerPrefs.GetInt("KillRobert") != 1)
            {
                achievementScript.ShowAchievement(8);
                PlayerPrefs.SetInt("KillRobert", 1);
            }
        }

        foreach (GameObject g in drops)
        {
            GameObject drop = Instantiate(g, transform.position, Quaternion.Euler(transform.rotation.x, Random.Range(0, 360), transform.rotation.z), null);
            if(drop.tag == "Money")
            {
                drop.GetComponent<MoneyDrop>().moneyAmount = 100;
            }
            drop.GetComponent<Rigidbody>().AddRelativeForce(0, 300, 10);
        }
        dissolving = true;
        if (freezeNum == 0)
        {
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
        yield return new WaitForSeconds(3);
        if (freezeNum == 0)
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            walkToPlayer = false;
            playerInRange = true;
            spitObLookAt = true;
            anim.SetBool("Walking", true);
            StartCoroutine(DoSpit());
        }
    }
    public void WalkSound()
    {
        sounds.walk.volume = Random.Range(sounds.soundVolume - sounds.walkSoundRange, sounds.soundVolume + sounds.walkSoundRange);
        sounds.walk.pitch = Random.Range(1 - sounds.soundRange, 1 + sounds.soundRange);
        sounds.walk.Play();
    }
}
