using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Credit to Geri
 * Put this on a Pickup with Trigger collider
 * And add every stat you want to change to the "statBoosts" list in the editor
 * Note: for weapon stats, all of them are Multiplying Modifiers
 */

public class PickupStatChanger : MonoBehaviour
{
    public List<SkillStatBoost> statBoosts;

    public SoundClass pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
			for (int i = 0; i < statBoosts.Count; i++)
			{
                GameManagerScript.Instance.playerHandler.ApplyStatBoost(statBoosts[i]);
            }

            AudioManager.Instance.PlayFXSound(pickupSound);

            Destroy(gameObject);
        }
    }
}
