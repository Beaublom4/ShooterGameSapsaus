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

    public float timeLeft;
    public TextMeshProUGUI timeLeftText;

    public bool killEnemiesMission;
    public int killAmount, currentKillAmount;
    int moneyAmount;

    private void Start()
    {
        SelectMainMission();
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

            currentSideMission = -1;
            killEnemiesMission = false;

            StartCoroutine(SelectSideMission());
        }
        if(killEnemiesMission == true)
        {
            sideMission1InfoText.text = currentKillAmount.ToString() + "/" + killAmount.ToString();
            if(currentKillAmount >= killAmount)
            {
                killEnemiesMission = false;
                currentSideMission = -1;
                timeLeft = 0;
                moneyManagerScript.GetMoney(moneyAmount);
                StartCoroutine(SelectSideMission());
                currentKillAmount = 0;
            }
        }
    }
    public void SelectMainMission()
    {
        MainMissionSetup();
        switch (currentMainMission)
        {
            case 0:
                RocketLauncherMission();
                break;
            case 1:
                BossBattleMission();
                break;
        }
    }
    public IEnumerator SelectSideMission()
    {
        SideMissionSetup();
        timeLeftText.text = "";
        int waitForSeconds = Random.Range(0, 60);
        yield return new WaitForSeconds(waitForSeconds);
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
    public void MainMissionSetup()
    {
        if (currentMainMission < mainMissions.Length)
        {
            mainMissionNameText.text = mainMissions[currentMainMission].missionName;
            if (mainMissions[currentMainMission].missionTasks.Length == 1)
            {
                mainMissionInfoText.text = mainMissions[currentMainMission].missionTasks[0];
            }
            else if (mainMissions[currentMainMission].missionTasks.Length == 2)
            {
                mainMissionInfoText.text = mainMissions[currentMainMission].missionTasks[0] + "<br>" + mainMissions[currentMainMission].missionTasks[1];
            }
            else if (mainMissions[currentMainMission].missionTasks.Length == 3)
            {
                mainMissionInfoText.text = mainMissions[currentMainMission].missionTasks[0] + "<br>" + mainMissions[currentMainMission].missionTasks[1] + "<br>" + mainMissions[currentMainMission].missionTasks[2];
            }
            else Debug.LogError("To many tasks for mission: " + mainMissions[currentMainMission].missionName);
        }
        else
        {
            Debug.LogError("Current mission empty");
            mainMissionNameText.text = "";
            mainMissionInfoText.text = "";
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
    }
    public void BossBattleMission()
    {
        print("Start boss battle mission");
    }
    public void KillMobsSideMission()
    {
        print("Start kill mobs side mission");
        int randomKillAmount = Random.Range(0, 3);
        print(randomKillAmount);
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
}
