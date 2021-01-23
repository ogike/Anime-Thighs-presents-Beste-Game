using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//should make this a header file or whatever, for readability
public enum SkillStatName
{
    Damage,
    MoveSpeed,
    MaxHealth
}

[System.Serializable] //editor is meg tudja jelen�teni
//ezt a class-t haszn�ljuk minden Player Stat-hoz, amit b�rhonann n�velni/v�ltoztatni akarunk
public class SkillStat
{
    public SkillStatName name;

    public float originalValue = 1;
    public float curStatMultiplier = 1;
    public float curStatAdder = 0; //ha szorz�s helyett hozz� akarunk adni valamennyit az �rt�khez

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
        //IMPORTANT!! cantt have duplicates!!

    ShootingScript myShooterScript;
    HealthScript myHealthScript;
    PlayerController myControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        //getting all the needed components, that we want to change/modify later
        myShooterScript = GetComponent<ShootingScript>();
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

    //ezt h�vjuk meg amikor v�ltoztatni akarjuk a statokat
    public void ApplyStatBoost (SkillStatBoost statBoost)
	{
        SkillStat curStat = skillStats.Find(x => x.name == statBoost.name);

        curStat.ModifyStatValues(statBoost.multiplierDifference, statBoost.valueDifference);

        ApplyStatChange(statBoost.name);

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
            case SkillStatName.Damage:
                myShooterScript.damage = (int)newValue;
                break;
            case SkillStatName.MaxHealth:
                myHealthScript.maxHealth = (int)newValue;
                myHealthScript.UpdateHealthVisuals(); //ha megv�ltozik a maxHealth, a visual representation is m�s lesz
                break;
            case SkillStatName.MoveSpeed:
                myControllerScript.moveSpeed = newValue;
                break;
            default:
                Debug.LogError("Thats fucked up lol");
                break;
        }
	}

    //Ez a currentHealth-et v�ltoztatja, nem a maxHealth-et! teh�t csak egy healthPickup ezt h�vja meg
    public void BoostHealth (int plusHealth)
	{
        myHealthScript.Heal(plusHealth);
    }

    IEnumerator ResetAfterTime(float time, SkillStat curStat, SkillStatBoost statBoost)
    {
        yield return new WaitForSeconds(time);
        curStat.ModifyStatValues(statBoost.multiplierDifference*(-1), statBoost.valueDifference*(-1));
        ApplyStatChange(statBoost.name);
    }
}
