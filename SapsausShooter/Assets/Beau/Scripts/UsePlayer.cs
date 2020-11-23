using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePlayer : MonoBehaviour
{
    public GameObject fpsCam;
    public float useRange;
    private void Update()
    {
        if (Input.GetButtonDown("Use"))
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, useRange))
            {
                if (hit.transform.GetComponent<GarageDoor>())
                {
                    hit.transform.GetComponent<GarageDoor>().OpenGarageDoor();
                }
            }
        }
    }
}
