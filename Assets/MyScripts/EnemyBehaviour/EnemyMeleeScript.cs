using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple Trigger-Zone based melee script
 * As long as the player is in the Trigger-Zone, the enemy will keep attacking every attackCooldown interval
 * Should be attached to an empty GameObject, which is the child of the enemy itself
 *      A Collider2d set as trigger should be attached to the same GameObject
 * Doesnt have to be added to an enemy tho, can be used for spikes/etc
 */

public class EnemyMeleeScript : MonoBehaviour
{
    public int   damage = 50;
    public float attackCooldown = 1;

    float curCooldown;
    bool isAttacking;

    HealthScript playerHealth;



    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameManagerScript.Instance.playerHealth;

        curCooldown = attackCooldown;
        isAttacking = false;
    }

    // Update is called once per frame
    // LateUpdate is the same, but its called after the normal Update()-s
    void LateUpdate()
    {
        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }
        else if(isAttacking) //if the player is in the TriggerZone
		{
            Attack();
            curCooldown = attackCooldown;
        }
    }

    void Attack()
	{
        playerHealth.TakeDamage(damage);
    }

    //the player enters the trigger
	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
            isAttacking = true;
        }
	}

    //the player leaves the trigger
	private void OnTriggerExit2D(Collider2D collider)
	{
        if (collider.gameObject.tag == "Player") //double checking, if the collision matrix is set properly it shouldnt be necessary
        {
            isAttacking = false;
        }
    }
}
