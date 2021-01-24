using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedBasic : MonoBehaviour
{
    public int   damage;
    public float cooldown;
    public float distToAttack;
    public float projectileSpeed;

    public Transform shootPosTrans; //where the projectiles will spawn;
    public GameObject projectilePrefab;

    EnemyHandler myHandler;
    Vector3 dirToTarget; //the normalized direction
    float distToTarget;

    float curCooldown;


    // Start is called before the first frame update
    void Start()
    {
        myHandler = GetComponent<EnemyHandler>();

        curCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (!myHandler.IsAwake()) //could optimize this, instead of calling another script every frame
        {
            //if asleep, dont do anything
            return;
        }

        //pass these variables as references to the method (like pointers)
        myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }
        else if (distToTarget < distToAttack)
        {
            Attack();
            curCooldown = cooldown;
        }
    }

    void Attack()
	{
        //this is where you would implement crazier stuff like multi-directional shots, etc etc�
        float angleToTarget =  Mathf.Atan2(dirToTarget.x, dirToTarget.y) * Mathf.Rad2Deg * (-1); //turns the direction vector into a rotation angle (the z rotation in the editor)
            //majd �tv�ltjuk fokk�
            //nem tudom mi�rt kell beszorozni (-1)-el???? csak �gy m�k�dik tho
        ShootOnce(angleToTarget);
    }

    void ShootOnce(float angleToTarget)
	{
        //az ir�nyvektorok helyett ink�bb z tengelyes rotation-nal kezelj�k a forgat�st, ez a angleToTarget
        Quaternion shootRot = Quaternion.Euler(0, 0, angleToTarget); //turn the shootingDirectionAngle into a Quaternion, amit haszn�l a unity iss

        //shootPosTrans is the GameObject/Transform which stores where the bullets should spawn
        //its a children of the Player GameObject, so its position is relative to the parent
        Vector3 shootPos = shootPosTrans.position;

        //GameObject.Instantiate: spawns a GameObject basically
        GameObject curBullet = GameObject.Instantiate(projectilePrefab, shootPos, shootRot);

        BulletScript curBulletScript = curBullet.GetComponent<BulletScript>();
        curBulletScript.speed = projectileSpeed;
        curBulletScript.damage = damage;
    }
}
