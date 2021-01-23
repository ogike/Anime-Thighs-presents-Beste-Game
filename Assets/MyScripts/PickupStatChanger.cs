using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupStatChanger : MonoBehaviour
{
    public List<SkillStatBoost> statBoosts;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
			for (int i = 0; i < statBoosts.Count; i++)
			{
                GameManagerScript.Instance.playerHandler.ApplyStatBoost(statBoosts[i]);
            }

            Destroy(gameObject);
        }
    }
}
