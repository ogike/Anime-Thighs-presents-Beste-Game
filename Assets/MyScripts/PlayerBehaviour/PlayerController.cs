using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 20.0f;
    public float dashSpeed = 50;

    public float plusRotationalAngle = 90;

    float inputHorizontal;
    float inputVertical;
    Vector2 inputDir;
    Vector2 dashDir; 

    Rigidbody2D myRigidbody;
    Camera      myCam;
    Transform   myTrans;

    //A dash base és jelenlegi cooldownját tárolja
    public float dashCooldown = 0.5f;
    public float curCooldown;

    
    // Start is called before the first frame update
    void Start()
    {
        //A GetComponent-el érsz hozzá annak a gameObjectnek a komponenséhez, amihez hozzá van csatolva ez a script
        //A rigidBody2D az a komponens, ami kezeli a physics-et
        myRigidbody = GetComponent<Rigidbody2D>();

        //Az fuck off
        myCam = GameManagerScript.Instance.mainCamera;

        //A Transform komponens kezeli a GameObject pozícióját/rotációját
        myTrans = GetComponent<Transform>();

        //instant lehessen dashelni startkor
        curCooldown = 0;

        
     }

    // Update is called once per frame
    void Update()
    {
        if (curCooldown > 0)
        {
            curCooldown -= Time.deltaTime;
        }

        
        //ha CD-n van a dash akkor nem tudsz
        if (Input.GetKeyDown(KeyCode.LeftShift) && curCooldown <= 0)
        {
            Dash();
        }


        MovePlayer();
        LookAtMouse();
    }

    void MovePlayer ()
	{
        //get movement inputs as floats
        inputHorizontal = Input.GetAxisRaw("Horizontal"); //might need to handle smoothing myself later idk
        inputVertical = Input.GetAxisRaw("Vertical");


        //az inputot egy irányvektorrá alakítjuk
        inputDir = new Vector2(inputHorizontal, inputVertical);

        //ez megadja a fizikai komponensek hogy mi legyen a mostani sebessége (irány * sebesség)
        //késõbb lehet át kéne alakítani hogy csak sima lökést adjon a rigidbody.AddForce()-al
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

    void Dash()
    {
        //Ugyanaz mint a MovePlayer az irány meghatározásához
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        dashDir = new Vector2(inputHorizontal, inputVertical);
        //megtolja az adott irányba a playert
        myRigidbody.velocity = dashDir * dashSpeed;

        //Cooldownra teszi a dasht
        curCooldown = dashCooldown;
            
    }
    
}
    
    
