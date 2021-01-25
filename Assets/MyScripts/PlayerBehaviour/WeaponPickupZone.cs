using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Entering a trigger with this component switches weapons
 * The weapon to switch to is the index of the wanted weapon in the WeaponManager script
 */

public class WeaponPickupZone : MonoBehaviour
{
    public int weaponIndexToSwitchTo; //csúnya megoldás, nem absztrakt számokkal kéne kezelni editorban, probably should make an enum for this
                                      //indexed from zero

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Player")
		{
            collision.GetComponent<WeaponManager>().SwitchWeapons(weaponIndexToSwitchTo);
		}
	}
}
