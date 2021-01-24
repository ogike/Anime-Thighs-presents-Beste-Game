using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //set by the playerHandler--------
    public float cooldownMultiplier;
    public float bulletSpeedMultiplier;
    public float damageMultiplier;
    //--------------------------------

    public List<ShootingScript> weapons; //all the possible weapons we can switch between if needed
                                         //should rename this later into a weaponScript later
                                         //these should be attached to different GameObjects that are the children of the Player GameObject
                                         //set manually

    //TODO
    //List<GameObject> weaponObjects; //this is set automatically
                                    //for disabling/enabling the actual gameObjects, for if they have Sprites too for example


    int curWeaponIndex;

    void Awake()
    {
        if(weapons.Count == 0)
		{
            Debug.LogError("No weapons added to the player!");
		}

		for (int i = 0; i < weapons.Count; i++)
		{
            weapons[i].enabled = false;
		}

        SwitchWeapons(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SwitchWeapons(int newWeaponIndex)
	{
        if(curWeaponIndex == newWeaponIndex)
		{
            Debug.Log("Switched to the same weapon");
            //idk what we should do here tbh
            //return;
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

    //for updating the current weapon's stats
    public void UpdateCurWeaponStats ()
	{
        weapons[curWeaponIndex].UpdateStats(cooldownMultiplier, bulletSpeedMultiplier, damageMultiplier);
    }
}
