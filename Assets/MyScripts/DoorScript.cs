using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Ezt kell rátenni az ajtókra
 * Ez kezeli a szobák közti váltást gyakorlatilag
 * Basztatnivaló ember: ogike
 */

public class DoorScript : MonoBehaviour
{
    public Transform teleportDestinationTrans;

    //QOL: could get this from the parent of the teleportDest in Start()
    public RoomHandler destRoomScript;

    /*[HideInInspector]*/ public bool isActive;


	private void OnTriggerEnter2D(Collider2D collision)
	{
        //when the player enters the door trigger:
        if (isActive && collision.tag == "Player")
		{
            RoomsManager.Instance.ChangeActiveRoom(destRoomScript, teleportDestinationTrans.position);

            /*
            destRoomScript.ActivateRoom(); //activating the new room;

            GameManagerScript.Instance.SetPlayerPosition(teleportDestinationTrans.position); //teleport the player
            */
		}
	}
}
