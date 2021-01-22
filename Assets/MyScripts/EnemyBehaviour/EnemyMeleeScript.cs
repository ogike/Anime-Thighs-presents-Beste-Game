using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
    public int   damage = 50;
    public float attackCooldown = 1;

    EnemyHandler myHandler;
    Vector3 dirToTarget; //the normalized direction

  public float curCooldown; //public for debug
    public bool isAttacking;

    HealthScript playerHealth;



    // Start is called before the first frame update
    void Start()
    {
        myHandler = transform.parent.GetComponent<EnemyHandler>(); //nem a legszebb megoldás, once again
        playerHealth = GameManagerScript.Instance.playerHealth;

        curCooldown = attackCooldown;
        isAttacking = false;
    }

    // Update is called once per frame
    // LateUpdate is the same, but its called after the normal Update()-s, which means it will be after the EnemyScripts's Update, and that all the variables will be up-to-date
    void LateUpdate()
    {
        if (!myHandler.IsAwake()) //could optimize this, instead of calling another script every frame
        {
            //if asleep, dont do anything
            return;
        }

        //pass these variables as references to the method (like pointers)

        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }
        else if(isAttacking)
		{
            Attack();
            curCooldown = attackCooldown;
        }
    }

    void Attack()
	{
        playerHealth.TakeDamage(damage);
        
    }

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "Player")
		{
            isAttacking = true;
        }
	}

	private void OnTriggerExit2D(Collider2D collider)
	{
        if (collider.gameObject.tag == "Player") //double checking, if the collision matrix is set properly it shouldnt be necessary
        {
            isAttacking = false;
        }
    }
}
