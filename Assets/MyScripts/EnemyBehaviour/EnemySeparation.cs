using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeparation : MonoBehaviour
{
    public float debugLineLength = 1;

    public float maxForce;
    public float distance;

    List<Transform> neighbours;

    Vector3 separationDir;

    RoomHandler myRoomHandler;
    Transform myTrans;
    Rigidbody2D myRigidbody;
    Vector3 curPos;

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        myRigidbody = GetComponent<Rigidbody2D>();

        //TODO expensive, should be set in enemyHandler
        myRoomHandler = myTrans.parent.parent.GetComponent<RoomHandler>();
 
    }

    // Update is called once per frame
    void Update()
    {
        separationDir = CalcSeparationForce();

        Move(separationDir);
    }

    Vector3 CalcSeparationForce()
	{
        Vector3 newDir = Vector3.zero;

        neighbours = myRoomHandler.GetNeighbours(myTrans, distance);

        if (neighbours.Count > 0)
        {
            curPos = myTrans.position;

            foreach (Transform otherTrans in neighbours)
            {
                newDir += curPos - otherTrans.position;
            }

            newDir.z = 0; //cos 2d space

            //TODO: this could be made smoother depending on distance
            //separationDir = separationDir.normalized * maxForce;
            //newDir.Normalize();
            newDir /= neighbours.Count;

            neighbours.Clear(); //resetting the cached memory

            Debug.DrawLine(curPos, curPos + (newDir * debugLineLength), Color.magenta, Time.deltaTime);
        }

        return newDir;
    }

    //should be modularized somewhere else
    void Move(Vector3 dir)
	{
        myRigidbody.AddForce(dir * maxForce * Time.deltaTime);
    }
}
