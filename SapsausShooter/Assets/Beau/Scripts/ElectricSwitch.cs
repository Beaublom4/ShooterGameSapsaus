using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSwitch : MonoBehaviour
{
    public LauncherPart itemInWires;
    public GameObject electricShader;

    public void UseSwitch()
    {
        GetComponent<Animator>().SetTrigger("Lever");
        GetComponent<Collider>().enabled = !enabled;
        itemInWires.cantPickUp = false;
        electricShader.SetActive(false);
    }
}
