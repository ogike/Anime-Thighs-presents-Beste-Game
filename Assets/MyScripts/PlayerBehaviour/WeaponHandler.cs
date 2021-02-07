using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script acts like the communicator between the weaponManager and the different types of weaponScripts
 * So if you wanted to make a weapon that doesnt work like a normal projectile-based gun:
 *      -you make a seperate script for it
 *      -then add it as an option here (as an enum type and in the switch case)
 *          -handle the updating of the SkillStats for the weapon here
 * Ez az egész biztos át lesz írva egyszer, Interface-ekkel vagy Inheritance-el, mert nagyon nem szép így, big S P A G O O T  times, de oh well
 *      vagy lehet mégis beleteszem ezt is a playerManagerbe
 *      Vagy lehet teljesen scrapp-eljük egyenlõre ezt az ötletet hogy több fajta fegyvertípus lehessen, amúgy is csak össze dobtam ezt szarul
 */

//this basically stores the different types of Scripts a weapon can be
public enum WeaponTypeName
{
    BasicShooting,
    BasicMelee
    //this is where you would put sth like "Melee", and whatever you want
}

public class WeaponHandler : MonoBehaviour
{
    public WeaponTypeName myType;

    WeaponScript myWeaponScript;
    MeleeWeaponScript myMeleeScript;

    // Start is called before the first frame update
    void Awake()
    {
        switch (myType)
        {
            case WeaponTypeName.BasicShooting:
                myWeaponScript = GetComponent<WeaponScript>();
                break;
            case WeaponTypeName.BasicMelee:
                myMeleeScript = GetComponent<MeleeWeaponScript>();
                break;
            default:
                Debug.LogError("Nem adtad meg a fegyver típusát!");
                break;
        }
    }

    //this is what gets called from the weaponManager
    //if there atr more stats (for example, melee specifics), you pass all of them as parameters here too
    //then the function decides which ones to send where, based on the weaponType
    public void UpdateMyWeaponsStats(float cooldownMultiplier, float bulletSpeedModifier, float damageMultiplier)
    {
        switch (myType)
        {
            case WeaponTypeName.BasicShooting:
                myWeaponScript.UpdateStats(cooldownMultiplier, bulletSpeedModifier, damageMultiplier);
                break;
            case WeaponTypeName.BasicMelee:
                myMeleeScript.UpdateStats(cooldownMultiplier, damageMultiplier);
                break;
            default:
                Debug.LogError("what");
                break;
        }
    }
}
