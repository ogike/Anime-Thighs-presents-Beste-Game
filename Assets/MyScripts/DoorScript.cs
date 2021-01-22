using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Transform teleportDestinationTrans;

    //QOL: could get this from the parent of the teleportDest in Start()
    public RoomHandler destRoomScript;

   // public GameObject playerBlocker;


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
        //when the player enters the door trigger:
        if (collision.tag == "Player")
		{
            destRoomScript.ActivateRoom(); //activating the new room;

            GameManagerScript.Instance.SetPlayerPosition(teleportDestinationTrans.position); //teleport the player
		}
	}
}
