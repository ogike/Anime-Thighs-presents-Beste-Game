using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Credit to Meem
 * Basically the EnemyRanged expanded (read that code for more explanations)
 *      added spread
 *      the closer the player is, the more inaccurate it becomes
 */

public class EnemyRangedShy : MonoBehaviour
{
    public int damage;
    public float cooldown;
    public float distToAttack;
    public float projectileSpeed;
    public float spreadAngle; //the degree of spread

    float spreadOverDistance;
    float randomizedSpread;

    public Transform shootPosTrans; //where the projectiles will spawn;
    public GameObject projectilePrefab;

    EnemyHandler myHandler;
    Vector3 dirToTarget; //the normalized direction to the player
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
        //pass these variables as references to the method (like pointers) to update them
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
        float dirAngle = Mathf.Atan2(dirToTarget.x, dirToTarget.y) * Mathf.Rad2Deg * (-1);
            //turns the direction vector into a rotation angle with trigonometry (the z rotation in the editor)
            //majd átváltjuk fokká
            //nem tudom miért kell beszorozni (-1)-el???? csak így mûködik tho

        spreadOverDistance = (spreadAngle / distToTarget);  //calculate the spread with distance??
        spreadOverDistance *= spreadOverDistance;           //négyzeteljök az értéket (ask meem why)
        randomizedSpread = Random.Range(-1 * spreadOverDistance, spreadOverDistance); //random spread value

        dirAngle += randomizedSpread; //applying spread to the direction angle
        ShootOnce(dirAngle);
    }

    void ShootOnce(float dirAngle)
    {
        //az irányvektorok helyett inkább z tengelyes rotation-nal kezeljük a forgatást, ez a dirAngle
        Quaternion shootRot = Quaternion.Euler(0, 0, dirAngle); //turn the shootingDirectionAngle into a Quaternion, amit használ a unity iss

        //shootPosTrans is the GameObject/Transform which stores where the bullets should spawn
        //its a children of the Player GameObject, so its position is relative to the parent
        Vector3 shootPos = shootPosTrans.position;

        //GameObject.Instantiate: spawns a GameObject basically
        GameObject curBullet = GameObject.Instantiate(projectilePrefab, shootPos, shootRot);

        //update the spawned bullet's stats
        BulletScript curBulletScript = curBullet.GetComponent<BulletScript>();
        curBulletScript.speed = projectileSpeed;
        curBulletScript.damage = damage;
    }
}
