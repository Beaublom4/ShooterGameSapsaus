using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public bool isTiming;
    public TextMeshProUGUI text;
    public GameObject timer;

    private void Start()
    {
        if(MainMenuManager.timerBool)
        {
            timer.SetActive(true);
            isTiming = true;
        }
    }
    private void Update()
    {
        if(isTiming == true)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Time.timeSinceLevelLoad);
            var value = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            text.text = value;
        }
    }
}
