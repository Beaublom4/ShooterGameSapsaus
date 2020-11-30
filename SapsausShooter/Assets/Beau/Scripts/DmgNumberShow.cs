using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DmgNumberShow : MonoBehaviour
{
    public float decreaseAlphaNum, raiseNum;
    public TextMeshProUGUI text;
    GameObject playerObj;
    public Color textColor;
    private void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        //Invoke("Destroy", liveTime);
    }
    private void Update()
    {
        transform.LookAt(playerObj.transform.position);
        transform.Translate(0, raiseNum * Time.deltaTime, 0, Space.World);

        textColor.a -= decreaseAlphaNum * Time.deltaTime;
        text.color = textColor;
        if(text.color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void UpdateNumber(float dmg)
    {
        text.text = dmg.ToString("F1");
    }
}
