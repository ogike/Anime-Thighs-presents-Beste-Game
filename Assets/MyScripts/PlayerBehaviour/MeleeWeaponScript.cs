using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* This is just to showcase how you can add different Weapon types to Marci
 * This is where you would write the code for melee weapons
 * 
 */
public class MeleeWeaponScript : MonoBehaviour
{

    
    public int damage = 200;
    public int knockbackStrength = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
