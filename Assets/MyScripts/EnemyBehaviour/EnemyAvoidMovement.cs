using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Credit to Meem
 * Enemy movement behaviour, avoid the player by always going in the opposite direction
 * Similiar to EnemyBasicFollow
 */

public class EnemyAvoidMovement : MonoBehaviour
{
    public float moveSpeed;
    public float distToStop; //if within this distance from the player, we wont move
    public float distToStart; //if farther than this distance from the player, we wont move

    EnemyHandler myHandler;

    Transform targetTrans;
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

        targetTrans = GameManagerScript.Instance.playerTransform; //automatically set it as the player thru the gameManager

        myTrans.position = new Vector3(myTrans.position.x, myTrans.position.y, 0); //setting z position to be 0
    }

    // Update is called once per frame
    // LateUpdate is the same, but its called after the normal Update()-s, which means it will be after the EnemyScripts's Update, and that all the variables will be up-to-date
    void LateUpdate()
    {
        //pass these variables as references to the method (like pointers)
        myHandler.GetTargetVectorData(ref dirToTarget, ref distToTarget);

        if (distToTarget > distToStop && distToStart > distToTarget)
        {
            Vector3 oppositeDir = new Vector3(-1 * dirToTarget.x, -1 * dirToTarget.y, 0);
            MoveInDir(oppositeDir);
        }
    }

    void MoveInDir(Vector3 dir)
    {
        myRigidbody.AddForce(dir * moveSpeed * Time.deltaTime);
    }
}
