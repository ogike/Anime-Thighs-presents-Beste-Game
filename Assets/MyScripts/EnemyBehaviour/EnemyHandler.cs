using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This is like the manager of the enemy, handles all the basic stuff
 * For now, it calculates the distance/direction to the player every frame
 *      This data can be accesed with the GetVectorData function
 *      So every enemy behaviour doesnt have to calculate them each frame
 * This should be added to every enemy!!
 */

/* Now this is also a steering agent for now
 * lol.
 * Im just prototyping at this point
 */

public class EnemyHandler : MonoBehaviour
{
    Transform targetTrans;
    Transform myTrans;
    Rigidbody2D myRigidbody;

    Vector3 dirToTarget; //the normalized direction to the player
    float distToTarget;  //the distance to the player

    public Vector3 curSteerVel;
    public float steerForce;
    public float steerMaxVel;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
        targetTrans = GameManagerScript.Instance.playerTransform; //automatically set it as the player thru the gameManager

        myTrans.position = new Vector3(myTrans.position.x, myTrans.position.y, 0); //setting z position to be 0
    }

	// Update is called once per frame
	void Update()
    {
        dirToTarget = targetTrans.position - myTrans.position; //elõször csak megszerezzük az irányt, hosszal együtt
        distToTarget = dirToTarget.magnitude; //kinyerjül ebbõl a távolságot (hosszát a vektornak)
        dirToTarget = dirToTarget.normalized; //utána már csak a normalizált irányvektor érdekel minket

        //Debug.Log(dirToTarget.x + ", " + dirToTarget.y + " " + dirToTarget.z);
    }

	private void LateUpdate()
	{
        //after all the other calculations are done, we can move
        Move();
        LookInDir(curSteerVel.normalized); //TODO optimize
        curSteerVel = Vector3.zero; //resetting for next frame
    }

	public void AddSteerDir(Vector3 plusDir, float weight)
	{
        curSteerVel += plusDir * weight;
	}
    void Move()
	{
        curSteerVel.z = 0; //safety, probably should be done better with proper types

        //TODO: replace this with squared checks
        /*if (curSteerVel.magnitude > steerMaxVel)
            curSteerVel = curSteerVel.normalized * steerMaxVel;

        myRigidbody.AddForce(curSteerVel * steerForce * Time.deltaTime);*/

        myRigidbody.AddForce(myTrans.up * steerForce * Time.deltaTime); //just move forward

        //curSteerVel = Vector3.zero; //resetting for next frame
    }

    void LookInDir(Vector3 targDir)
    {
        float angleToTarget = Mathf.Atan2(targDir.x, targDir.y) * Mathf.Rad2Deg * (-1);
        Quaternion targRot = Quaternion.Euler(0, 0, angleToTarget);

        //???
        myTrans.rotation = Quaternion.Slerp(myTrans.rotation, targRot, rotationSpeed * Time.deltaTime);
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
