using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float followTime = 1; // much time it takes to smoothlyupdate the position
                                 //the lower the faster

    public float snappingDistance = 0.001f; //the distance where we just snap to the target position

    [HideInInspector] public Vector3 targetPos;

    Transform myTrans;
    Vector3 curPos;
    float curDist; //distance between target and current position
    Vector3 lerpHelperVelocity = Vector3.zero; //this is for the Vector3.SmoothDamp, unity handles it

    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        curPos = myTrans.position;
        curDist = Vector3.Distance(curPos, targetPos);

        if (curDist != 0)
        {
            if (curDist > snappingDistance)
            {
                myTrans.position = Vector3.SmoothDamp(curPos, targetPos, ref lerpHelperVelocity, followTime);
            }
            else
			{
                myTrans.position = targetPos;
            }
        }
    }

    public void UpdateTargetPosition (Vector3 newTargPos)
	{
        targetPos = newTargPos;
	}
}
