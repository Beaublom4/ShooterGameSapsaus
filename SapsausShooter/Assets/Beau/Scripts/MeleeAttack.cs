using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public bool hasMeleeWeapon;
    public Weapon testWeapon;

    public GameObject mainCam;
    public Transform mainCamPos;
    public Transform[] meleeCamPos;
    public float camChangeSpeed = 5, meleeDuration;
    Transform wantedLoc, wantedLookAt;
    bool canMelee = true, isReturning;
    bool moveCam;
    
    public LayerMask ignoreLayer;
    public RaycastHit hit;

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

                for (int i = 0; i < meleeCamPos.Length; i++)
                {
                    if(!Physics.Raycast(transform.position, meleeCamPos[i].position, out hit, Vector3.Distance(transform.position, meleeCamPos[i].position), ~ignoreLayer))
                    {
                        print(i + " hit nothing");
                        wantedLoc = meleeCamPos[i];
                        wantedLookAt = transform;
                        moveCam = true;
                        break; 
                    }
                }
            }
            StartCoroutine(MeleeTiming());
        }
    }
    void SwitchCamPos()
    {
        mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, wantedLoc.position, camChangeSpeed * Time.deltaTime);
        mainCam.transform.LookAt(wantedLookAt);
        if(Vector3.Distance(mainCam.transform.position, wantedLoc.transform.position) <= .1f)
        {
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
