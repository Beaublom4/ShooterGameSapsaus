using UnityEditor.Animations;
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
    AmmoCounter ammoCounterScript;
    public Image weaponImage;

    IEnumerator coroutine;
    public TextMeshProUGUI nameText;
    public float displayNameTime;

    [System.Serializable]
    public class Colors
    {
        public ColorBlock selectedColor, normalColor;
    }
    public Colors colors;
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
                        if (hit.collider.gameObject.GetComponent<WeaponScript>().weapon.type == "Gun")
                        {
                            for (int i = 0; i < rings[0].transform.childCount; i++)
                            {
                                if (rings[0].transform.GetChild(i).GetComponent<Slot>().weapon == null)
                                {
                                    rings[0].transform.GetChild(i).GetComponent<Slot>().weapon = hit.collider.GetComponent<WeaponScript>().weapon;
                                    rings[0].transform.GetChild(i).GetComponent<Slot>().ammoInMag = hit.collider.GetComponent<WeaponScript>().ammoInMag;
                                    Destroy(hit.collider.gameObject);
                                    if (selectedSlotScript == null)
                                    {
                                        SelectSlot(rings[0].transform.GetChild(i).GetComponent<Slot>());
                                    }
                                    break;
                                }
                                else if (i + 1 == rings[0].transform.childCount)
                                {
                                    print("Switch " + selectedSlotScript.weapon.name + " for " + hit.collider.GetComponent<WeaponScript>().weapon.name + " at slot " + selectedSlotScript.gameObject.name);
                                    Instantiate(selectedSlotScript.weapon.weaponPrefab, dropLoc.transform.position, Quaternion.identity, dropLoc.transform);
                                    GameObject g = dropLoc.transform.GetChild(0).gameObject;
                                    g.GetComponent<WeaponScript>().ammoInMag = selectedSlotScript.ammoInMag;
                                    selectedSlotScript.weapon = hit.collider.GetComponent<WeaponScript>().weapon;
                                    selectedSlotScript.ammoInMag = hit.collider.GetComponent<WeaponScript>().ammoInMag;
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
            if (slotscript.weapon != null)
            {
                selectedSlotScript = slotscript;
                weaponImage.sprite = slotscript.weapon.uiSprite;

                if (coroutine == null)
                    StopCoroutine(coroutine);
                coroutine = ShowWeaponName(slotscript.weapon.weaponName);
                StartCoroutine(coroutine);

                if (slotscript.weapon.type == "Gun")
                {
                    print(slotscript.weapon);
                    shootScript.weapon = slotscript.weapon;
                    shootScript.currentSlot = slotscript;

                    ammoCounterScript.UpdateAmmo(slotscript.ammoInMag);
                    if (slotscript.weapon.gunType == "Pistol")
                    {
                        ammoCounterScript.UpdatePistolAmmoLeft();
                    }
                }
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
