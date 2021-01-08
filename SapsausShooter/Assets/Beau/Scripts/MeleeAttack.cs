using UnityEngine;
using System.Collections;

public class MeleeAttack : MonoBehaviour
{
    public Melee weapon;

    [HideInInspector] public Slot currentSlot;
    public AmmoCounter ammoScript;
    public GameObject mainCam, weaponWheel, optionsPanel;
    public Transform mainCamPos;
    public Transform[] meleeCamPos;
    public float camChangeSpeed = 5, meleeDuration;
    Transform wantedLoc, wantedLookAt;
    public bool canMelee = true, isReturning;
    bool moveCam;
    public MouseLook camScript;
    public Animator playerAnim;
    
    public GameObject scytheParticlesParent, mailboxParticlesParent, trashCanParticle;

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
                if (weaponWheel.activeSelf == false && optionsPanel.activeSelf == false)
                {
                    if (weapon != null && canMelee == true)
                    {
                        currentSlot.ammoInMag--;
                        ammoScript.UpdateMeleeAmmo(currentSlot.ammoInMag);

                        canMelee = false;
                        camScript.enabled = !enabled;
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
        camScript.enabled = enabled;
        GetComponent<Movement>().enabled = enabled;
        mainCam.transform.localRotation = Quaternion.Euler(Vector3.zero);
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
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GunLayer"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Melee"), 1);
        if (weapon.weaponName == "Scythe")
        {
            playerAnim.SetTrigger("MeleeHitS");
            foreach (Transform child in scytheParticlesParent.transform)
            {
                if (child.GetComponent<ParticleSystem>())
                {
                    child.GetComponent<ParticleSystem>().Play();
                }
            }
        }
        else if (weapon.weaponName == "MailBox")
        {
            playerAnim.SetTrigger("MeleeHit");
            StartCoroutine(playVFX(mailboxParticlesParent, .5f));
        }
        else if (weapon.weaponName == "TrashCan")
        {
            foreach (Transform child in trashCanParticle.transform)
            {
                if (child.GetComponent<ParticleSystem>())
                {
                    child.GetComponent<ParticleSystem>().Play();
                }
            }
        }
        DoHitBox();
        yield return new WaitForSeconds(meleeDuration);

        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("GunLayer"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("Melee"), 0);
        DestroyHitBox();

        wantedLoc = mainCamPos;
        isReturning = true;
        moveCam = true;
        yield return new WaitForSeconds(2);
        canMelee = true;
    }
    IEnumerator playVFX(GameObject parent, float time)
    {
        yield return new WaitForSeconds(time);
        foreach (Transform child in parent.transform)
        {
            if (child.GetComponent<ParticleSystem>())
            {
                child.GetComponent<ParticleSystem>().Play();
            }
        }
    }
}
