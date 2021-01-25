using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//should make this a header file or whatever, for readability
public enum SkillStatName
{
    BaseDamageMultiplier,
    MoveSpeed,
    MaxHealth,
    ShootingCooldownMultiplier,
    BulletSpeedMultiplier
}

[System.Serializable] //editor is meg tudja jelen�teni
//ezt a class-t haszáljuk minden Player Stat-hoz, amit bárhonann növelni/változtatni akarunk
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
    public bool isPowerup;
    public float duration;
}

//the actual script-----------------------
public class PlayerHandler : MonoBehaviour
{
    public List<SkillStat> skillStats;
        //the list of different stats the player has
        //IMPORTANT!! cant have duplicates!!

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

        //applying all the stats
        //note: this overwrites these bitches so far: movementSpeed, maxHealth, and the weapon stat multipliers
		for (int i = 0; i < skillStats.Count; i++)
		{
            ApplyStatChange(skillStats[i].name);
		}

        //setting the starting health for the player, after setting the maxHealthStat
        myHealthScript.HealToMax();
    }


    //ezt hívjuk meg amikor változtatni akarjuk a statokat
    public void ApplyStatBoost (SkillStatBoost statBoost)
	{
        SkillStat curStat = skillStats.Find(x => x.name == statBoost.name); //megkeressük a változtatnivaló stat-ot a listában

        curStat.ModifyStatValues(statBoost.multiplierDifference, statBoost.valueDifference);
            //modify-oljuk ezt a statot
            //(magában a SkillStat class-ban van ez a függvény)

        ApplyStatChange(statBoost.name);

        //ha powerup, akkor a powerupDuration után reverse-elni akarjuk
        if(statBoost.isPowerup)
        {
            StartCoroutine(ResetAfterTime(statBoost.duration, curStat, statBoost));
        }
	}

    public void ApplyStatChange(SkillStatName skillName)
	{
        SkillStat curStat = skillStats.Find(x => x.name == skillName);

        float newValue = curStat.GetCurValue();

        switch (skillName)
		{
            case SkillStatName.MaxHealth:
                myHealthScript.maxHealth = (int)newValue;
                myHealthScript.UpdateHealthVisuals(); //ha megv�ltozik a maxHealth, a visual representation is m�s lesz
                break;

            case SkillStatName.MoveSpeed:
                myControllerScript.moveSpeed = newValue;
                break;

            case SkillStatName.BaseDamageMultiplier:
                myWeaponManager.damageMultiplier = newValue;
                myWeaponManager.UpdateCurWeaponStats();
                break;

            case SkillStatName.ShootingCooldownMultiplier:
                myWeaponManager.cooldownMultiplier = newValue;
                myWeaponManager.UpdateCurWeaponStats();
                break;

            case SkillStatName.BulletSpeedMultiplier:
                myWeaponManager.bulletSpeedMultiplier = newValue;
                myWeaponManager.UpdateCurWeaponStats();
                break;

            default:
                Debug.LogError("You probably havent set up your new SkillStat here properly");
                break;
        }
	}

    //Ez a currentHealth-et változtatja, nem a maxHealth-et! tehát egy healthPickup ezt hívja meg
    public void HealPlayer (int plusHealth)
	{
        myHealthScript.Heal(plusHealth);
    }

    //for deactivating powerups
    IEnumerator ResetAfterTime(float time, SkillStat curStat, SkillStatBoost statBoost)
    {
        yield return new WaitForSeconds(time); //csak a "time" után folytatódik ez a függvény

        curStat.ModifyStatValues(statBoost.multiplierDifference*(-1), statBoost.valueDifference*(-1));
        ApplyStatChange(statBoost.name);
    }
}
