﻿using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float health;
    public bool canGetDmg;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public Animator anim;
    public Animator hudAnim;

    public GameObject mainCam;
    public float camMoveSpeed, camRotateSpeed;
    public Transform deathCamPos;

    public GameObject deathPanel;
    public Transform spawnPoint;
    public List<GameObject> areasTriggered = new List<GameObject>();
    private void Start()
    {
        if (healthText != null)
        {
            healthSlider.maxValue = health;
            UpdateNumber();
        }
        else
        {
            print("NoHealthShitAssigned");
        }
    }
    public void DoDamage(float damage)
    {
        if (canGetDmg == true)
        {
            health -= damage;
            hudAnim.SetTrigger("ShakeScreen");
            if (health <= 0)
            {
                health = 0;
                canGetDmg = false;
                anim.SetTrigger("Dead");
                GetComponent<Movement>().enabled = !enabled;
                GetComponentInChildren<MouseLook>().enabled = !enabled;
                StartCoroutine(waitForDeath());
            }
            UpdateNumber();
        }
    }
    public void UpdateNumber()
    {
        healthText.text = health.ToString("F0");
        healthSlider.value = health;
    }
    IEnumerator waitForDeath()
    {
        yield return new WaitForSeconds(2);
        deathPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }
    public void Respawn()
    {
        Time.timeScale = 1;
        foreach(GameObject g in areasTriggered)
        {
            foreach(Transform child in g.transform)
            {
                if (child.GetComponent<Enemy>())
                {
                    child.GetComponent<Enemy>().UnTrigger();
                }
            }
        }
        StartCoroutine(InvisableFrames());
    }
    IEnumerator InvisableFrames()
    {
        gameObject.transform.position = spawnPoint.position;

        yield return new WaitForSeconds(0.00001f);

        health = healthSlider.maxValue;
        UpdateNumber();
        anim.SetTrigger("Respawn");
        deathPanel.SetActive(false);
        
        GetComponent<Movement>().enabled = enabled;
        GetComponentInChildren<MouseLook>().enabled = enabled;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return new WaitForSeconds(5);

        canGetDmg = true;
    }
}
