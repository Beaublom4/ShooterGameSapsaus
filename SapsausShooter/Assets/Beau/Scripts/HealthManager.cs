using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float health;
    public bool canGetDmg;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;

    public GameObject mainCam;
    public float camMoveSpeed, camRotateSpeed;
    public Transform deathCamPos;
    bool death, lookDown;
    private void Start()
    {
        if (healthText != null)
        {
            healthSlider.maxValue = health;
            UpdateNumber();
        }
        else
        {
            print("NoHealthShitAssigned");
        }
    }
    private void Update()
    {
        if(death == true)
        {
            mainCam.GetComponent<MouseLook>().enabled = !enabled;
            if(lookDown == false)
            {
                lookDown = true;
                mainCam.transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            mainCam.transform.Rotate(0, 0, camRotateSpeed * Time.deltaTime);
            if (mainCam.transform.position != deathCamPos.position)
            {
                mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, deathCamPos.position, camMoveSpeed * Time.deltaTime);
            }
        }
    }
    public void DoDamage(int damage)
    {
        if (canGetDmg == true)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                death = true;
            }
            UpdateNumber();
        }
    }
    public void DoDamageWithGun(Weapon weapon, GameObject enemy)
    {
        float range = Vector3.Distance(enemy.transform.position, transform.position);
        float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
        health -= calculatedDamage;
        if (health <= 0)
        {
            health = 0;
            death = true;
        }
        //if (weapon.damageOverTime != 0)
        //{
        //    float getDamageOverTime = weapon.damageOverTime / weapon.damageOverTimeTime;
        //    StartCoroutine(DoDamageOvertime(getDamageOverTime, weapon.damageOverTimeTime));
        //}
        //if (weapon.canStun == true)
        //{
        //    StartCoroutine(DoStun(weapon.stunTime));
        //}
        //else if (weapon.canSlow == true)
        //{
        //    StartCoroutine(DoSlow(weapon.slowTime, weapon.slowTimesNumber));
        //}
        UpdateNumber();
    }
    public void UpdateNumber()
    {
        healthText.text = health.ToString("F0");
        healthSlider.value = health;
    }
}
