using JetBrains.Annotations;
using System.ComponentModel;
using System.Configuration;
using TMPro;
using UnityEngine;

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
        public string[] missionTasks;
        public bool completed;
    }

    public MainMission[] mainMissions;
    public SideMission[] sideMissions;
    public int currentMainMission, currentSideMission1;
    public TextMeshProUGUI mainMissionNameText, sideMission1NameText;
    public TextMeshProUGUI mainMissionInfoText, sideMission1InfoText;

    private void Start()
    {
        SelectMainMission();
        SelectSideMission();
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
    public void SelectSideMission()
    {
        SideMissionSetup();
        switch (currentSideMission1)
        {
            case -1:
                print("No Side missions selected");
                break;
            case 0:
                KillMobsSideMission(20);
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
        if (currentSideMission1 < sideMissions.Length && currentSideMission1 >= 0)
        {
            sideMission1NameText.text = sideMissions[currentSideMission1].missionName;
            if (sideMissions[currentSideMission1].missionTasks.Length == 1)
            {
                sideMission1InfoText.text = sideMissions[currentSideMission1].missionTasks[0];
            }
            else if (sideMissions[currentSideMission1].missionTasks.Length == 2)
            {
                sideMission1InfoText.text = sideMissions[currentSideMission1].missionTasks[0] + "<br>" + sideMissions[currentSideMission1].missionTasks[1];
            }
            else if (sideMissions[currentSideMission1].missionTasks.Length == 3)
            {
                sideMission1InfoText.text = sideMissions[currentSideMission1].missionTasks[0] + "<br>" + sideMissions[currentSideMission1].missionTasks[1] + "<br>" + sideMissions[currentSideMission1].missionTasks[2];
            }
            else Debug.LogError("To many tasks for mission: " + sideMissions[currentSideMission1].missionName);
        }
        else
        {
            Debug.LogError("Current side mission empty");
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
    public void KillMobsSideMission(int killAmount)
    {
        print("Start kill mobs side mission");
    }
}
