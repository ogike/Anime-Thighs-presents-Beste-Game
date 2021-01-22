using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public int  maxHealth = 100;
    public bool isPlayer = false;

    public int curHealth; //only public for debugging
    bool isDead = false;

    SpriteRenderer myRenderer; //for temp health display

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage (int dmg)
	{
        curHealth -= dmg;

        if(curHealth <= 0 && !isDead)
		{
            Die();
		}
        else if (isPlayer && !isDead)
		{
            //placeholder, the less health the more red
            float newNonRedColor = (float)curHealth / maxHealth;
            myRenderer.color = new Color(1, newNonRedColor, newNonRedColor); //color-s values are between 0 and 1
		}
	}

    void Die()
	{
        if (isPlayer)
        {
            //special GameOver stuff
            Debug.Log("You died. bruh");
            //Time.timeScale = 0; //stop time
            GetComponent<PlayerController>().enabled = false; //disable player controller, cos we fucking dead
            GetComponent<ShootingScript>().enabled   = false;
            myRenderer.color = Color.red;
            isDead = true;
        }
        else //if enemy
        {
            RoomsManager.Instance.ChangeActiveEnemiesToCount(-1); //decreasing the current enemiesInThisRoom

            //gameObject: the GameObject this component is linked to
            //            is built in, doesnt need to be retrieved with GetComponent<>()
            //Destroy (Gameobject): deletes the GameObject from the scene
            Destroy(gameObject);
        }
	}
}
