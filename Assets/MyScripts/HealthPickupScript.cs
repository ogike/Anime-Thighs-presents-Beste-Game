using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Credit to Geri
 * Simple HealthBooster script, that can be put on Pickups with a Trigger
 */

public class HealthPickupScript : MonoBehaviour
{
	public int healthGiven = 50;

	public SoundClass pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
			GameManagerScript.Instance.playerHandler.HealPlayer(healthGiven);
			AudioManager.Instance.PlayFXSound(pickupSound);
            Destroy(gameObject);
		}
	}
}
