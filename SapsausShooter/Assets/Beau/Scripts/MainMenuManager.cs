using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class optionsThings
    {
        public Slider[] soundSliders;
        public Slider sensitivitySlider;
        public Toggle fullScreenToggle;
        public TMP_Dropdown resDropdown;
    }
    public GameObject mainMenuPanel, optionsPanel, exitGamePanel;
    public GameObject mainCamPos, optionsCamPos, exitGameCamPos;
    public GameObject mainCamLookAt, optionsCamLookAt, exitGameLookAt;

    public GameObject cam;
    public bool moveToPos;
    public float camMoveSpeed, camRotSpeed;
    public Transform wantedPos, wantedLookAt;

    public AudioMixer audioMixer;

    public Vector3 moveButtonToPos;

    public TextMeshProUGUI resText;

    public optionsThings options;
    private void Start()
    {
        PreScene.defaultRes = Screen.currentResolution;
        resText.text = PreScene.defaultRes.width.ToString() + "x" + PreScene.defaultRes.height.ToString();
    }
    private void Update()
    {
        if(moveToPos == true)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, wantedPos.position, camMoveSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, wantedPos.rotation, camRotSpeed * Time.deltaTime);
            if(Vector3.Distance(cam.transform.position, wantedPos.position) <= .2f)
            {
                moveToPos = false;

                wantedPos = null;
                wantedLookAt = null;
            }
        }
    }
    public void StartNewGame()
    {

    }
    public void LoadGame()
    {

    }
    public void Options()
    {
        DisableAllPanels();
        optionsPanel.SetActive(true);

        wantedPos = optionsCamPos.transform;
        wantedLookAt = optionsCamLookAt.transform;
        moveToPos = true;
    }
    public void Exit()
    {
        DisableAllPanels();
        exitGamePanel.SetActive(true);

        wantedPos = exitGameCamPos.transform;
        wantedLookAt = exitGameLookAt.transform;
        moveToPos = true;
    }
    public void BackToMenu()
    {
        DisableAllPanels();
        mainMenuPanel.SetActive(true);

        wantedPos = mainCamPos.transform;
        wantedLookAt = mainCamLookAt.transform;
        moveToPos = true;
    }

    public void MouseOverButton(GameObject text)
    {
        text.transform.localPosition = moveButtonToPos;
    }
    public void MouseHoverStop(GameObject text)
    {
        text.transform.position = text.transform.parent.transform.position;
    }
    public void QuitGame()
    {
        print("Quit Game");
        Application.Quit();
    }
    public void SliderChange(SliderInfo slider)
    {
        audioMixer.SetFloat(slider.group.name, Mathf.Log10(slider.slider.value) * 20);
    }
    public void SliderUp(AudioSource audio)
    {
        audio.Play();
    }
    public void SensSliderChange()
    {

    }
    public void FullScreenToggle(Toggle toggle)
    {
        if(toggle.isOn == true)
        {
            print("Full screen");
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else if(toggle.isOn == false)
        {
            print("Windowed");
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
    public void ScreenResChange(TMP_Dropdown dropdown)
    {
        if(dropdown.value == 0)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreenMode);
        }
        else if(dropdown.value == 1)
        {
            Screen.SetResolution(1366, 768, Screen.fullScreenMode);
        }
        else if (dropdown.value == 2)
        {
            Screen.SetResolution(800, 600, Screen.fullScreenMode);
        }
        print(Screen.currentResolution);
    }
    public void ResetSettings()
    {
        print("Reset Settings");
        foreach(Slider s in options.soundSliders)
        {
            s.value = 1;
        }
        options.fullScreenToggle.isOn = true;
        options.resDropdown.value = 0;
    }
    void DisableAllPanels()
    {
        print("Disable all panels");
        mainMenuPanel.SetActive(false);
        foreach(Transform child in mainMenuPanel.transform.GetChild(1).transform)
        {
            if (child.GetComponent<Button>())
            {
                MouseHoverStop(child.GetChild(0).gameObject);
            }
        }
        optionsPanel.SetActive(false);
        foreach (Transform child in optionsPanel.transform.GetChild(1).transform)
        {
            if (child.GetComponent<Button>())
            {
                MouseHoverStop(child.GetChild(0).gameObject);
            }
        }
        exitGamePanel.SetActive(false);
        foreach (Transform child in exitGamePanel.transform.GetChild(1).transform)
        {
            if (child.GetComponent<Button>())
            {
                MouseHoverStop(child.GetChild(0).gameObject);
            }
        }
    }
}
