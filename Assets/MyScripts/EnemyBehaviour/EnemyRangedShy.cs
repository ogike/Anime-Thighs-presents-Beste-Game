using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedShy : MonoBehaviour
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
