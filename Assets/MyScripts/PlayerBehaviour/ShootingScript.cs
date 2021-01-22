using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public Transform shootPosTrans; //Have to set this in the scene
    public GameObject bulletPrefab;

    public bool  isAuto      = true; //is this weapon automatic-firing?
    public float cooldown    = 0.5f; //should be 0 if its not auto
    public float bulletSpeed = 10;
    public int   damage      = 34;

    public float curCooldown; //stores how much time there is until we can shoot again

    // Start is called before the first frame update
    void Start()
    {
        curCooldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }
        else
        {
            //this is ugly lol, but readable
            //left mouse button is held down
            if (isAuto && Input.GetMouseButton(0))
            {
                ShootOnce();
                curCooldown = cooldown; //resetting the cooldown
            }
            //left mouse button was pressed, this is only called once per click or per mouse-being-held-down
            else if (!isAuto && Input.GetMouseButtonDown(0))
            {
                ShootOnce();
                curCooldown = cooldown; //resetting the cooldown
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
        curBulletScript.speed  = bulletSpeed;
        curBulletScript.damage = damage;
    }
}
