using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLineCol : MonoBehaviour
{
    public GameObject stopThisVoiceline;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartSound();
        }
    }
    public void StartSound()
    {
        if(stopThisVoiceline != null)
        {
            stopThisVoiceline.SetActive(false);
        }
        GetComponentInParent<VoiceLines>().PlaySound(GetComponent<VoiceLineCol>());
    }
}
