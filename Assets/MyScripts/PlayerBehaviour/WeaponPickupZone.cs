using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupZone : MonoBehaviour
{
    public int weaponIndexToSwitchTo; //csúnya megoldás, nem absztrakt számokkal kéne kezelni editorban, probably should make an enum for this
                                      //indexed from zero

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
            collision.GetComponent<WeaponManager>().SwitchWeapons(weaponIndexToSwitchTo);
		}
	}
}
