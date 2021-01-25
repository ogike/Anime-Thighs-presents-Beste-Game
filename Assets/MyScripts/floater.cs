using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// le fel mozgat egy objectet sin fuggveny alapjan
// ne hasznald olyanra ami mashogy is mozoghat, a rigidbody elbaszahtja a transformot!

public class floater : MonoBehaviour
{
    Transform   myTrans;
    double index;
    public double speed;
    // Start is called before the first frame update
    void Start()
    {
        myTrans = GetComponent<Transform>();
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        index += speed;
        myTrans.Translate(Vector3.up * Time.deltaTime* Mathf.Sin((float)index));
    }
}
