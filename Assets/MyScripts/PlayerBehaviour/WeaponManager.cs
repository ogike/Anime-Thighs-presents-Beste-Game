using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This handles the switching between different weapons, and also the updating of the different weapon stats (from the PlayerHandler)
 * Has to be on the player, and the different weapons should be empty GameObjects (children of the player)
 * The first (0-th indexed) weapon is the default/starting weapon
 * Weapon pickup zones should communicate with this script
 */

public class WeaponManager : MonoBehaviour
{
    //set by the playerHandler--------
    [HideInInspector] public float cooldownMultiplier;
    [HideInInspector] public float bulletSpeedMultiplier;
    [HideInInspector] public float damageMultiplier;
    //--------------------------------

    public List<WeaponScript> weapons; //all the possible weapons we can switch between if needed
                                         //should rename this later into a weaponScript later
                                         //these should be attached to different GameObjects that are the children of the Player GameObject
                                         //set manually

    //TODO:
    //List<GameObject> weaponObjects; //this is set automatically
                                      //for disabling/enabling the actual gameObjects, for if they have Sprites too for example


    int curWeaponIndex;

    void Awake()
    {
        if(weapons.Count == 0)
		{
            Debug.LogError("No weapons added to the WeaponManager!");
		}

        //disabling all the weapons other than the first
            //the 0th weapon is the deafult starting weapon!
		for (int i = 1; i < weapons.Count; i++)
		{
            weapons[i].enabled = false;
		}

        //setting up the first weapon----------------------------------
        curWeaponIndex = 0;
        weapons[curWeaponIndex].enabled = true; //enabling the weapon
        UpdateCurWeaponStats(); //setting the base stats for the weapon
    }
    
    public void SwitchWeapons(int newWeaponIndex)
	{
        if(curWeaponIndex == newWeaponIndex)
		{
            Debug.Log("Switched to the same weapon");
            //idk what we should do here tbh
            return;
		}

        if(newWeaponIndex >= weapons.Count)
		{
            Debug.LogError("Olyan fegyverre akarsz váltani, ami nincs beállítva a WeaponManagerben");
            return;
		}

        weapons[curWeaponIndex].enabled = false; //disabling the current weapon

        curWeaponIndex = newWeaponIndex;
        weapons[curWeaponIndex].enabled = true; //enabling the new weapon

        UpdateCurWeaponStats(); //if the weapon stats changed since last time the weapon was active, we should update them
    }

    //when we die, the HealthScript calls this function
    public void DisableCurWeapon()
	{
        weapons[curWeaponIndex].enabled = false;
	}

    //for updating the current weapon's stats
    public void UpdateCurWeaponStats ()
	{
        weapons[curWeaponIndex].UpdateStats(cooldownMultiplier, bulletSpeedMultiplier, damageMultiplier);
    }
}
