using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is the script thatcan be accesed from anywhere in the scene
 * Use this if you want to handle GameStage stuff, or access the player/camera from different scripts
 *      Especially because GetComponent is slow, accesing player components from this script is ideal 
 */

public class GameManagerScript : MonoBehaviour
{

    //make this instance of the GameManagerScript static, so this will be a static script basically
    //which means it can be acesses from anywhere
    //the get/set thing is so everyone can access this, but only this script can set this static variable (which happens in Awake)
    public static GameManagerScript Instance { get; private set; } //basically make this a singleton

    public GameObject playerObject; //need to be set manually
    public Camera    mainCamera;
    public float     cameraZPosition = -10; //where the camera should be on the Z axis
    public float     playerZPosition = 0;

    public GameObject winReward; //PLACEHOLDER
                                 //This is what gets activated when we win

    Transform camTrans;
    [HideInInspector] public Transform playerTransform;
    [HideInInspector] public HealthScript playerHealth; //automatically set
    [HideInInspector] public PlayerHandler playerHandler;

    //Awake is called before start
	private void Awake()
	{
        Instance = this;

        camTrans = mainCamera.GetComponent<Transform>();

        playerTransform = playerObject.GetComponent<Transform>();
        playerHealth    = playerObject.GetComponent<HealthScript>();
        playerHandler   = playerObject.GetComponent<PlayerHandler>();

        winReward.SetActive(false); //PLACEHOLDER
	}

    public void SetCameraPosition (Vector3 newPos)
	{
        //should make this fancy later, and within the CameraScript
        camTrans.position = new Vector3(newPos.x, newPos.y, cameraZPosition);
    }

    public void SetPlayerPosition (Vector3 newPos)
	{
        //basically teleport
        playerTransform.position = new Vector3(newPos.x, newPos.y, playerZPosition);
        //fancy stuff goes here
	}

    //called when we win the game
    public void WinStage()
	{
        winReward.SetActive(true);
	}
}
