using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float health;
    public bool canGetDmg;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public Animator anim;
    public Animator hudAnim;

    public GameObject mainCam;
    public float camMoveSpeed, camRotateSpeed;
    public Transform deathCamPos;

    public GameObject deathPanel;
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
    public void DoDamage(float damage)
    {
        if (canGetDmg == true)
        {
            health -= damage;
            hudAnim.SetTrigger("ShakeScreen");
            if (health <= 0)
            {
                health = 0;
                anim.SetTrigger("Dead");
                GetComponent<Movement>().enabled = !enabled;
                GetComponentInChildren<MouseLook>().enabled = !enabled;
                deathPanel.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            UpdateNumber();
        }
    }
    public void DoDamageWithGun(Weapon weapon, GameObject enemy)
    {
        if (canGetDmg == true)
        {
            float range = Vector3.Distance(enemy.transform.position, transform.position);
            float calculatedDamage = weapon.damage - (weapon.damageDropOverDist * range);
            health -= calculatedDamage;
            print(calculatedDamage);
            if (health <= 0)
            {
                health = 0;
                anim.SetTrigger("Dead");
                GetComponent<Movement>().enabled = !enabled;
                GetComponent<MouseLook>().enabled = !enabled;
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
    }
    public void UpdateNumber()
    {
        healthText.text = health.ToString("F0");
        healthSlider.value = health;
    }
    public void Respawn()
    {

    }
}
