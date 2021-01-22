using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
    public int   damage = 50;
    public float distToAttack = 1;
    public float attackRange = 1.25f;
    public float attackCooldown = 1;

    public Transform meleeRayPos;  //this is where the line-of-sight check will be start from for the melee attack
    public LayerMask attackLayers; //TODO: explain this

    EnemyHandler myHandler;
    Vector3 dirToTarget; //the normalized direction
  public float distToTarget;

  public float curCooldown;


    // Start is called before the first frame update
    void Start()
    {
        myHandler = GetComponent<EnemyHandler>();

        curCooldown = attackCooldown;
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
        myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }
        else if(distToTarget < distToAttack)
		{
            Attack();
            curCooldown = attackCooldown;
        }
    }

    void Attack()
	{
        //should replace this whole thing with triggers instead
        RaycastHit2D hit = Physics2D.Raycast(meleeRayPos.position, dirToTarget, attackRange, attackLayers);
        //Color color;
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Player")
            {
                hit.transform.GetComponent<HealthScript>().TakeDamage(damage);
            //    color = Color.yellow;
            }
            //else
            //    color = Color.green;
            //Debug.DrawLine(meleeRayPos.position, hit.point, color, attackCooldown);
        }
    }

	/*private void OnCollisionEnter2D(Collision2D collision)
	{
        //very placeholder, should be done with distance checks later!!!
		if(collision.gameObject.tag == "Player")
		{
            collision.gameObject.GetComponent<HealthScript>().TakeDamage(damage);
        }
	}*/
}
