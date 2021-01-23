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
    void Awake()
    {
        //curHealth = maxHealth;
        myRenderer = GetComponent<SpriteRenderer>();

        //a plaert a playerHandler-ben healeljük
        if(!isPlayer)
            HealToMax();
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
            UpdateHealthVisuals();

        }
	}

    public void HealToMax ()
	{
        if(!isDead)
		{
            curHealth = maxHealth;
		}
        if (isPlayer)
        {
            UpdateHealthVisuals();
        }
	}

    // handles health increase by value
    public void Heal(int healing)
    {
        if(!isDead)
        {
            curHealth += healing;
        }

        if(curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }

        UpdateHealthVisuals();
    }

    public void UpdateHealthVisuals ()
	{
        //placeholder, the less health the more red
        float newNonRedColor = (float)curHealth / maxHealth;
        //Debug.Log("new percent =" + newNonRedColor);
        myRenderer.color = new Color(1, newNonRedColor, newNonRedColor); //color-s values are between 0 and 1
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
