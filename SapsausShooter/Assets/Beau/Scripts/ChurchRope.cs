using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChurchRope : MonoBehaviour
{
    public BoxCollider launcherPartCol;
    public GameObject rope1, rope2;

    public GameObject[] areas;
    public AudioSource bell;
    public void DoChurchRope()
    {
        GetComponent<Collider>().enabled = !enabled;
        launcherPartCol.enabled = enabled;
        bell.Play();
        rope1.SetActive(false);
        rope2.SetActive(true);
        foreach(GameObject g in areas)
        {
            foreach(Transform child in g.transform)
            {
                if (child.GetComponent<Enemy>())
                {
                    child.GetComponent<Enemy>().Trigger(GameObject.FindGameObjectWithTag("Player"));
                }
            }
        }
    }
}
