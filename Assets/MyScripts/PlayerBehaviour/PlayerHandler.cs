using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//should make this a header file or whatever, for readability
public enum SkillStatName
{
    BaseDamageMultiplier,
    MoveSpeed,
    MaxHealth,
    ShootingCooldown,
    BulletSpeed
}

[System.Serializable] //editor is meg tudja jeleníteni
//ezt a class-t használjuk minden Player Stat-hoz, amit bárhonann növelni/változtatni akarunk
public class SkillStat
{
    public SkillStatName name;

    public float originalValue = 1;
    public float curStatMultiplier = 1;
    public float curStatAdder = 0; //ha szorzás helyett hozzá akarunk adni valamennyit az értékhez

    //public float curPowerUpModifier = 1;
    //public float curPowerupAdder = 0;

    float curValue;

    public void ModifyStatValues(float modifier, float adder)
	{
        curStatMultiplier += modifier;
        curStatAdder      += adder;
	}

    public float GetCurValue ()
	{
        //curValue = (originalValue * curStatMultiplier * curPowerUpModifier) + curStatAdder + curPowerupAdder;
        curValue = (originalValue * curStatMultiplier) + curStatAdder;

        return curValue;
	}
}


[System.Serializable]
public class SkillStatBoost
{
    public SkillStatName name;

    public float multiplierDifference;
    public float valueDifference;
}

//the actual script-----------------------
public class PlayerHandler : MonoBehaviour
{
    public List<SkillStat> skillStats;
    //the list of different stats the player has
    //IMPORTANT!! cantt have duplicates!!

    //ShootingScript myShooterScript;
    WeaponManager myWeaponManager;
    HealthScript myHealthScript;
    PlayerController myControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        //getting all the needed components, that we want to change/modify later
        myWeaponManager = GetComponent<WeaponManager>();
        myHealthScript = GetComponent<HealthScript>();
        myControllerScript = GetComponent<PlayerController>();

		for (int i = 0; i < skillStats.Count; i++)
		{
            ApplyStatChange(skillStats[i].name);
		}

        //setting the starting health for the player
        myHealthScript.HealToMax();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ezt hívjuk meg amikor változtatni akarjuk a statokat
    public void ApplyStatBoost (SkillStatBoost statBoost)
	{
        SkillStat curStat = skillStats.Find(x => x.name == statBoost.name);

        curStat.ModifyStatValues(statBoost.multiplierDifference, statBoost.valueDifference);

        ApplyStatChange(statBoost.name);
	}

    public void ApplyStatChange(SkillStatName skillName)
	{
        SkillStat curStat = skillStats.Find(x => x.name == skillName);

        float newValue = curStat.GetCurValue();

        switch (skillName)
		{
            case SkillStatName.MaxHealth:
                myHealthScript.maxHealth = (int)newValue;
                myHealthScript.UpdateHealthVisuals(); //ha megváltozik a maxHealth, a visual representation is más lesz
                break;

            case SkillStatName.MoveSpeed:
                myControllerScript.moveSpeed = newValue;
                break;

            case SkillStatName.BaseDamageMultiplier:
                myWeaponManager.damageMultiplier = newValue;
                myWeaponManager.UpdateCurWeaponStats();
                break;

            case SkillStatName.ShootingCooldown:
                myWeaponManager.cooldownMultiplier = newValue;
                myWeaponManager.UpdateCurWeaponStats();
                break;

            case SkillStatName.BulletSpeed:
                myWeaponManager.bulletSpeedMultiplier = newValue;
                myWeaponManager.UpdateCurWeaponStats();
                break;

            default:
                Debug.LogError("Thats fucked up lol");
                break;
        }
	}

    //Ez a currentHealth-et változtatja, nem a maxHealth-et! tehát csak egy healthPickup ezt hívja meg
    public void BoostHealth (int plusHealth)
	{
        myHealthScript.Heal(plusHealth);
    }
}
