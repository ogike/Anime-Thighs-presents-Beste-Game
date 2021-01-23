using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedShy : MonoBehaviour
{
    public int damage;
    public float cooldown;
    public float distToAttack;
    public float projectileSpeed;
    public float spreadAngle;
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

    /*
    float RandomSpread(float min, float max)
    {
        Random rnd = new Random();
        return (float)rnd.NextDouble(min, max);
    }
    */

    void Attack()
    {
        spreadOverDistance = (spreadAngle / distToTarget);
        spreadOverDistance *= spreadOverDistance;
        randomizedSpread = Random.Range(-1 * spreadOverDistance, spreadOverDistance);
        Vector3 spreadDir = new Vector3(dirToTarget.x + randomizedSpread, dirToTarget.y + randomizedSpread, 0);
        ShootOnce(spreadDir);
    }

    void ShootOnce(Vector3 dir)
    {
        //shootPosTrans is the GameObject/Transform which stores where the bullets should spawn
        //its a children of the Player GameObject, so its position is relative to the parent
        Quaternion shootRot = Quaternion.LookRotation(dir, Vector3.forward); //TODO: make this use dir somehow??
        Vector3 shootPos = shootPosTrans.position;

        //GameObject.Instantiate: spawns a GameObject basically
        GameObject curBullet = GameObject.Instantiate(projectilePrefab, shootPos, shootRot);

        BulletScript curBulletScript = curBullet.GetComponent<BulletScript>();
        curBulletScript.speed = projectileSpeed;
        curBulletScript.damage = damage;
    }
}
