using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Transform shootPosTrans; //Have to set this in the scene
    public GameObject bulletPrefab;

    //these are the base stats of the weapon-------------------------------
    public bool  isAuto      = true; //is this weapon automatic-firing?
    public float cooldown    = 0.5f; //should be 0 if its not auto
                                     //basically how much time until the weapon can fire again
    public float bulletSpeed = 10;
    public int   damage      = 34;
    //---------------------------------------------------------------------

    //these are the current stats that we should use-----------------------
    //after multiplying them with the different statBoosts
    float curMaxCooldown;
    float curBulletSpeed;
    int curDamage;
    //---------------------------------------------------------------------

    public float curRemainingCooldown; //stores how much time there is until we can shoot again

    // Start is called before the first frame update
    void Start()
    {
        curRemainingCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (curRemainingCooldown > 0)
        {
            curRemainingCooldown -= Time.deltaTime;
        }
        else
        {
            //this is ugly lol, but readable
            //left mouse button is held down
            if (isAuto && Input.GetMouseButton(0))
            {
                ShootOnce();
                curRemainingCooldown = curMaxCooldown; //resetting the cooldown
            }
            //left mouse button was pressed, this is only called once per click or per mouse-being-held-down
            else if (!isAuto && Input.GetMouseButtonDown(0))
            {
                ShootOnce();
                curRemainingCooldown = curMaxCooldown; //resetting the cooldown
            }
        }
    }

    void ShootOnce ()
	{
        //shootPosTrans is the GameObject/Transform which stores where the bullets should spawn
        //its a children of the Player GameObject, so its position is relative to the parent
        Quaternion shootRot = shootPosTrans.rotation;
        Vector3 shootPos = shootPosTrans.position;

        //GameObject.Instantiate: spawns a GameObject basically
        GameObject curBullet = GameObject.Instantiate(bulletPrefab, shootPos, shootRot);

        BulletScript curBulletScript = curBullet.GetComponent<BulletScript>();
        curBulletScript.speed  = curBulletSpeed;
        curBulletScript.damage = curDamage;
    }

    public void UpdateStats (float cooldownMultiplier, float bulletSpeedModifier, float damageMultiplier)
	{
        curMaxCooldown = cooldown    * cooldownMultiplier;
        curBulletSpeed = bulletSpeed * bulletSpeedModifier;
        curDamage = (int)(damage * damageMultiplier);
	}
}
