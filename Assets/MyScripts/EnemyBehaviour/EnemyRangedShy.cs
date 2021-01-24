using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        spreadOverDistance = (spreadAngle / distToTarget);
        spreadOverDistance *= spreadOverDistance;
        randomizedSpread = Random.Range(-1 * spreadOverDistance, spreadOverDistance);
        float dirAngle = Mathf.Atan2(dirToTarget.x, dirToTarget.y) * Mathf.Rad2Deg * (-1); //turns the direction vector into a rotation angle (the z rotation in the editor)
            //majd átváltjuk fokká
            //nem tudom miért kell beszorozni (-1)-el???? csak így mûködik tho
        dirAngle += randomizedSpread; //applying spread to the angle
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

        BulletScript curBulletScript = curBullet.GetComponent<BulletScript>();
        curBulletScript.speed = projectileSpeed;
        curBulletScript.damage = damage;
    }
}
