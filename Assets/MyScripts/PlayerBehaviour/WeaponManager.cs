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

    public List<GameObject> weaponObjects; //this is set manually, just drag and drop
                                           //all the possible weapons we can switch between if needed
                                           //for disabling/enabling the actual gameObjects, for if they have Sprites too for example

    List<WeaponHandler> weaponHandlers; //the weaponHandler is whats needed to update the stats, so every type can be managed inside of taht
                                        //these should be attached to different GameObjects that are the children of the Player GameObject
                                        //set automatically

    public SoundClass weaponSwitchSound;

    int curWeaponIndex;

    void Awake()
    {
        weaponHandlers = new List<WeaponHandler>(); //initializing the list

        if (weaponObjects.Count == 0)
		{
            Debug.LogError("No weapons added to the WeaponManager!");
		}

        //disabling all the weapons, and getting all the weaponHandlers
		for (int i = 0; i < weaponObjects.Count; i++)
		{
            weaponHandlers.Add(weaponObjects[i].GetComponent<WeaponHandler>());
            weaponObjects[i].SetActive(false);
		}

        //setting up the first weapon----------------------------------
        curWeaponIndex = 0;
        weaponObjects[curWeaponIndex].SetActive(true); //enabling the weapon
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

        if(newWeaponIndex >= weaponObjects.Count)
		{
            Debug.LogError("Olyan fegyverre akarsz v�ltani, ami nincs be�ll�tva a WeaponManagerben");
            return;
		}

        weaponObjects[curWeaponIndex].SetActive(false); //disabling the current weapon

        curWeaponIndex = newWeaponIndex;
        weaponObjects[curWeaponIndex].SetActive(true); //enabling the new weapon

        UpdateCurWeaponStats(); //if the weapon stats changed since last time the weapon was active, we should update them

        AudioManager.Instance.PlayFXSound(weaponSwitchSound);
    }

    //when we die, the HealthScript calls this function
    //and also the GameManager when pausing
    public void DisableCurWeapon()
	{
        weaponObjects[curWeaponIndex].SetActive(false);
    }

    //GameManager calls this when we resume the game
    public void EnableCurWeapon()
	{
        weaponObjects[curWeaponIndex].SetActive(true);
    }

    //for updating the current weapon's stats
    public void UpdateCurWeaponStats ()
	{
        weaponHandlers[curWeaponIndex].UpdateMyWeaponsStats(cooldownMultiplier, bulletSpeedMultiplier, damageMultiplier);
    }
}
