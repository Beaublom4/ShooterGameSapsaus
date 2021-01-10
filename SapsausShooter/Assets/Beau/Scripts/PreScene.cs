using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreScene : MonoBehaviour
{
    public static Resolution defaultRes;
    private void Start()
    {
        defaultRes = Screen.currentResolution;
        SceneManager.LoadScene("MainMenu");
    }
}
