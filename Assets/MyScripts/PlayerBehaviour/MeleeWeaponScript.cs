using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This is just to showcase how you can add different Weapon types to Marci
 * This is where you would write the code for melee weapons
 * 
 */
public class MeleeWeaponScript : MonoBehaviour
{

    enum Position //Amelyik oldalon van a kard, azzal ellenkezõ irányba kell swingelni
    {
        Left,
        Right
    }

    bool IsAttacking;
    Position myPos;
    public int damage = 200;
    public int knockbackStrength = 15;
    public float swingCD = 0.3f;
    public float curCD;
    public float swingSpeed = 200;

    Transform myTrans;
    // Start is called before the first frame update
    void Start()
    {
        myPos = Position.Right;
        curCD = 0;
        myTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        if (curCD > 0)
        {
            curCD -= Time.deltaTime;
            if (myPos == Position.Right)
            {
                transform.RotateAround(transform.parent.position, Vector3.forward, swingSpeed * -1 * Time.deltaTime);
                
                //myTrans.rotation = transform.parent.rotation * Quaternion.Euler(0, 0, 180);

                
            }
            else
            {
                transform.RotateAround(transform.parent.position, Vector3.forward, swingSpeed * Time.deltaTime);

                //myTrans.rotation = transform.parent.rotation * Quaternion.Euler(0, 0, 180);


            }
        }
        if (!(curCD > 0) && Input.GetMouseButton(0))
        {
            Attack();

        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Vector3 tempKnockbackDir = (other.transform.position - transform.parent.position).normalized; //the direction of the knockback normalized
            Vector2 knockbackDir = new Vector2(tempKnockbackDir.x, tempKnockbackDir.y);          //turning it into a 2D Vector
            HealthScript targetHealth = other.GetComponent<HealthScript>();
            //Debug.Log(targetTag + " has taken " + damage + " dmg");
            targetHealth.TakeDamage(damage, knockbackDir, knockbackStrength);
        }
    }
            public void UpdateStats(float cooldownMultiplier, float damageMultiplier)
	{
        //update your stats here
	}

    void Attack()
    {
        curCD = swingCD;
        if (myPos == Position.Right)
            myPos = Position.Left;
        else
            myPos = Position.Right;
    }

}
