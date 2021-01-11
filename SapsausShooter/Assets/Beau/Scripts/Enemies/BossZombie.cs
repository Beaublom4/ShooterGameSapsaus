using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;

public class BossZombie : MonoBehaviour
{
    [System.Serializable]
    public class NormalStats
    {
        public float normalSpeed, normalChargeSpeed, normalTimeBetweenAttacks;
        public float spinHitDmg, chargeDmg, shockWaveParticleDmg;
        public int spitTimes;
    }
    [System.Serializable]
    public class EnragedStats
    {
        public float wantedHealthPercentage;
        public float enragedSpeed, enragedChargeSpeed, enragedTimeBetweenAttacks;
        public float spinHitDmg, chargeDmg, shockWaveParticleDmg;
        public int spitTimes;
    }
    [System.Serializable]
    public class MadAFStats
    {
        public float wantedHealthPercentage;
        public float madAFSpeed, madAFChargeSpeed, madAFTimeBetweenAttacks;
        public float spinHitDmg, chargeDmg, shockWaveParticleDmg;
        public int spitTimes;
    }

    public NormalStats normalStats;
    public EnragedStats enragedStats;
    public MadAFStats madAFStats;

    [HideInInspector] public float speed, chargeSpeed, timeBetweenAttacks;
    [HideInInspector] public float spinHitDmg, chargeDmg, shockWaveParticleDmg;
    [HideInInspector] public int barrelSpitTimes;

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
    public VoiceLineCol bossDead;

    public bool isBiteAttacking, isSpinAttacking, lookAtPlayer;
    NavMeshAgent agent;
    Animator anim;

    IEnumerator coroutine;

    public float bossHealth;
    public GameObject bossHealthBarObj;
    public Slider bossHealthBar;
    public TextMeshProUGUI bossHealthPercentage;
    public GameObject hitNumPrefab;
    public bool isDead, hasRunned;
    public GameObject ending;

    public AudioSource walk, spit, chargeHit, chargeLoop, shockWave, spin, battleMusic;

