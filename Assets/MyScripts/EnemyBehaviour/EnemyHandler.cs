using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is like the manager of the enemy, handles all the basic stuff
 * For now, it calculates the distance/direction to the player every frame
 *      This data can be accesed with the GetVectorData function
 *      So every enemy behaviour doesnt have to calculate them each frame
 * This should be added to every enemy!!
 */

public class EnemyHandler : MonoBehaviour
{
    Transform targetTrans;
    Transform myTrans;

    Vector3 dirToTarget; //the normalized direction to the player
    float distToTarget;  //the distance to the player

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        targetTrans = GameManagerScript.Instance.playerTransform; //automatically set it as the player thru the gameManager

        myTrans.position = new Vector3(myTrans.position.x, myTrans.position.y, 0); //setting z position to be 0
    }

	// Update is called once per frame
	void Update()
    {
        dirToTarget = targetTrans.position - myTrans.position; //elõször csak megszerezzük az irányt, hosszal együtt
        distToTarget = dirToTarget.magnitude; //kinyerjül ebbõl a távolságot (hosszát a vektornak)
        dirToTarget = dirToTarget.normalized; //utána már csak a normalizált irányvektor érdekel minket

        Debug.Log(dirToTarget.x + ", " + dirToTarget.y + " " + dirToTarget.z);
    }

    public Vector3 GetDirToPlayer ()
	{
        return dirToTarget;
	}

    public float GetDistToTarget ()
	{
        return distToTarget;
	}

    //parameters are references, which are like pointers
    //basically "ref" in c# = "&" in c++
    public void GetTargetVectorData (ref Vector3 dir, ref float dist)
	{
        dir = dirToTarget;    //updated the passed direction variable to the current one
        dist = distToTarget;  //same here
	}
}
