using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public float moveSpeed;
    public float rotationSpeed;

    public float decideTimeMin;
    public float decideTimeMax;

    public float wallAvoidanceDist = 2f;
    public LayerMask wallAvoidLayers;

    public int maxNumOfDecideTries = 20; //how many times we can try for a random direction before disabling

    float curDecideTime;
    Vector3 curDir;

    Rigidbody2D myRigidbody;
    Transform myTrans;

    RaycastHit2D tempRayHit;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myTrans = GetComponent<Transform>();

        ChooseNewdir();
    }

    // Update is called once per frame
    void Update()
    {
        if(curDecideTime > 0)
		{
            curDecideTime -= Time.deltaTime;
        }
        else
		{
            ChooseNewdir();
        }

        MoveInCurDir();
        LookInDir(curDir);
    }

	private void FixedUpdate()
	{
        //this might be really performance heavy, should do these checks less
        tempRayHit = Physics2D.Raycast(myTrans.position, curDir, wallAvoidanceDist, wallAvoidLayers);
        if(tempRayHit.collider != null)
		{
            //Debug.LogWarning("Wanderer found obstacle in dir: (" + curDir.x + ", " + curDir.y + "), with name: " + tempRayHit.transform.name);
            ChooseNewdir();
		}
    }

	void ChooseNewdir ()
	{
        bool isCorrectDir = true; //false if the next dir sends us into a wall
        int curTries = 0;

        do
        {
            curDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);

            tempRayHit = Physics2D.Raycast(myTrans.position, curDir, wallAvoidanceDist, wallAvoidLayers);

            isCorrectDir = tempRayHit.collider == null; //if the raycast didnt hit anything, its a correct direction

			if (!isCorrectDir)
			{
                //DEBUG!!
                //Debug.LogWarning("Wanderer found obstacle in dir: (" + curDir.x + ", " + curDir.y + "), with name: " + tempRayHit.transform.name);
			}

            curTries++;
            if(curTries >= maxNumOfDecideTries) //if we are stuck in a n endless loop
            {
                //TEMPORARY 
                    //this should later be set to automatically re-enable after a set time
                Debug.LogWarning("This wanderers decision making is stuck in an endless loop, disabling");
                this.enabled = false; //disabling this component
                return;
			}

        } while (!isCorrectDir);

        curDir.Normalize();

        curDecideTime = Random.Range(decideTimeMin, decideTimeMax);
    }

    void LookInDir(Vector3 targDir)
	{
        float angleToTarget = Mathf.Atan2(targDir.x, targDir.y) * Mathf.Rad2Deg * (-1);
        Quaternion targRot = Quaternion.Euler(0, 0, angleToTarget);

        //???
        myTrans.rotation = Quaternion.Slerp(myTrans.rotation, targRot, rotationSpeed * Time.deltaTime);
    }

    void MoveInCurDir()
    {
        myRigidbody.AddForce(myTrans.up * moveSpeed * Time.deltaTime);
        //myRigidbody.AddForce(curDir * moveSpeed * Time.deltaTime);
    }
}