    public float timeForAchievementMin;
    public ShootAttack shootScript;
    public MissionManager missionManager;
    public HealthManager healthManager;
    public AchievementPopUp achievementScript;
    private void Start()
    {
        ChangeStats(normalStats.normalSpeed, normalStats.normalChargeSpeed, normalStats.normalTimeBetweenAttacks, normalStats.spinHitDmg, normalStats.chargeDmg, normalStats.shockWaveParticleDmg, normalStats.spitTimes);

        bossHealthBar.maxValue = bossHealth;
        bossHealthBar.value = bossHealth;
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
        if (isDead == false)
        {
            if (isBiteAttacking == true)
            {
                agent.SetDestination(playerObj.transform.position);
            }
            if (isSpinAttacking == true)
            {
                agent.SetDestination(playerObj.transform.position);
                if (Vector3.Distance(spinAttackPlace.position, playerObj.transform.position) < rangeForSpinAttack)
                {
                    isSpinAttacking = false;
                    if (coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    coroutine = DoSpinAttack();
                    StartCoroutine(coroutine);
                }
            }
            if (lookAtPlayer == true)
            {
                transform.LookAt(new Vector3(playerObj.transform.position.x, transform.position.y, playerObj.transform.position.z));
            }
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

        if((bossHealth / bossHealthBar.maxValue) * 100 <= enragedStats.wantedHealthPercentage && (bossHealth / bossHealthBar.maxValue) * 100 > madAFStats.wantedHealthPercentage)
        {
            print("Boss is enraged");
            ChangeStats(enragedStats.enragedSpeed, enragedStats.enragedChargeSpeed, enragedStats.enragedTimeBetweenAttacks, enragedStats.spinHitDmg, enragedStats.chargeDmg, enragedStats.shockWaveParticleDmg, enragedStats.spitTimes);
        }
        else if((bossHealth / bossHealthBar.maxValue) * 100 <= madAFStats.wantedHealthPercentage)
        {
            print("Boss is Mad As Fuck");
            ChangeStats(madAFStats.madAFSpeed, madAFStats.madAFChargeSpeed, madAFStats.madAFTimeBetweenAttacks, madAFStats.spinHitDmg, madAFStats.chargeDmg, madAFStats.shockWaveParticleDmg, madAFStats.spitTimes);
        }

        bossHealthBar.value = bossHealth;
        bossHealthPercentage.text = ((bossHealth / bossHealthBar.maxValue) * 100).ToString("F0") + "%";
        if (bossHealth <= 0)
        {
            isDead = true;
            bossHealth = 0;
            bossHealthBar.value = bossHealth;
            bossHealthPercentage.text = ((bossHealth / bossHealthBar.maxValue) * 100).ToString("F0") + "%";

            if(healthManager.deaths == 0)
            {
                achievementScript.ShowAchievement(0);
                PlayerPrefs.SetInt("WinWithoutDying", 1);
            }
            if(missionManager.killCount == 0)
            {
                achievementScript.ShowAchievement(1);
                PlayerPrefs.SetInt("WinWithoutKilling", 1);
            }
            if(timeForAchievementMin * 60 >= Time.timeSinceLevelLoad)
            {
                achievementScript.ShowAchievement(2);
                PlayerPrefs.SetInt("WinUnderMinutes", 1);
            }
            if(healthManager.hasTakenDmg == false)
            {
                achievementScript.ShowAchievement(4);
                PlayerPrefs.SetInt("WinWithoutTakingDamage", 1);
            }
            if(missionManager.smallKillChallange == true && missionManager.mediumKillChallange == true && missionManager.bigKillChallange == true)
            {
                achievementScript.ShowAchievement(6);
                PlayerPrefs.SetInt("FinishAllChallanges", 1);
            }
            if(shootScript.hasShoot == false)
            {
                achievementScript.ShowAchievement(7);
                PlayerPrefs.SetInt("OnlyUseMelee", 1);
            }
            
            achievementScript.ShowAchievement(5);
            PlayerPrefs.SetInt("KillRufus", 1);

            battleMusic.Stop();
            bossDead.StartSound();
            ending.SetActive(true);
            agent.speed = 0;
            agent.velocity = Vector3.zero;
            if (coroutine != null)
                StopCoroutine(coroutine);

            anim.SetTrigger("Dead");
        }
    }
    void ChangeStats(float _Speed, float _chargeSpeed, float _timeBetweenAttacks, float _spinHitDmg, float _chargeDmg, float _shockWaveParticleDmg, int _SpitTimes)
    {
        speed = _Speed;
        chargeSpeed = _chargeSpeed;
        timeBetweenAttacks = _timeBetweenAttacks;
        spinHitDmg = _spinHitDmg;
        chargeDmg = _chargeDmg;
        shockWaveParticleDmg = _shockWaveParticleDmg;
        barrelSpitTimes = _SpitTimes;
    }
    public IEnumerator Trigger()
    {
        if (hasRunned == false)
        {
            hasRunned = true;
            battleMusic.Play();
            bossHealthBarObj.SetActive(true);
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
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
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
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
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
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = HitSomething();
                StartCoroutine(coroutine);
            }
        }
    }
    IEnumerator HitSomething()
    {
        yield return new WaitForSeconds(.2f);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StopDashAttack();
        StartCoroutine(coroutine);
    }
    IEnumerator StopDashAttack()
    {
        chargeLoop.Stop();
        chargeHit.Play();
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        rushHitBox.enabled = !enabled;
        anim.SetBool("Charge", false);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + timeBetweenAttacks);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
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
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        StartCoroutine(Trigger());
    } 
    void ToxicBoyTrow()
    {
        agent.speed = 0;
        agent.velocity = Vector3.zero;
        lookAtPlayer = true;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = DoToxicBarrelTrow();
        StartCoroutine(coroutine);
    }
    IEnumerator DoToxicBarrelTrow()
    {
        for (int i = 0; i < barrelSpitTimes; i++)
        {
            print("Spit");
            StartCoroutine(Spit());
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(timeBetweenAttacks);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        StartCoroutine(Trigger());
    }
    IEnumerator Spit()
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
    }
    public void WalkSound()
    {
        walk.pitch = Random.Range(0.9f, 1.1f);
        walk.Play();
    }
}
