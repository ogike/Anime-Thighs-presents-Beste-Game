using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Shoots the given projectilePrefab-s at the player
 * Can be used as a template for other ranged enemy attack behaviour
 * Uses the EnemyHandler to get the direction/distance to the player
 *      Needs to be on the same GameObject with this
 * The direction to shoot in is handled as a rotation angle (the z value in the editor)
 *      Can be really easy to expand by adding/subtraction values from it, like addig:
 *          spread
 *          multi-directional shooting (either shotguns, or going around in a 360 angle)
 *          etc etc
 */

public class EnemyRangedBasic : MonoBehaviour
{
    public int   damage;
    public float cooldown;
    public int knockbackStrength;
    //public float distToAttack;
    public float projectileSpeed;

    public SoundClass shootSound;

    public Transform shootPosTrans;     //where the projectiles will spawn;
    public GameObject projectilePrefab; //the projectile to spawn

    EnemyHandler myHandler;
    Vector3 dirToTarget; //the normalized direction
    float distToTarget;  //the distance to the player

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
        //get the vector data from the enemy handler
        dirToTarget = myHandler.GetDirToPlayer();
        distToTarget = myHandler.GetDistToTarget();

        //myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }
        else// if (distToTarget < distToAttack)
        {
            Attack();
            curCooldown = cooldown;
        }
    }

    void Attack()
	{
        //this is where you would implement crazier stuff like multi-directional shots, etc etc

        float angleToTarget =  Mathf.Atan2(dirToTarget.x, dirToTarget.y) * Mathf.Rad2Deg * (-1);
        //turns the direction vector into a rotation angle with trigonometry (the z rotation in the editor)
        //majd átváltjuk fokká
        //nem tudom miért kell beszorozni (-1)-el???? csak így műkődik tho

        AudioManager.Instance.PlayFXSound(shootSound);

        ShootOnce(angleToTarget);
    }

    //called for every projectile
    void ShootOnce(float angleToTarget)
	{
        //az irányvektorok helyett inkább z tengelyes rotation-nal kezeljük a forgatást, ez a angleToTarget
        Quaternion shootRot = Quaternion.Euler(0, 0, angleToTarget); //turn the shootingDirectionAngle into a Quaternion, amit használ a unity is

        //shootPosTrans is the GameObject/Transform which stores where the bullets should spawn
        //its a children of the Player GameObject, so its position is relative to the parent
        Vector3 shootPos = shootPosTrans.position;

        //GameObject.Instantiate: spawns a GameObject basically
        GameObject curBullet = GameObject.Instantiate(projectilePrefab, shootPos, shootRot);

        //update the bullet stats
        BulletScript curBulletScript = curBullet.GetComponent<BulletScript>();
        curBulletScript.speed = projectileSpeed;
        curBulletScript.damage = damage;
        curBulletScript.knockbackStrength = knockbackStrength;
    }
}
