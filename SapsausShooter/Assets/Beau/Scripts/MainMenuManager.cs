using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel, optionsPanel, exitGamePanel;
    public GameObject mainCamPos, optionsCamPos, exitGameCamPos;
    public GameObject mainCamLookAt, optionsCamLookAt, exitGameLookAt;

    public GameObject cam;
    public bool moveToPos;
    public float camMoveSpeed, camRotSpeed;
    public Transform wantedPos, wantedLookAt;

    public Vector3 moveButtonToPos;
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
