using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    public GameObject[] achievements;
    public Color gotColor;
    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("WinWithoutDying") == 1)
        {
            achievements[0].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[0].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("WinWithoutKilling") == 1)
        {
            achievements[1].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[1].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("WinUnderMinutes") == 1)
        {
            achievements[2].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[2].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("KillAllZombies") == 1)
        {
            achievements[3].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[3].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("WinWithoutTakingDamage") == 1)
        {
            achievements[4].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[4].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("KillRufus") == 1)
        {
            achievements[5].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[5].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("FinishAllChallanges") == 1)
        {
            achievements[6].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[6].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("OnlyUseMelee") == 1)
        {
            achievements[7].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[7].GetComponentInChildren<Image>().color = gotColor;
        }
        if (PlayerPrefs.GetInt("KillRobert") == 1)
        {
            achievements[8].GetComponentInChildren<TextMeshProUGUI>().color = gotColor;
            achievements[8].GetComponentInChildren<Image>().color = gotColor;
        }
    }
}
