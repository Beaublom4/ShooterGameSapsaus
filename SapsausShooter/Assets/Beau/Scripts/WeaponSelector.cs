
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelector : MonoBehaviour
{
    public GameObject weaponWheelObj;
    public GameObject player, fpsCam;
    public Transform dropLoc;
    public float pickUpRange;
    public Slot selectedSlotScript;
    public GameObject[] rings;
    public ShootAttack shootScript;
    public MeleeAttack meleeScript;
    AmmoCounter ammoCounterScript;
    public Image weaponImage;
    public Animator playerAnim;

    IEnumerator coroutine;
    public TextMeshProUGUI nameText;
    public float displayNameTime;

    [System.Serializable]
    public class Colors
    {
        public ColorBlock selectedColor, normalColor;
    }
    public Colors colors;
    [System.Serializable]
    public class WeaponLocations
    {
        public GameObject shotgun;
        public Transform shotgunLoc;
    }
    public GameObject hand;
    public WeaponLocations weaponLocations;
    private void Start()
    {
        ammoCounterScript = GetComponent<AmmoCounter>();
        coroutine = ShowWeaponName("Fist");
        StartCoroutine(coroutine);
    }
    private void Update()
    {
        if (Input.GetButtonDown("WeaponWheel"))
        {
            if(weaponWheelObj.activeSelf == false)
            {
                weaponWheelObj.SetActive(true);
                player.GetComponent<Movement>().enabled = !enabled;
                player.GetComponentInChildren<MouseLook>().enabled = !enabled;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                weaponWheelObj.SetActive(false);
                player.GetComponent<Movement>().enabled = enabled;
                player.GetComponentInChildren<MouseLook>().enabled = enabled;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
        if (Input.GetButtonDown("Use"))
        {
            if (shootScript.isReloading == false)
            {
                RaycastHit hit;
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, pickUpRange))
                {
                    if (hit.collider.gameObject.tag == "Weapon")
                    {
                        if (hit.collider.gameObject.GetComponent<GunScript>())
                        {
                            for (int i = 0; i < rings[0].transform.childCount; i++)
                            {
                                if (rings[0].transform.GetChild(i).GetComponent<Slot>().gunWeapon == null)
                                {
                                    rings[0].transform.GetChild(i).GetComponent<Slot>().gunWeapon = hit.collider.GetComponent<GunScript>().weapon;
                                    rings[0].transform.GetChild(i).GetComponent<Slot>().ammoInMag = hit.collider.GetComponent<GunScript>().ammoInMag;
                                    Destroy(hit.collider.gameObject);
                                    if (selectedSlotScript == null)
                                    {
                                        SelectSlot(rings[0].transform.GetChild(i).GetComponent<Slot>());
                                    }
                                    break;
                                }
                                else if (i + 1 == rings[0].transform.childCount)
                                {
                                    print("Switch " + selectedSlotScript.gunWeapon.name + " for " + hit.collider.GetComponent<GunScript>().weapon.name + " at slot " + selectedSlotScript.gameObject.name);
                                    Instantiate(selectedSlotScript.gunWeapon.weaponPrefab, dropLoc.transform.position, Quaternion.identity, dropLoc.transform);
                                    GameObject g = dropLoc.transform.GetChild(0).gameObject;
                                    g.GetComponent<GunScript>().ammoInMag = selectedSlotScript.ammoInMag;
                                    selectedSlotScript.gunWeapon = hit.collider.GetComponent<GunScript>().weapon;
                                    selectedSlotScript.ammoInMag = hit.collider.GetComponent<GunScript>().ammoInMag;
                                    g.transform.SetParent(null);
                                    Destroy(hit.collider.gameObject);
                                    break;
                                }
                            }
                        }
                        if (hit.collider.gameObject.GetComponent<MeleeScript>())
                        {
                            print("Pick up melee weapon");
                            for (int i = 0; i < rings[1].transform.childCount; i++)
                            {
                                if (rings[1].transform.GetChild(i).GetComponent<Slot>().meleeWeapon == null)
                                {
                                    rings[1].transform.GetChild(i).GetComponent<Slot>().meleeWeapon = hit.collider.GetComponent<MeleeScript>().weapon;
                                    rings[1].transform.GetChild(i).GetComponent<Slot>().ammoInMag = hit.collider.GetComponent<MeleeScript>().uses;
                                    Destroy(hit.collider.gameObject);
                                    if (selectedSlotScript == null)
                                    {
                                        SelectSlot(rings[1].transform.GetChild(i).GetComponent<Slot>());
                                    }
                                    break;
                                }
                                else if (i + 1 == rings[1].transform.childCount)
                                {
                                    print("Switch " + selectedSlotScript.meleeWeapon.name + " for " + hit.collider.GetComponent<MeleeScript>().weapon.name + " at slot " + selectedSlotScript.gameObject.name);
                                    Instantiate(selectedSlotScript.meleeWeapon.weaponPrefab, dropLoc.transform.position, Quaternion.identity, dropLoc.transform);
                                    GameObject g = dropLoc.transform.GetChild(0).gameObject;
                                    g.GetComponent<MeleeScript>().uses = selectedSlotScript.ammoInMag;
                                    selectedSlotScript.meleeWeapon = hit.collider.GetComponent<MeleeScript>().weapon;
                                    selectedSlotScript.ammoInMag = hit.collider.GetComponent<MeleeScript>().uses;
                                    g.transform.SetParent(null);
                                    Destroy(hit.collider.gameObject);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public void SelectSlot(Slot slotscript)
    {
        if (shootScript.isReloading == false)
        {
            shootScript.weapon = null;
            meleeScript.weapon = null;
            if (slotscript.gunWeapon != null)
            {
                selectedSlotScript = slotscript;
                weaponImage.sprite = slotscript.gunWeapon.uiSprite;

                print(slotscript.gunWeapon.weaponName);
                shootScript.weapon = slotscript.gunWeapon;
                shootScript.currentSlot = slotscript;

                if (coroutine == null)
                    StopCoroutine(coroutine);
                coroutine = ShowWeaponName(slotscript.gunWeapon.weaponName);
                StartCoroutine(coroutine);

                playerAnim.SetBool("Gun", true);

                ammoCounterScript.UpdateAmmo(slotscript.ammoInMag);
                if (slotscript.gunWeapon.gunType == "Pistol")
                {
                    ammoCounterScript.UpdatePistolAmmoLeft();
                }
                else if (slotscript.gunWeapon.gunType == "Sniper")
                {
                    ammoCounterScript.UpdateSniperAmmoLeft();
                }
                else if (slotscript.gunWeapon.gunType == "Shotgun")
                {
                    ammoCounterScript.UpdateShotgunAmmoLeft();
                    Instantiate(weaponLocations.shotgun, weaponLocations.shotgunLoc.transform.position, weaponLocations.shotgunLoc.rotation, hand.transform);
                }
                else if (slotscript.gunWeapon.gunType == "Launcher")
                {
                    ammoCounterScript.UpdateLauncherAmmoLeft();
                }
                else if (slotscript.gunWeapon.gunType == "Special")
                {
                    ammoCounterScript.UpdateSpecialAmmoLeft();
                }
            }
            else if (slotscript.meleeWeapon != null)
            {
                selectedSlotScript = slotscript;
                weaponImage.sprite = slotscript.meleeWeapon.uiSprite;
                
                print(slotscript.meleeWeapon.weaponName);
                meleeScript.currentSlot = slotscript;
                ammoCounterScript.UpdateMeleeAmmo(slotscript.ammoInMag);
                meleeScript.weapon = slotscript.meleeWeapon;
                if (coroutine == null)
                    StopCoroutine(coroutine);
                coroutine = ShowWeaponName(slotscript.meleeWeapon.weaponName);
                StartCoroutine(coroutine);
            }
        }
    }
    IEnumerator ShowWeaponName(string name)
    {
        nameText.text = name;
        yield return new WaitForSeconds(displayNameTime);
        nameText.text = "";
    }
}
