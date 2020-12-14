using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public Melee weapon;

    [HideInInspector] public Slot currentSlot;
    public AmmoCounter ammoScript;
    public GameObject mainCam, weaponWheel;
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
        foreach(Transform t in meleeCamPos)
        {
            Debug.DrawRay(transform.position, t.position - transform.position, Color.red);
        }

        if (moveCam == true)
        {
            SwitchCamPos();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            if (currentSlot != null && currentSlot.ammoInMag > 0)
                if (weaponWheel.activeSelf == false)
                    if (weapon != null && canMelee == true)
                    {
                        currentSlot.ammoInMag--;
                        ammoScript.UpdateMeleeAmmo(currentSlot.ammoInMag);

                        canMelee = false;
                        mainCam.GetComponent<MouseLook>().enabled = !enabled;
                        GetComponent<Movement>().enabled = !enabled;

                        for (int i = 0; i < meleeCamPos.Length; i++)
                        {
                            if (Physics.Raycast(transform.position, meleeCamPos[i].position - transform.position, out hit, Vector3.Distance(transform.position, meleeCamPos[i].position), ~ignoreLayer, QueryTriggerInteraction.Ignore))
                            {
                                print(hit.collider.name);
                            }
                            else
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
            //moveCam = false;
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
        Instantiate(weapon.hitBox, transform);
    }
    void DestroyHitBox()
    {
        foreach(Transform child in transform)
        {
            if (child.gameObject.tag == "MeleeHitBox")
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
