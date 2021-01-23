using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{
     public int healthGiven = 50;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
            GameManagerScript.Instance.playerHealth.Heal(healthGiven);
            Destroy(gameObject);
		}
	}
}
