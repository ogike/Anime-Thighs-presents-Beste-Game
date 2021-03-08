using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookInVelocity : MonoBehaviour
{
    public float rotationSpeed;

    Transform myTrans;
    Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        LookForward();
    }
    void LookForward()
    {
        Vector2 curVel = myRigidbody.velocity;
        float angleToTarget = Mathf.Atan2(curVel.x, curVel.y) * Mathf.Rad2Deg * (-1);
        Quaternion targRot = Quaternion.Euler(0, 0, angleToTarget);

        //???
        myTrans.rotation = Quaternion.Slerp(myTrans.rotation, targRot, rotationSpeed * Time.deltaTime);
    }
}
