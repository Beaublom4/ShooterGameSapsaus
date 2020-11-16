﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int health;
    public bool canGetDmg;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    private void Start()
    {
        if (healthText == null)
        {
            healthSlider.maxValue = health;
            UpdateNumber();
        }
        else
        {
            print("NoHealthShitAssigned");
        }
    }
    public void DoDamage(int damage)
    {
        if (canGetDmg == true)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
            }
            UpdateNumber();
        }
    }
    public void UpdateNumber()
    {
        healthText.text = health.ToString("F0");
        healthSlider.value = health;
    }
}
