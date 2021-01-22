using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is like the manager of the enemy, handles all the basic stuff
public class EnemyHandler : MonoBehaviour
{
    //TEMPORARY, csak arra van hogyha megnyitod a játékot ne egybõl rohanjanak
    public float timeToWake = 2;
    float timeElapsedFromStart; //for checking if we are awake
    public bool awake = false;

    Transform targetTrans;
    Transform myTrans;

    Vector3 targetPos;
    Vector3 myPos;
    Vector3 dirToTarget; //the normalized direction
    float distToTarget;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        targetTrans = GameManagerScript.Instance.playerTransform; //automatically set it as the player thru the gameManager

        awake = false;
        timeElapsedFromStart = 0;
    }

	// Update is called once per frame
	void Update()
    {
        if (!awake)
        {
            if (timeElapsedFromStart >= timeToWake)
            {
                awake = true;
            }
            else
            {
                timeElapsedFromStart += Time.deltaTime;
            }
            return;
        }

        myPos = myTrans.position;
        targetPos = targetTrans.position;
        
        dirToTarget = targetPos - myPos; //elõször csak megszerezzük az irányt, hosszal együtt
        distToTarget = dirToTarget.magnitude; //kinyerjül ebbõl a távolságot (hosszát a vektornak)
        dirToTarget = dirToTarget.normalized; //utána már csak a normalizált irányvektor érdekel minket
    }

    //all of this should be handles by C# get/set fuckery and object-oriented magic
    //inefficient
    public bool IsAwake ()
	{
        return awake;
	}

    //parameters are references, which are like pointers
    //basically "ref" in c# = "&" in c++
    public void GetTargetVectorData (ref Vector3 dir, ref float dist)
	{
        dir = dirToTarget;
        dist = distToTarget;
	}
}
