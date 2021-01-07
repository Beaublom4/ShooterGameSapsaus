using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSwitch : MonoBehaviour
{
    public Collider itemInWires;
    
    public void UseSwitch()
    {
        GetComponent<Animator>().SetTrigger("Lever");
        GetComponent<Collider>().enabled = !enabled;
        itemInWires.enabled = enabled;
    }
}
