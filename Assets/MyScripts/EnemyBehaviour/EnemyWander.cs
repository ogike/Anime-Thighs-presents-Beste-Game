using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public float moveSpeed;

    public float decideTimeMin;
    public float decideTimeMax;

    public float wallAvoidanceDist = 2f;
    public LayerMask wallAvoidLayers;

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

        } while (!isCorrectDir);

        curDir.Normalize();

        curDecideTime = Random.Range(decideTimeMin, decideTimeMax);
    }

    void MoveInCurDir()
    {
        myRigidbody.AddForce(curDir * moveSpeed * Time.deltaTime);
    }
}
