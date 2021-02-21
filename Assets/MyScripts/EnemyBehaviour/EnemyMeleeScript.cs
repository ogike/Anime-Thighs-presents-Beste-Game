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
    public int knockbackStrength = 0;

    public SoundClass meleeSound;
    public GameObject targetHitPrefab;

    float curCooldown;
    bool isAttacking;

    HealthScript playerHealth;

    Transform playerTrans;
    Transform myTrans;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameManagerScript.Instance.playerHealth;
        playerTrans = GameManagerScript.Instance.playerTransform;
        myTrans = GetComponent<Transform>();

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
        Vector3 tempKnockbackDir = (playerTrans.position - myTrans.position).normalized; //calculating the direction between us and the enemy, and using it for knockback
        Vector2 knockbackDir = new Vector2(tempKnockbackDir.x, tempKnockbackDir.y);

        playerHealth.TakeDamage(damage, knockbackDir, knockbackStrength);

        AudioManager.Instance.PlayFXSound(meleeSound);
        ParticleManager.Instance.PlayParticleEffect(targetHitPrefab, playerTrans.position);
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
