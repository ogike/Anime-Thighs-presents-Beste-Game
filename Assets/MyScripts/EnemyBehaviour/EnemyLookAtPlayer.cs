using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLookAtPlayer : MonoBehaviour
{
    public float plusRotation; //rotation value thats always applied

    EnemyHandler myHandler;

    Transform myTrans;
    Rigidbody2D myRigidbody;

    Vector3 dirToTarget; //the normalized direction
    float distToTarget;


    // Start is called before the first frame update
    void Start()
    {
        myHandler = GetComponent<EnemyHandler>();
        myTrans = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();

        myTrans.position = new Vector3(myTrans.position.x, myTrans.position.y, 0); //setting z position to be 0
    }

    // Update is called once per frame
    // LateUpdate is the same, but its called after the normal Update()-s, which means it will be after the EnemyScripts's Update, and that all the variables will be up-to-date
    void LateUpdate()
    {
        if (!myHandler.IsAwake()) //could optimize this, instead of calling another script every frame
        {
            //if asleep, dont do anything
            return;
        }

        //pass these variables as references to the method (like pointers)
        myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        RotateIn2D(dirToTarget);
    }

    //should make this a seperate script/monobehaviour sometime
    //fuck oop tho, im too dumb
    void RotateIn2D(Vector3 dir)
    {
        float rotZ = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        myTrans.rotation = Quaternion.Euler(0, 0, rotZ - plusRotation);
    }
}

