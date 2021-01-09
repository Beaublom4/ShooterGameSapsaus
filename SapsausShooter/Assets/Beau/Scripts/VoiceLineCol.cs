using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineCol : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartSound();
        }
    }
    public void StartSound()
    {
        GetComponentInParent<VoiceLines>().PlaySound(GetComponent<VoiceLineCol>());
    }
}
