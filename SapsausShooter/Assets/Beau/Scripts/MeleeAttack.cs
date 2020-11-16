using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public bool hasMeleeWeapon;
    public Weapon testWeapon;

    public GameObject mainCam;
    public Transform mainCamPos, meleeCamPos;
    public float camChangeSpeed, meleeDuration;
    Transform wantedLoc, wantedLookAt;
    bool canMelee = true, isReturning;
    public bool moveCam;
    public void Update()
    {
        if (moveCam == true)
        {
            SwitchCamPos();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if(hasMeleeWeapon == true && canMelee== true)
            {
                canMelee = false;
                mainCam.GetComponent<MouseLook>().enabled = !enabled;
                GetComponent<Movement>().enabled = !enabled;
                wantedLoc = meleeCamPos;
                wantedLookAt = transform;
                StartCoroutine(MeleeTiming());
                moveCam = true;
            }
        }
    }
    void SwitchCamPos()
    {
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, wantedLoc.position, camChangeSpeed * Time.deltaTime);
        mainCam.transform.LookAt(wantedLookAt);
        if(Vector3.Distance(mainCam.transform.position, wantedLoc.transform.position) <= .1f)
        {
            print("at loc");
            moveCam = false;
            if(isReturning == true)
            {
                isReturning = false;
                ResetMelee();
            }
        }
    }
    void ResetMelee()
    {
        mainCam.GetComponent<MouseLook>().enabled = enabled;
        GetComponent<Movement>().enabled = enabled;
    }
    IEnumerator MeleeTiming()
    {
        yield return new WaitForSeconds(meleeDuration);
        wantedLoc = mainCamPos;
        isReturning = true;
        moveCam = true;
        yield return new WaitForSeconds(2);
        canMelee = true;
    }
}
