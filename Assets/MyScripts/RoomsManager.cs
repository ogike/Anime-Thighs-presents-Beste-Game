using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//basically just deactivates all the rooms
//probably should be simply part of gameManager???
public class RoomsManager : MonoBehaviour
{
    public static RoomsManager Instance { get; private set; } //makes this a singleton, the same as with GameManager

    public List<RoomHandler> rooms; //0-th has to be the starting room!

    public int curEnemyCountInRoom; //only public for debug
    int curRoomIndex;

	private void Awake()
	{
        Instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        for (int i = 0; i < rooms.Count; i++)
		{
            rooms[i].DeactivateRoom();
        }

        rooms[0].ActivateRoom();
    }

    public void ChangeActiveRoom (RoomHandler newRoom)
	{
        //bejön a haskell névtelen függvények
        //FindIndex: a listában keres elemet amire igaz a predikátum
        //(maybe it would be worth optimizing, so that we always store and pass room indexes too???)
        curRoomIndex = rooms.FindIndex(x => (x == newRoom));

        if(curRoomIndex == -1)
		{
            Debug.LogError("Valamit elbasztál, nincsen a keresett szoba a listában");
		}
    }

    //all this should be refactored with get/set classes
    //this is called when entering the room, or spawning more enemies
    //(negative count = removing, positive = adding)
    public void ChangeActiveEnemiesToCount(int plusCount)
    {
        curEnemyCountInRoom += plusCount;

        if (curEnemyCountInRoom <= 0) //if there are no more enemies in this room
        {
            rooms[curRoomIndex].OpenDoors();
            rooms[curRoomIndex].NoMoreEnemies();
        }
    }
}
