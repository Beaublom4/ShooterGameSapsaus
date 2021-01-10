using System.Collections;
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
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            kills.text = "Kills: " + missionScript.killCount.ToString();
            deaths.text = "Deaths: " + healthScript.deaths.ToString();
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            var value = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            timePlayed.text = "Time played: " + value;
            panel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
