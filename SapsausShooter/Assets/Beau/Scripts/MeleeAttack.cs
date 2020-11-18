using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public bool hasMeleeWeapon;
    public Melee testWeapon;

    public GameObject mainCam;
    public Transform mainCamPos;
    public Transform[] meleeCamPos;
    public float camChangeSpeed = 5, meleeDuration;
    Transform wantedLoc, wantedLookAt;
    public bool canMelee = true, isReturning;
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
                        wantedLoc = meleeCamPos[i];
                        wantedLookAt = transform;
                        moveCam = true;
                        break; 
                    }
                }
                StartCoroutine(MeleeTiming());
            }
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
    void DoHitBox()
    {
        Instantiate(testWeapon.hitBox, transform);
    }
    void DestroyHitBox()
    {
        foreach(Transform child in transform)
        {
            if (child.GetComponent<HitBoxMelee>())
            {
                Destroy(child.gameObject);
            }
        }
    }
    IEnumerator MeleeTiming()
    {
        DoHitBox();
        yield return new WaitForSeconds(meleeDuration);

        DestroyHitBox();

        wantedLoc = mainCamPos;
        isReturning = true;
        moveCam = true;
        yield return new WaitForSeconds(2);
        canMelee = true;
    }
}
