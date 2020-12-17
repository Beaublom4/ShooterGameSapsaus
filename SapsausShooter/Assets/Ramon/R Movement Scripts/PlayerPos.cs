using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerPos : MonoBehaviour
{
    private Checkpoint cp;

    void Start()
    {
        cp = GameObject.FindGameObjectWithTag("GM").GetComponent<Checkpoint>();
        transform.position = cp.lastCheckPointPos;
        print("starting position set");
    }

    void Update()
    {
        //om te kunnen testen of checkpoints werken
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("reset scene");
        }
    }
}
