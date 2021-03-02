using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Should be put on the GameManager GameObject in the scene
 * Also a Singleton, the same way as the game manager is, so this can be accesed from anywhere with RoomsManager.Instance
 * Handles switching between different rooms, and communicates between different RoomHandler-s on room change
 * !!Every existing room inthe scene should be added to the "rooms" list!!!
 */

public class RoomsManager : MonoBehaviour
{
    public static RoomsManager Instance { get; private set; } //makes this a singleton, the same as with GameManager

    public List<RoomHandler> rooms; //0-th has to be the starting room!

    public SoundClass openDoorsSound;
    public SoundClass closeDoorsSound;

    public int curEnemyCountInRoom; //only public for debug
    int curRoomIndex; //what is the index of the current room in the "rooms" list
                      //-1 = its hasnt been set

	private void Awake()
	{
        Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        //deactivate all the rooms
        for (int i = 0; i < rooms.Count; i++)
		{
            rooms[i].DeactivateRoom();
        }

        curRoomIndex = -1; //saying this is the first room
        ChangeActiveRoom(rooms[0], Vector3.zero); //activating the first room
                                                  //the second parameter is where the player will be spawned basically (without actual spawning)
    }

    public void ChangeActiveRoom (RoomHandler newRoom, Vector3 newPlayerPosition)
	{
        if(curRoomIndex != -1) //if its not the first room
            rooms[curRoomIndex].LeaveRoom();

        //bejön a haskell névtelen függvények
        //FindIndex: a listában keres elemet amire igaz a predikátum
        //(maybe it would be worth optimizing, so that we always store and pass room indexes too???
        curRoomIndex = rooms.FindIndex(x => (x == newRoom));
        if (curRoomIndex == -1)
        {
            Debug.LogError("Valamit elbasztál, nincsen a keresett szoba a listában");
        }

        GameManagerScript.Instance.SetPlayerPosition(newPlayerPosition);
        GameManagerScript.Instance.SetCameraPosition(rooms[curRoomIndex].cameraPosTrans.position);

        curEnemyCountInRoom = rooms[curRoomIndex].numOfenemiesInThisRoom; //updating the enemy counter to the new room
        rooms[curRoomIndex].EnterRoom();
    }

    //all this should be refactored with get/set classes
    //this is called when entering the room, or spawning more enemies
    //(negative count = removing, positive = adding)
    public void ChangeActiveEnemiesToCount(int plusCount)
    {
        curEnemyCountInRoom += plusCount;

        if (curEnemyCountInRoom <= 0) //if there are no more enemies in this room
        {
            rooms[curRoomIndex].CompleteRoom();
        }
    }

    //for sound effects basically
    public void OpenDoors()
	{
        AudioManager.Instance.PlayFXSound(openDoorsSound);
	}

    public void CloseDoors()
	{
        AudioManager.Instance.PlayFXSound(closeDoorsSound);
	}
}
