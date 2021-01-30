using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This should be put on every bullet prefab, enemy and player too
 * This is what handles damaging the enemy
 * Its stats are set by the script spawing it (WeaponScript/EnemyRanged)
 * Basztatnival� ember: ogike
 */
public class BulletScript : MonoBehaviour
{
    Transform myTrans;

    //these will be set by the WeaponScript, no need to do anything
    [HideInInspector] public float speed;
    [HideInInspector] public int   damage;
    public string targetTag = "Enemy";
    [HideInInspector] public int knockbackStrength;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //speed scaled by time so it wont be affected by fps
        float scaledSpeed = speed * Time.deltaTime;

        myTrans.Translate(Vector3.up * scaledSpeed);
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == targetTag)
        {
            Vector3 tempKnockbackDir = (other.transform.position - myTrans.position).normalized;
            Vector2 knockbackDir = new Vector2(tempKnockbackDir.x, tempKnockbackDir.y);
            HealthScript targetHealth = other.GetComponent<HealthScript>();
            //Debug.Log(targetTag + " has taken " + damage + " dmg");
            targetHealth.TakeDamage(damage, knockbackDir, knockbackStrength);
        }
        Destroy(gameObject);
    }
}
