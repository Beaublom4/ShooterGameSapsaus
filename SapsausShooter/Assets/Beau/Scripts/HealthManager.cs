using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;

public class HealthManager : MonoBehaviour
{
    public float health;
    public bool canGetDmg;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public Animator anim;
    public Animator hudAnim;
    public int deaths;
    public bool hasTakenDmg;

    public GameObject mainCam;
    public float camMoveSpeed, camRotateSpeed;
    public Transform deathCamPos;

    public GameObject menuPanel;
    public GameObject deathPanel;
    public Transform spawnPoint;
    public VoiceLineCol firstRespawn, hpIsLow;
    public List<GameObject> areasTriggered = new List<GameObject>();
    public Vector3 spawnLoc;
    public ChangeSpot spotScipt;
    public CharacterController gappie;
    public GameObject postProcessDmg;
    IEnumerator dmgCoroutine;
    public ShootAttack shootScript;
    bool turnOffPost;
    private void Start()
    {
        spawnLoc = spawnPoint.position;
        if (healthText != null)
        {
            healthSlider.maxValue = health;
            UpdateNumber();
        }
        else
        {
            print("NoHealthShitAssigned");
        }
        if (MainMenuManager.devMode == true)
            canGetDmg = false;
    }
    public void DoDamage(float damage)
    {
        if (canGetDmg == true)
        {
            health -= damage;
            hudAnim.SetTrigger("ShakeScreen");
            if (dmgCoroutine != null)
                StopCoroutine(dmgCoroutine);
            dmgCoroutine = DmgPostProcessing();
            StartCoroutine(dmgCoroutine);
            hasTakenDmg = true;
            if (health <= 20)
            {
                hpIsLow.StartSound();
            }
            if (health <= 0)
            {
                health = 0;
                canGetDmg = false;
                shootScript.isReloading = true;
                anim.SetTrigger("Dead");
                GetComponent<Movement>().enabled = !enabled;
                GetComponentInChildren<MouseLook>().enabled = !enabled;
                StartCoroutine(waitForDeath());
            }
            UpdateNumber();
        }
    }
    IEnumerator DmgPostProcessing()
    {
        turnOffPost = false;
        postProcessDmg.GetComponent<Volume>().weight = 1;
        yield return new WaitForSeconds(1);
        turnOffPost = true;
    }
    public void UpdateNumber()
    {
        healthText.text = health.ToString("F0");
        healthSlider.value = health;
    }
    IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(2);
        menuPanel.SetActive(false);
        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    IEnumerator coroutine;
    public bool movePlayer;
    private void Update()
    {
        if(turnOffPost == true)
        {
            postProcessDmg.GetComponent<Volume>().weight -= Time.deltaTime;
            if(postProcessDmg.GetComponent<Volume>().weight <= 0)
            {
                turnOffPost = false;
                postProcessDmg.GetComponent<Volume>().weight = 0;
            }
        }
    }
    public void Respawn()
    {
        firstRespawn.StartSound();
        deaths++;
        gappie.enabled = false;
        transform.position = spawnPoint.position;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = InvisableFrames();
        StartCoroutine(coroutine);
    }
    IEnumerator InvisableFrames()
    {
        health = healthSlider.maxValue;
        UpdateNumber();
        anim.SetTrigger("Respawn");
        deathPanel.SetActive(false);
        
        GetComponent<Movement>().enabled = enabled;
        GetComponentInChildren<MouseLook>().enabled = enabled;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        shootScript.isReloading = false;

        Time.timeScale = 1;
        gappie.enabled = true;

        yield return new WaitForSeconds(5);

        canGetDmg = true;
    }
}
