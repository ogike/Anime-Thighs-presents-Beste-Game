using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 20.0f;

    public float plusRotationalAngle = 90;

    float inputHorizontal;
    float inputVertical;
    Vector2 inputDir;

    Rigidbody2D myRigidbody;
    Camera      myCam;
    Transform   myTrans;

    // Start is called before the first frame update
    void Start()
    {
        //A GetComponent-el �rsz hozz� annak a gameObjectnek a komponens�hez, amihez hozz� van csatolva ez a script
        //A rigidBody2D az a komponens, ami kezeli a physics-et
        myRigidbody = GetComponent<Rigidbody2D>();

        //Az 
        myCam = GameManagerScript.Instance.mainCamera;

        //A Transform komponens kezeli a GameObject poz�ci�j�t/rot�ci�j�t
        myTrans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        LookAtMouse();
    }

    void MovePlayer ()
	{
        //get movement inputs as floats
        inputHorizontal = Input.GetAxisRaw("Horizontal"); //might need to handle smoothing myself later idk
        inputVertical = Input.GetAxisRaw("Vertical");


        //az inputot egy ir�nyvektorr� alak�tjuk
        inputDir = new Vector2(inputHorizontal, inputVertical);

        //ez megadja a fizikai komponensek hogy mi legyen a mostani sebess�ge (ir�ny * sebess�g)
        //k�s�bb lehet �t k�ne alak�tani hogy csak sima l�k�st adjon a rigidbody.AddForce()-al
        //myRigidbody.velocity = inputDir * moveSpeed;// * Time.deltaTime;

        myRigidbody.AddForce(inputDir * moveSpeed * Time.deltaTime);
    }

    void LookAtMouse ()
	{
        //paszta from my older project
        Vector3 diff = (myCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        float aimRotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        //plusRotValue???

        //honestly i have no fucking idea have euler-s work anymore
        myTrans.rotation = Quaternion.Euler(0, 0, aimRotZ + plusRotationalAngle);
        //myTrans.rotation = Quaternion.AngleAxis(aimRotZ, Vector3.forward);
    }
}
