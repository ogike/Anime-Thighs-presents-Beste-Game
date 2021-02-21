using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWander : MonoBehaviour
{
    public float moveSpeed;

    public float decideTimeMin;
    public float decideTimeMax;

    float curDecideTime;
    Vector3 curDir;

    Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();

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

    void ChooseNewdir ()
	{
        curDir = new Vector3( Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0 );

        curDecideTime = Random.Range(decideTimeMin, decideTimeMax);
    }

    void MoveInCurDir()
    {
        myRigidbody.AddForce(curDir * moveSpeed * Time.deltaTime);
    }
}
