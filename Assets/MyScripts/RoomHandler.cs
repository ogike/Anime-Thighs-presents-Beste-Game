using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHandler : MonoBehaviour
{
    public Transform cameraPosTrans; //the position where the camera should be in this room

    public GameObject enemiesToWakeOnEnter; //all the enemies to activate on room-enter

    public GameObject playerBlockers;

    //public GameObject rewards; //the gameObject to activate when all the enemies are dead in this room
                               //optional

    public List<GameObject> rewards;

    public int enemiesInThisRoom;

	public void Awake()
	{
        enemiesInThisRoom = enemiesToWakeOnEnter.transform.childCount;
    }

	public void ActivateRoom()
	{
        if (enemiesInThisRoom > 0)
        {
            enemiesToWakeOnEnter.SetActive(true); //activate all the enemy GameObjects
            RoomsManager.Instance.ChangeActiveEnemiesToCount(enemiesInThisRoom); //updating the curEnemiesInRoom
        }
        else
		{
            //if there are no enemies, doors should be open
            OpenDoors();
            NoMoreEnemies();

        }

        GameManagerScript.Instance.SetCameraPosition(cameraPosTrans.position);
        RoomsManager.Instance.ChangeActiveRoom(this);
    }

    public void DeactivateRoom()
    {
        //should only be called at the start of the game for the unopened rooms
        enemiesToWakeOnEnter.SetActive(false); //deactivate enemies

        if(rewards != null)
            rewards.SetActive(false); //deactivate rewards

        CloseDoors();
    }

    public void CloseDoors()
	{
        playerBlockers.SetActive(true);
    }

    public void OpenDoors ()
	{
        playerBlockers.SetActive(false); //deactivate the player blockers
    }

    //called when all the enemies are killed in this room
    public void NoMoreEnemies ()
	{
        enemiesInThisRoom = 0;

        bool test = rewards.Any();
        if (test == true)
        {
            int index = Random.Range(0, rewards.Count);
            rewards[index].SetActive(true);
        }
            
    }
}
