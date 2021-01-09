using JetBrains.Annotations;
using System.ComponentModel;
using System.Configuration;
using TMPro;
using UnityEngine;
using System.Collections;

public class MissionManager : MonoBehaviour
{
    [System.Serializable]
   public class MainMission
    {
        public string missionName;
        public string[] missionTasks;
        public bool completed;
    }
    [System.Serializable]
    public class SideMission
    {
        public string missionName;
        public string missionTask;
        public bool completed;
    }

    public MainMission[] mainMissions;
    public SideMission[] sideMissions;
    public int currentMainMission, currentSideMission;
    public TextMeshProUGUI mainMissionNameText, sideMission1NameText;
    public TextMeshProUGUI mainMissionInfoText, sideMission1InfoText;
    public MoneyManager moneyManagerScript;

    public int killCount;
    public VoiceLineCol kills;

    public int partVoiceLIie;
    public VoiceLineCol partBetweenVoiceLine;
    public VoiceLineCol[] partVoiceLines;

    public float timeLeft;
    public TextMeshProUGUI timeLeftText;

    public GameObject partsDisplay;
    public GameObject[] partsOnDisplay;
    public int partsFound, partsToFind;
    public GameObject particlesWall;
    public AudioSource foundPart, allPartsFound;
    public AudioSource startSideMis, winSideMis, failSideMis;

    public bool killEnemiesMission;
    public int killAmount, currentKillAmount;
    int moneyAmount;

    private void Start()
    {
        RocketLauncherMission();
        StartCoroutine(SelectSideMission());
    }
    private void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timeLeftText.text = timeLeft.ToString("F0");
        }
        else if(timeLeft < 0)
        {
            timeLeft = 0;

            failSideMis.Play();
            currentSideMission = -1;
            killEnemiesMission = false;

            StartCoroutine(SelectSideMission());
        }
        if(killEnemiesMission == true)
        {
            sideMission1InfoText.text = currentKillAmount.ToString() + "/" + killAmount.ToString();
            if(currentKillAmount >= killAmount)
            {
                winSideMis.Play();
                killEnemiesMission = false;
                currentSideMission = -1;
                timeLeft = 0;
                moneyManagerScript.GetMoney(moneyAmount);
                StartCoroutine(SelectSideMission());
                currentKillAmount = 0;
            }
        }
    }
    public IEnumerator SelectSideMission()
    {
        timeLeftText.text = "";
        int waitForSeconds = Random.Range(0, 60);
        yield return new WaitForSeconds(waitForSeconds);
        startSideMis.Play();
        int randomSideMission = Random.Range(0, 1);
        switch (randomSideMission)
        {
            case -1:
                print("Eng");
                break;
            case 0:
                KillMobsSideMission();
                currentSideMission = 0;
                SideMissionSetup();
                break;
            case 1:
                currentSideMission = 1;
                Debug.LogError("wrong side mission");
                SideMissionSetup();
                break;
        }
    }
    public void SideMissionSetup()
    {
        if (currentSideMission < sideMissions.Length && currentSideMission >= 0)
        {
            sideMission1NameText.text = sideMissions[currentSideMission].missionName;
            sideMission1InfoText.text = sideMissions[currentSideMission].missionTask;
        }
        else
        {
            sideMission1NameText.text = "";
            sideMission1InfoText.text = "";
        }
    }
    public void RocketLauncherMission()
    {
        print("Start rocket launcher mission");
        partsDisplay.SetActive(true);
        mainMissionNameText.text = mainMissions[currentMainMission].missionName;
        mainMissionInfoText.text = "Parts collected " + partsFound + "/" + partsToFind;
    }
    public void RLPickUp(int partNumber, GameObject toDestroy)
    {
        Destroy(toDestroy);
        foundPart.Play();

        partVoiceLines[partsFound].StartSound();
        if(partsFound == 2)
        {
            StartCoroutine(WaitForPartSound());
        }
        partsFound++;
        partsOnDisplay[partNumber].SetActive(true);
        
        mainMissionInfoText.text = "Parts collected " + partsFound + "/" + partsToFind;

        if(partsFound == partsToFind)
        {
            partsDisplay.SetActive(false);
            allPartsFound.Play();
            BossBattleMission();
            print("All parts found");
        }
    }
    IEnumerator WaitForPartSound()
    {
        yield return new WaitForSeconds(10);
        partBetweenVoiceLine.StartSound();
    }
    public void BossBattleMission()
    {
        currentMainMission++;
        particlesWall.SetActive(true);
        mainMissionNameText.text = mainMissions[currentMainMission].missionName;
        mainMissionInfoText.text = mainMissions[currentMainMission].missionTasks[0] + "<br>" + mainMissions[currentMainMission].missionTasks[1];
        print("Start boss battle mission");
    }
    public void DestroyWall()
    {
        mainMissionInfoText.text = mainMissions[currentMainMission].missionTasks[1];
    }
    public void KillMobsSideMission()
    {
        print("Start kill mobs side mission");
        int randomKillAmount = Random.Range(0, 3);
        switch (randomKillAmount)
        {
            case 0:
                killAmount = 5;
                timeLeft = 120;
                moneyAmount = 100;
                break;
            case 1:
                killAmount = 10;
                timeLeft = 300;
                moneyAmount = 200;
                break;
            case 2:
                killAmount = 25;
                timeLeft = 600;
                moneyAmount = 500;
                break;
        }
        killEnemiesMission = true;
    }
    public void AddToKillCount()
    {
        killCount++;
        if(killCount >= 30)
        {
            kills.StartSound();
        }
    }
}
