using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles Health and dying for both players and enemies
 * Healing and stat updates for the player should be called from the PlayerHandler
 * 
 */

[System.Serializable]
public class SpawnOnDeath
{
    public GameObject Object;
    [Range(0, 100)]public int Chance; //chance of spawning
    public bool isEnemy;
}

public class HealthScript : MonoBehaviour
{
    public int  maxHealth = 100; //ezt playernek a playerHandler irja felul
                                 //with enemies, should be set in the inpsector
    public bool isPlayer = false;

    //A sebezhetetlenség allapotat tarolja
    bool isInvincible = false;

    [SerializeField]
    float iTimeWhenDamageTaken = 0.3f; // meddig tart a sebezhetetlenseg ha damaget kapsz

    public int curHealth; //only public for debugging
    bool isDead = false;

    public List<SpawnOnDeath> ObjectsToSpawn;

    public SoundClass hurtSound;
    public SoundClass deathSound;

    SpriteRenderer myRenderer; //for temp health display
    Transform myTransform;
    Rigidbody2D myRigidbody;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Awake()
    {
        //curHealth = maxHealth;
        myRenderer = GetComponent<SpriteRenderer>();

        //a playert a playerHandler-ben healeljuk (mert ott allitjuk be a HealthStatokat is)
        if (!isPlayer)
            HealToMax();
        else
            myAudioSource = GetComponent<AudioSource>();

        myTransform = GetComponent<Transform>();

        myRigidbody = GetComponent<Rigidbody2D>();
    }

	public void TakeDamage (int dmg, Vector2 knockbackDir, int knockbackStrength)
	{

        //ha sebezhetetlen akkor nem fog damaget kapni
        if (isInvincible)
            return;

        if (isPlayer) //csak player kap sebezhetetlenséget ha damaget kap
        {
            StartCoroutine(BecomeInvincible(iTimeWhenDamageTaken));
        }

        Knockback(knockbackDir, knockbackStrength);

        curHealth -= dmg;

        if (!isDead)
        {
            if (curHealth <= 0)
            {
                Die();
            }
            else
            {
                if (isPlayer)
                    UpdateHealthVisuals();

                myAudioSource.PlayOneShot(hurtSound.clip, hurtSound.volume);
            }
        }
	}

    public void Knockback(Vector2 knockbackDir, int knockbackStrength)
    {
        myRigidbody.AddForce(knockbackDir * knockbackStrength);
        //Debug.Log(knockbackStrength);
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
    // should be called from the PlayerHandler
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
        //we are using PlayClipAtThisPoint() because if we played the sound via this GameObject, the sound would stop once this gameObject is destroyed
        AudioSource.PlayClipAtPoint(deathSound.clip, myTransform.position, deathSound.volume);

        if (isPlayer)
        {
            //special GameOver stuff
            Debug.Log("You died. bruh");

            GetComponent<PlayerController>().enabled = false; //disable player controller, cos we fucking dead
            GetComponent<WeaponManager>().DisableCurWeapon(); //disable shooting as well
            myRenderer.color = Color.red;
            isDead = true;
        }
        else //if enemy
        {
            //spawing randomly from ObjectsToSpawn on death
            SpawnRandomObjects();

            //this has to be here otherwise doors open prematurely
            RoomsManager.Instance.ChangeActiveEnemiesToCount(-1); //decreasing the current enemiesInThisRoom

            //gameObject: the GameObject this component is linked to
            //            is built in, doesnt need to be retrieved with GetComponent<>()
            //Destroy (Gameobject): deletes the GameObject from the scene
            Destroy(gameObject);
        }
	}

    void SpawnRandomObjects ()
	{
        for (int i = 0; i < ObjectsToSpawn.Count; i++)
        {
            int chanceRoll = Random.Range(1, 100); // inlcusive has to be 1-100
            if (chanceRoll <= ObjectsToSpawn[i].Chance)
            {
                Instantiate(ObjectsToSpawn[i].Object, myTransform.position, myTransform.rotation);
                if (ObjectsToSpawn[i].isEnemy)
                {
                    RoomsManager.Instance.ChangeActiveEnemiesToCount(+1); //increase the current enemiesInThisRoom
                }
            }
        }
    }

    public IEnumerator BecomeInvincible(float iTime)
    {
        if (!isInvincible)
        {
            isInvincible = true;
            Debug.Log("Invincible");

            
            yield return new WaitForSeconds(iTime);


            isInvincible = false;
            Debug.Log("No longer invincible");
        }
    }
    
}
