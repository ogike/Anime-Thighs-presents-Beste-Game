using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles all the player movement/rotation
 * Dash added by Marci
 * 
 */

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float moveSpeed = 20.0f; //this is set by the PlayerHandler

    public float plusRotationalAngle = 90;

    float inputHorizontal;
    float inputVertical;
    Vector2 inputDir;

    Rigidbody2D myRigidbody;
    Camera      myCam;
    Transform   myTrans;

    //A dash base �s jelenlegi cooldownj�t t�rolja
    public float dashCooldown = 0.5f;
    public float dashSpeed = 50;

    float curCooldown; //for dash

    // Start is called before the first frame update
    void Start()
    {
        //A GetComponent-el �rsz hozz� annak a gameObjectnek a komponens�hez, amihez hozz� van csatolva ez a script
        //A rigidBody2D az a komponens, ami kezeli a physics-et
        myRigidbody = GetComponent<Rigidbody2D>();

        //Az fuck off
        myCam = GameManagerScript.Instance.mainCamera;

        //A Transform komponens kezeli a GameObject poz�ci�j�t/rot�ci�j�t
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

        //ha CD-n van a dash akkor nem tudsz //(CoolDown-on ig)
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


        //az inputot egy ir�nyvektorr� alak�tjuk
        inputDir = new Vector2(inputHorizontal, inputVertical);

        //ez ad a fizikai komponensek egy l�k�st, a mostani movement-er�t sebess�ggel
        myRigidbody.AddForce(inputDir * moveSpeed * Time.deltaTime);
    }

    void LookAtMouse ()
	{
                      //myCam.ScreenToWorldPoint(Input.mousePosition): the world position the mouse is pointing at
        Vector3 diff = (myCam.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            //basically get the direction vector from player to mouse

        float aimRotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            //turns the direction vector into a rotation angle with trigonometry (the z rotation in the editor)
            //majd �tv�ltjuk fokk�

        //plusRotationalAngle: the added compensation for the playermodel rotation
        myTrans.rotation = Quaternion.Euler(0, 0, aimRotZ + plusRotationalAngle); //sets the player Z rotation with Quaternions
    }

    void Dash()
    {
            //Ugyanaz mint a MovePlayer az ir�ny meghat�roz�s�hoz
            inputHorizontal = Input.GetAxisRaw("Horizontal"); 
            inputVertical = Input.GetAxisRaw("Vertical");       

            inputDir = new Vector2(inputHorizontal, inputVertical);
            
            //megtolja az adott ir�nyba a playert
            myRigidbody.velocity = inputDir * dashSpeed;
            
            //Cooldownra teszi a dasht
            curCooldown = dashCooldown;
    }
}
    
    
