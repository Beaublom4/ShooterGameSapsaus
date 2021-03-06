﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Ending : MonoBehaviour
{
    public GameObject panel;

    public TextMeshProUGUI kills, deaths, timePlayed;
    public MissionManager missionScript;
    public HealthManager healthScript;
    public GameObject speedRunTimer;
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject.name);
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "PickUpCol")
        {
            speedRunTimer.SetActive(false);
            kills.text = "Kills: " + missionScript.killCount.ToString();
            deaths.text = "Deaths: " + healthScript.deaths.ToString();
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            var value = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timePlayed.text = "Time played: " + value;
            panel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Invoke("SetTime", 3);
        }
    }
    void SetTime()
    {
        Time.timeScale = 0;
        
    }
}
