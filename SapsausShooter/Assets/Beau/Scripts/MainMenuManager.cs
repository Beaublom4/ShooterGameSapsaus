using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [System.Serializable]
    public class Sounds
    {
        [HideInInspector] public float buttonSoundVolume;
        public AudioSource buttonPressSound, buttonHoverSound;
    }
    [System.Serializable]
    public class optionsThings
    {
        public GameObject[] soundSlidersObj;
        public Slider sensitivitySlider;
        public TextMeshProUGUI sensNum;
        public Toggle fullScreenToggle;
        public Toggle timerToggle;
        public TMP_Dropdown resDropdown;
    }
    public GameObject mainMenuPanel, optionsPanel, exitGamePanel;
    public GameObject mainCamPos, optionsCamPos, exitGameCamPos;
    public GameObject mainCamLookAt, optionsCamLookAt, exitGameLookAt;


    public GameObject cam;
    public Sounds sounds;
    public bool moveToPos;
    public float camMoveSpeed, camRotSpeed;
    public Transform wantedPos, wantedLookAt;
    public TextMeshProUGUI sensNumber;

    public AudioMixer audioMixer;

    public Vector3 moveButtonToPos;

    public TextMeshProUGUI resText;

    public optionsThings options;
    public GameObject optionsPanelObj;
    public MouseLook camScript;
    public Movement movementScript;
    public bool inGame;

    public GameObject loader;
    public Slider loadingBar;
    public TextMeshProUGUI loadingNum;

    public static bool devMode;
    public static bool timer;
    
    public static float masterVolume = -1, soundVolume = -1, musicVolume = -1, footstepVolume= -1, voiceLineVolume = -1, sensitiviy = -1;

    public GameObject deathPanel;

    private void Start()
    {
        if (masterVolume != -1)
        {
            options.soundSlidersObj[0].GetComponent<Slider>().value = masterVolume;
        }
         if (soundVolume != -1)
        {
            options.soundSlidersObj[1].GetComponent<Slider>().value = soundVolume;
        }
         if(musicVolume != -1)
        {
            options.soundSlidersObj[2].GetComponent<Slider>().value = soundVolume;
        }
         if (footstepVolume != -1)
        {
            options.soundSlidersObj[3].GetComponent<Slider>().value = footstepVolume;
        }
         if (voiceLineVolume != -1)
        {
            options.soundSlidersObj[4].GetComponent<Slider>().value = voiceLineVolume;
        }
         if (sensitiviy != -1)
        {
            options.sensitivitySlider.value = sensitiviy;
            options.sensNum.text = (sensitiviy / 100).ToString("F1");
        }
    }
    private void Update()
    {
        if(inGame == true)
        {
            if (Input.GetButtonDown("Cancel") && deathPanel)
            {
                if (optionsPanelObj.activeSelf == false)
                {
                    optionsPanelObj.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;

                    movementScript.enabled = !enabled;
                    camScript.enabled = !enabled;
                }
                else
                {
                    optionsPanelObj.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;

                    movementScript.enabled = enabled;
                    camScript.enabled = enabled;
                }
            }
        }
        if (moveToPos == true)
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
        if(loader != null)
        loader.SetActive(true);
        StartCoroutine(LoadSceneAsync("BeauScene"));
    }
    public void LoadGame()
    {

    }
    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        while (!async.isDone)
        {
            float progress = Mathf.Clamp01(async.progress / 0.9f);
            loadingBar.value = progress;
            loadingNum.text = (progress * 100).ToString("F1") + "%";
            yield return null;
        }
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
        sounds.buttonHoverSound.Play();
    }
    public void MouseHoverStop(GameObject text)
    {
        text.transform.position = text.transform.parent.transform.position;
    }
    public void QuitGame()
    {
        print("Quit Game");
        PlayerPrefs.Save();
        Application.Quit();
    }
    public void SliderChange(SliderInfo slider)
    {
        audioMixer.SetFloat(slider.group.name, Mathf.Log10(slider.slider.value) * 20);
        if(slider.staticName == "masterVolume")
        {
            masterVolume = slider.slider.value;
        }
        else if(slider.staticName == "soundVolume")
        {
            soundVolume = slider.slider.value;
        }
        else if (slider.staticName == "musicVolume")
        {
            musicVolume = slider.slider.value;
        }
        else if (slider.staticName == "footstepVolume")
        {
            footstepVolume = slider.slider.value;
        }
        else if (slider.staticName == "voiceLineVolume")
        {
            voiceLineVolume = slider.slider.value;
        }
    }
    public void SliderUp(AudioSource audio)
    {
        audio.Play();
    }
    public void SensSliderChange(SliderInfo slider)
    {
        sensitiviy = slider.slider.value;
        sensNumber.text = (slider.slider.value / 100).ToString("F1");
        if (inGame == true)
        {
            camScript.mouseSensitivity = options.sensitivitySlider.value;
        }
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
    public void DeveloperMode(Toggle toggle)
    {
        if(toggle.isOn == true)
        {
            devMode = true;
            print("DevMode on");
            PlayerPrefs.SetInt("DevMode", 1);
        }
        else
        {
            devMode = false;
            PlayerPrefs.SetInt("DevMode", 0);
        }
    }
    public void TimerToggle(Toggle toggle)
    {
        if(toggle.isOn == true)
        {
            timer = true;
            PlayerPrefs.SetInt("Timer", 1);
        }
        if (toggle.isOn == true)
        {
            timer = false;
            PlayerPrefs.SetInt("Timer", 0);
        }
    }
    public void ResetSettings()
    {
        print("Reset Settings");
        foreach (GameObject g in options.soundSlidersObj)
        {
            g.GetComponentInChildren<Slider>().value = 1;
            PlayerPrefs.SetFloat(g.GetComponent<SliderInfo>().group.name, g.GetComponentInChildren<Slider>().value);
        }
        options.sensitivitySlider.value = 250;
        sensNumber.text = (options.sensitivitySlider.value / 100).ToString("F1");
        PlayerPrefs.SetFloat("Sensitivity", options.sensitivitySlider.value);
        options.fullScreenToggle.isOn = true;
        options.resDropdown.value = 0;
        devMode = false;
        PlayerPrefs.SetInt("DevMode", 0);
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Continue()
    {
        optionsPanelObj.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        movementScript.enabled = enabled;
        camScript.enabled = enabled;
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
    public void ClickSound()
    {
        sounds.buttonPressSound.Play();
    }
}
