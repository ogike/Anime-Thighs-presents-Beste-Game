using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatBoostScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int dmg;
    public float duration;

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
            GameManagerScript.Instance.playerShoot.dmgIncrease(duration,dmg);
            Destroy(gameObject);
		}
	}
}
