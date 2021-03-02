using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/* Script to put onto every Room GameObject
 * Also every instance of these should be added to the "rooms" list in the RoomManager
 * Handles activating/deactivating stuff in the room on entering
 */

public class RoomHandler : MonoBehaviour
{
    public Transform cameraPosTrans; //the position where the camera should be in this room

    public GameObject enemiesToWakeOnEnter; //all the enemies to activate on room-enter

    public GameObject playerBlockers;

    public DoorScript[] doors;

    //public GameObject rewards; //the gameObject to activate when all the enemies are dead in this room
                               //optional

    public List<GameObject> rewards;

    [HideInInspector] public int numOfenemiesInThisRoom;
    public List<Transform> curEnemiesInThisRoom;
    [HideInInspector] public bool completed;

    public void Awake()
	{
        Transform enemiesParent = enemiesToWakeOnEnter.transform;

        numOfenemiesInThisRoom = enemiesParent.childCount;

		//TODO: probably very performance heavy, should be baked in during editor-time with a burron or sth
		for (int i = 0; i < numOfenemiesInThisRoom; i++)
		{
            curEnemiesInThisRoom.Add(enemiesParent.GetChild(i));
        }

        if(numOfenemiesInThisRoom == 0) //ha alapból nincs ellenség szobában, egybõl completed
		{
            CompleteRoom();
        }
        else
		{
            completed = false;
		}
    }

    //called from the RoomManager when entering
    public void EnterRoom ()
	{
        if (!completed)
        {
            enemiesToWakeOnEnter.SetActive(true);

            RoomsManager.Instance.CloseDoors(); //for sound fx
        }
        else
		{
            playerBlockers.SetActive(false); //deactivate the player blockers
        }

        //activate door scripts, even if they are locked behind the playerBlockers
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].isActive = true; //set the doorscript variable to be true
        }
    }

    //called from the RoomManager when leaving
    public void LeaveRoom ()
	{
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].isActive = false; //set the doorscript variable to be false
        }
    }

    //called when all the enemies are killed in this room
    public void CompleteRoom()
    {
        completed = true;
        numOfenemiesInThisRoom = 0;

        playerBlockers.SetActive(false); //deactivate the player blockers
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].isActive = true; //activating the doorScript variables, so the player can leave
        }

        if (rewards.Any())
        {
            int index = Random.Range(0, rewards.Count);
            Instantiate(rewards[index], cameraPosTrans);
            //rewards[index].SetActive(true);
        }

        RoomsManager.Instance.OpenDoors(); //for sound fx
    }

    //should only be called at the start of the game for the unopened rooms
    public void DeactivateRoom()
    {
        enemiesToWakeOnEnter.SetActive(false); //deactivate enemies

        //deactivate rewards
        /*if(rewards.Any())
        {
            for(int i = 0;i < rewards.Count;i++)
            {
                rewards[i].SetActive(false);
            }
        }*/

        playerBlockers.SetActive(true);
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].isActive = false; //set the doorscript variable to be false
        }
    }

    public void EnemyDied(Transform deceased)
	{
        curEnemiesInThisRoom.Remove(deceased);
	}

    public void EnemySpawned(Transform spawned)
	{
        curEnemiesInThisRoom.Add(spawned);
	}

    public List<Transform> GetNeighbours (Transform orig, float maxDist)
	{
        Vector3 origPos = orig.position;

        List<Transform> neighbours = new List<Transform>();

		foreach (Transform enemy in curEnemiesInThisRoom)
		{
            if (enemy != orig && Vector2.Distance(origPos, enemy.position) < maxDist)
			{
                neighbours.Add(enemy);
			}
		}

        return neighbours;
	}
}
