using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreScene : MonoBehaviour
{
    public static Resolution defaultRes;
    private void Start()
    {
        Invoke("NextScene", 2);
        defaultRes = Screen.currentResolution;
    }
    void NextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
