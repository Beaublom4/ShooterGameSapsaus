using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementPopUp : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI text;
    public GameObject[] achievements;

    public float speed;
    public Transform under, above;
    public bool run;
    public Vector3 wantedLoc;
    private void Update()
    {
        if(run == true)
        {
            transform.position = Vector3.Lerp(transform.position, wantedLoc, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, wantedLoc) <= 0.1)
            {
                run = false;
            }
        }   
    }
    public void ShowAchievement(int index)
    {
        image.sprite = achievements[index].GetComponentInChildren<Image>().sprite;
        text.text = achievements[index].GetComponentInChildren<TextMeshProUGUI>().text;

        StartCoroutine(Show());
    }
    IEnumerator Show()
    {
        wantedLoc = under.position;
        run = true;
        yield return new WaitForSeconds(5);
        wantedLoc = above.position;
    }
}
