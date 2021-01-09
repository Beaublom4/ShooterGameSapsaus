using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{
    [System.Serializable]
    public class Line
    {
        public AudioSource voiceAudio;
        public bool hasPlayed;
        public VoiceLineCol colScript;
    }
    public Line[] lines;

    public VoiceLineCol firstTenSec;
    public void PlaySound(VoiceLineCol _ColScript)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].colScript == _ColScript)
            {
                if (lines[i].hasPlayed == false)
                {
                    lines[i].hasPlayed = true;
                    lines[i].voiceAudio.Play();
                }
            }
        }
    }
    private void Start()
    {
        StartCoroutine(Timer());
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(10);
        PlaySound(firstTenSec);
    }
}
