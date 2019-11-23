﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Players : MonoBehaviour
{
    //Controls
    public KeyCode Jump = KeyCode.D;
    public KeyCode Run = KeyCode.C;
    public KeyCode Duck = KeyCode.S; //Hide behind boxes
    public KeyCode Peak = KeyCode.W; //Peak Out from cover
    public KeyCode Reset = KeyCode.V; // for testing purposes
    public KeyCode ShootHigh = KeyCode.F; // Only kills jumping characters
    public KeyCode ShootLow = KeyCode.E; //Kills a running player or when a player is peaking
    public KeyCode Reload = KeyCode.R;
//    public KeyCode Menu = KeyCode.Escape;

    private int CoverPosition = 0; // Determines what cover position the player is in
    public float Speed =0.5f; // How fast the character moves
    public GameObject Cover1;  //First Position
    public GameObject Cover2; //Second Position
    public GameObject Cover3; // Third Position
    public GameObject Cover4; //Fourth Position
    public GameObject Jump1;
    public GameObject Jump2;
    public GameObject Jump3;
    public GameObject Jump4;
    public Camera camera;
    public GameObject BlueWin;
    public GameObject RedWin;

   
    public GameObject Player; //Tells who the player is
    
    public GameObject Enemy; // Tells who the enemy is
    public GameObject Winner; //The winners platform
    public GameObject Spawn; //Spawn point of players
    public GameObject Gun; // The players weapon
    
    
    private float MuzTime1; // Determines how long the muzzle flash should last
    private float MuzTime2; //by comparing these two times
    private int StageCount;
    private bool ShotHigh; //Determines whether a player has shot high (within a given time frame)
    private bool ShotLow; //Determines whether a player has shot low (within a given time frame)
    private bool ChooseJump; //Determines if the player has chosen to jump
    private bool ChooseRun; //Determines if the player has chosen to run
    private bool jumping = false;
    private float DeadTime1; //This is the variable for the bullet delay, it compares the two times
    private float DeadTime2; //and determines if the player is out of cover

    public float MuzzleFlashTime = 0.1f; // Determines how long the muzzle flash stays on screen
    public float BulletHitTime = 0.1f; //Determines the delay from when player shoots to when it hits
    public float BulletMissTime = 0.6f; //Determines how long befoe the enemy is safe
    public float ReloadTime = 3f;
    private float rlt1;
    private float rlt2;
    private bool BulletLoaded;
    //private bool enemydead;
    public GameObject MuzzleFlash; //the muzzle flash game object
    private bool EnemyCover; //Determines if the enemy is in cover
    private bool CanShoot; //Determines if the player has the ability to shoot
    private bool reloading;
    private bool muz; // keeps trakc if muzzle flash is active
    private bool muz2;
    
    private bool InCover; //Determines whether the player is in cover or not
    
    // Start is called before the first frame update
    void Start()
    {
        InCover = true; //Players start in cover
        MuzzleFlash.SetActive(false); 
        CanShoot = true;
        BulletLoaded = true;
        reloading = false;
         
       

    }
   
    // Update is called once per frame
    void Update()
    {

        EnemyCover = Enemy.GetComponent<Players>().InCover; // checks if the enemy is in cover
        MuzzleFlashFunct(); //Function that handles muzzle flashes
        //***************This makes the player pull out their gun when comming out of cover***************************
        if (InCover)

        {

            Gun.SetActive(false);
        }
        else
        {
            Gun.SetActive(true);
        }

        Aiming();
        //***************************Resets the game for testing purposes************************************************************************
        if (Input.GetKeyDown(Reset))
        {
            Player.transform.position = new Vector2(Spawn.transform.position.x, Spawn.transform.position.y);
            CoverPosition = 0;


            Enemy.GetComponent<Players>().InCover = true;
            ChooseRun = false;
            ChooseJump = false;
            jumping = false;
            InCover = true;
            camera.transform.position = new Vector3(0, 0, -10);
        }

        

        //****************************This handles when a player hits another player ******************************

        if (((DeadTime1 - DeadTime2) > BulletHitTime) && ShotLow && !EnemyCover && !Enemy.GetComponent<Players>().ChooseJump) //Determines the delay between bullet firing and hitting target
        {


            Enemy.transform.position = Enemy.GetComponent<Players>().Spawn.transform.position; //Respawns the hit player
            //resets variables
            Enemy.GetComponent<Players>().CoverPosition = 0;
            Enemy.GetComponent<Players>().CanShoot = true;
            Enemy.GetComponent<Players>().InCover = true;
            Enemy.GetComponent<Players>().ChooseRun = false;
            Enemy.GetComponent<Players>().jumping = false;
            Enemy.GetComponent<Players>().ChooseJump = false;
        }
        else if (((DeadTime1 - DeadTime2) > BulletMissTime)) //This means the bullet has passed and wont hit the player
        {
            ShotLow = false;
        }
        //This just does the same thing as the statement above but for when the player jumps
        if (((DeadTime1 - DeadTime2) > BulletHitTime) & ShotHigh && !EnemyCover && Enemy.GetComponent<Players>().ChooseJump)
        {

            Enemy.transform.position = Enemy.GetComponent<Players>().Spawn.transform.position; //respawns enemy
            //reset variables
            Enemy.GetComponent<Players>().CoverPosition = 0;
            Enemy.GetComponent<Players>().jumping = false;
            Enemy.GetComponent<Players>().CanShoot = true;
            Enemy.GetComponent<Players>().InCover = true;
            Enemy.GetComponent<Players>().ChooseRun = false;
            Enemy.GetComponent<Players>().ChooseJump = false;


        }
        else if (((DeadTime1 - DeadTime2) > 0.6f))
        {
            ShotHigh = false;
        }
        
        PlayerChoosesRun();

        PlayerChoosesJump();

        //*************************This determines whether the character is able to move to the next cover position for running********************
        if (Input.GetKeyDown(Run) && InCover == false && !reloading &&!ChooseJump)
        {


            if (CoverPosition == 0)
            {


                CanShoot = false;
                ChooseRun = true;

            }
            else if (CoverPosition == 1)
            {
                ChooseRun = true;
                CanShoot = false;


            }
            else if (CoverPosition <= 2)
            {
                ChooseRun = true;
                CanShoot = false;


            }
            else if (CoverPosition <= 3)
            {
                CanShoot = false;
                ChooseRun = true;



            }


        }


        if (Input.GetKeyDown(Jump) && InCover == false && !reloading && !ChooseRun)
        {


            if (CoverPosition == 0)
            {


                CanShoot = false;
                ChooseJump = true;

            }
            else if (CoverPosition == 1)
            {
                ChooseJump = true;
                CanShoot = false;


            }
            else if (CoverPosition <= 2)
            {
                ChooseJump = true;
                CanShoot = false;


            }
            else if (CoverPosition <= 3)
            {
                CanShoot = false;
                ChooseJump = true;



            }


        }

        //**********************This moves the player in and out of cover**************************************
        if (Input.GetKeyDown(Peak) && InCover == true && CanShoot)
        {
            Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y + .7f);
            InCover = false;
            // Debug.Log("test1");
        }
        if (Input.GetKeyDown(Duck) && InCover == false && CanShoot)
        {
            Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
            InCover = true;

            // Debug.Log("test");
        }

        rlt1 = Time.fixedTime;
        if (Input.GetKeyDown(Reload) && InCover && !reloading)
        {
            Debug.Log("reloading");
            reloading = true;
            rlt2 = Time.fixedTime;

        }


        else if ((rlt1 - rlt2) >= ReloadTime && reloading)
        {
            Debug.Log("reloading complete");
            reloading = false;
            BulletLoaded = true;
        }

        
       
    }
    private void MuzzleFlashFunct()
    {
       

        MuzTime1 = Time.fixedTime; //Keeps track of the time
        DeadTime1 = Time.fixedTime; // ditto

        //*********************This controls the muzzle flash and controls how much time it stays on screen**********************************
        if (muz)
        {

            MuzzleFlash.SetActive(true);

            MuzTime2 = Time.fixedTime; //starts the timer that determines how much time has passed
            muz = false;
            muz2 = true;


        }
        if ((MuzTime1 - MuzTime2) > MuzzleFlashTime && muz2 == true)
        {
            MuzzleFlash.SetActive(false);
            bool HasShotP1 = (Player.gameObject.name == "Player 1" && ShotHigh); //This determine which player is shooting
            bool HasShotP2 = (Player.gameObject.name == "Player 2" && ShotHigh);
            if (HasShotP1)
            {
                Gun.transform.Rotate(new Vector3(0, 0, -45)); // Points the gun up when the player shoots for p1

                muz2 = false;
                ShotLow = false;
                ShotHigh = false;
                HasShotP1 = false;
            }
            else if (HasShotP2)
            {

                Gun.transform.Rotate(new Vector3(0, 0, 45));  // Points the gun up when the player shoots for p2
                muz2 = false;
                ShotLow = false;
                ShotHigh = false;
                HasShotP2 = false;
            }

        }
    }

    private void Aiming()
    {
        //***************************This handles whether the player is shooting high or low ****************************************************
        if (Input.GetKeyDown(ShootLow) && InCover == false && CanShoot && BulletLoaded)
        {

            BulletLoaded = false;
            DeadTime2 = Time.fixedTime; //Starts the timer for the bullet 
            ShotLow = true;
            muz = true;
        }
        if (Input.GetKeyDown(ShootHigh) && InCover == false && CanShoot && BulletLoaded)
        {
            BulletLoaded = false;
            DeadTime2 = Time.fixedTime; //Starts the timer for the bullet 
            ShotHigh = true;
            if (Player.name == "Player 1") //Determines which player gun points up when shoot high is pressed
            {
                Gun.transform.Rotate(new Vector3(0, 0, 45));
            }
            else
            {
                Gun.transform.Rotate(new Vector3(0, 0, -45));
            }

            muz = true;
        }
    }

    private void PlayerChoosesRun()
    {
        //************************This handles animating the characters when they change positions ****************************
        //moving to cover 1
        if (ChooseRun && CoverPosition == 0)
        {


            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover1.transform.position, Speed); //moves the player towards a postion

            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover1.transform.position.x && Player.transform.position.y == Cover1.transform.position.y)
            {
                //reset varibales
                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
            }

        }
        //moving to cover 2
        if (ChooseRun && CoverPosition == 1)
        {

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover2.transform.position, Speed);


            if (Player.transform.position.x == Cover2.transform.position.x && Player.transform.position.y == Cover2.transform.position.y)
            {

                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
            }

        }
        //moving to cover 3
        if (ChooseRun && CoverPosition == 2)
        {

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover3.transform.position, Speed);


            if (Player.transform.position.x == Cover3.transform.position.x && Player.transform.position.y == Cover3.transform.position.y)
            {

                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
            }

        }
        //moving to cover 4
        if (ChooseRun && CoverPosition == 3)
        {

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover4.transform.position, Speed);


            if (Player.transform.position.x == Cover4.transform.position.x && Player.transform.position.y == Cover4.transform.position.y)
            {
                Enemy.transform.position = new Vector2(1000, 1000);
                // Player.transform.position = Winner.transform.position;

                if (Player.gameObject.tag == "Player1")
                {
                    camera.transform.position = BlueWin.transform.position;
                }
                else
                {
                    camera.transform.position = RedWin.transform.position;
                }
                InCover = true;


                CanShoot = true;

                ChooseRun = false;

            }

        }
    }

    private void PlayerChoosesJump()
    {
        if (ChooseJump && CoverPosition == 0)
        {
            if (!jumping)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump1.transform.position, Speed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump1.transform.position.x && Player.transform.position.y == Jump1.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover1.transform.position, Speed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover1.transform.position.x && Player.transform.position.y == Cover1.transform.position.y)
            {
                //reset varibales
                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseJump = false;
                jumping = false;
            }

        }

        //moving to cover 2
        if (ChooseJump && CoverPosition == 1)
        {

            if (!jumping)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump2.transform.position, Speed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump2.transform.position.x && Player.transform.position.y == Jump2.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover2.transform.position, Speed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover2.transform.position.x && Player.transform.position.y == Cover2.transform.position.y)
            {
                //reset varibales
                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseJump = false;
                jumping = false;
            }

        }
        //moving to cover 3
        if (ChooseJump && CoverPosition == 2)
        {

            if (!jumping)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump3.transform.position, Speed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump3.transform.position.x && Player.transform.position.y == Jump3.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover3.transform.position, Speed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover3.transform.position.x && Player.transform.position.y == Cover3.transform.position.y)
            {
                //reset varibales
                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseJump = false;
                jumping = false;
            }

        }
        //moving to cover 4
        if (ChooseJump && CoverPosition == 3)
        {

            if (!jumping)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump4.transform.position, Speed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump4.transform.position.x && Player.transform.position.y == Jump4.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover4.transform.position, Speed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover4.transform.position.x && Player.transform.position.y == Cover4.transform.position.y)
            {
                //reset varibales
                if (Player.gameObject.tag == "Player1")
                {
                    camera.transform.position = BlueWin.transform.position;
                }
                else
                {
                    camera.transform.position = RedWin.transform.position;
                }
                Enemy.transform.position = new Vector2(1000, 1000);
                //Player.transform.position = Winner.transform.position;
                InCover = true;
                jumping = false;

                CanShoot = true;

                ChooseJump = false;

            }

        }
    }
    
}