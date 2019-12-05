using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class CanyonPlayerScript : MonoBehaviour
{
    //Controls
    public KeyCode Jump = KeyCode.D;
    public KeyCode Run = KeyCode.C;
    public KeyCode Duck = KeyCode.S; //Hide behind boxes
    public KeyCode Peak = KeyCode.W; //Peak Out from cover
   
    public KeyCode ShootHigh = KeyCode.F; // Only kills jumping characters
    public KeyCode ShootLow = KeyCode.E; //Kills a running player or when a player is peaking
    public KeyCode Reload = KeyCode.R;
    //    public KeyCode Menu = KeyCode.Escape;

    private int CoverPosition = 0; // Determines what cover position the player is in
    public float Speed = 0.5f; // How fast the character moves
    public GameObject Cover1;  //First Position
    public GameObject Cover2; //Second Position
    public GameObject Cover3; // Third Position
    public GameObject Cover4; //Fourth Position
    public GameObject Cover5;
    public GameObject Cover6;
    public GameObject Jump1;
    public GameObject Jump2;
    public GameObject Jump3;
    public GameObject Jump4;
    public GameObject Jump5;
    public GameObject Jump6;

    public Camera camera;
    public GameObject BlueWin;
    public GameObject RedWin;

    public bool hasWon;
    public GameObject Player; //Tells who the player is

    public GameObject Enemy; // Tells who the enemy is
    public GameObject Winner; //The winners platform
    public GameObject Spawn; //Spawn point of players
   


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
    public GameObject PauseMenu;
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
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        InCover = true; //Players start in cover
        MuzzleFlash.SetActive(false);
        CanShoot = true;
        BulletLoaded = true;
        reloading = false;
        hasWon = false;



    }

    // Update is called once per frame
    void Update()
    {
     
           isPaused = PauseMenu.GetComponent<PauseMenuScript>().isPaused;

      

        EnemyCover = Enemy.GetComponent<CanyonPlayerScript>().InCover; // checks if the enemy is in cover
        MuzzleFlashFunct(); //Function that handles muzzle flashes
        //***************This makes the player pull out their gun when comming out of cover***************************
       
        Aiming();
       


        //****************************This handles when a player hits another player ******************************

        if (((DeadTime1 - DeadTime2) > BulletHitTime) && ShotLow && !EnemyCover && !Enemy.GetComponent<CanyonPlayerScript>().ChooseJump) //Determines the delay between bullet firing and hitting target
        {


            Enemy.transform.position = Enemy.GetComponent<CanyonPlayerScript>().Spawn.transform.position; //Respawns the hit player
            //resets variables
            Enemy.GetComponent<CanyonPlayerScript>().CoverPosition = 0;
            Enemy.GetComponent<CanyonPlayerScript>().CanShoot = true;
            Enemy.GetComponent<CanyonPlayerScript>().InCover = true;
            Enemy.GetComponent<CanyonPlayerScript>().ChooseRun = false;
            Enemy.GetComponent<CanyonPlayerScript>().jumping = false;
            Enemy.GetComponent<CanyonPlayerScript>().ChooseJump = false;
        }
        else if (((DeadTime1 - DeadTime2) > BulletMissTime)) //This means the bullet has passed and wont hit the player
        {
            ShotLow = false;
        }
        //This just does the same thing as the statement above but for when the player jumps
        if (((DeadTime1 - DeadTime2) > BulletHitTime) & ShotHigh && !EnemyCover && Enemy.GetComponent<CanyonPlayerScript>().ChooseJump)
        {

            Enemy.transform.position = Enemy.GetComponent<CanyonPlayerScript>().Spawn.transform.position; //respawns enemy
            //reset variables
            Enemy.GetComponent<CanyonPlayerScript>().CoverPosition = 0;
            Enemy.GetComponent<CanyonPlayerScript>().jumping = false;
            Enemy.GetComponent<CanyonPlayerScript>().CanShoot = true;
            Enemy.GetComponent<CanyonPlayerScript>().InCover = true;
            Enemy.GetComponent<CanyonPlayerScript>().ChooseRun = false;
            Enemy.GetComponent<CanyonPlayerScript>().ChooseJump = false;


        }
        else if (((DeadTime1 - DeadTime2) > 0.6f))
        {
            ShotHigh = false;
        }

        PlayerChoosesRun();

        PlayerChoosesJump();

        //*************************This determines whether the character is able to move to the next cover position for running********************
        if (Input.GetKeyDown(Run) && InCover == false && !reloading && !ChooseJump)
        {


            if (CoverPosition == 0)
            {

                Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
                CanShoot = false;
                ChooseRun = true;

            }
            else if (CoverPosition == 1)
            {
                Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
                ChooseRun = true;
                CanShoot = false;


            }
            else if (CoverPosition <= 2)
            {
                Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
                ChooseRun = true;
                CanShoot = false;


            }
            else if (CoverPosition <= 3)
            {
                Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
                CanShoot = false;
                ChooseRun = true;



            }
            else if (CoverPosition <= 4)
            {
                Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
                CanShoot = false;
                ChooseRun = true;



            }
            else if (CoverPosition <= 5)
            {
                Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
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
            else if (CoverPosition <= 4)
            {
                CanShoot = false;
                ChooseJump = true;



            }
            else if (CoverPosition <= 5)
            {
                CanShoot = false;
                ChooseJump = true;



            }


        }

        //**********************This moves the player in and out of cover**************************************
        if (Input.GetKeyDown(Peak) && InCover == true && CanShoot && !isPaused)
        {
            Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y + .7f);
            InCover = false;
            // Debug.Log("test1");
        }
        if (Input.GetKeyDown(Duck) && InCover == false && CanShoot && !isPaused)
        {
            Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
            InCover = true;

            // Debug.Log("test");
        }

        rlt1 = Time.fixedTime;
        if (Input.GetKeyDown(Reload) && InCover && !reloading && !isPaused)
        {
            Debug.Log("reloading");
            reloading = true;
            rlt2 = Time.fixedTime;

        }


        else if ((rlt1 - rlt2) >= ReloadTime && reloading && !isPaused)
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
                

                muz2 = false;
                ShotLow = false;
                ShotHigh = false;
                HasShotP1 = false;
            }
            else if (HasShotP2)
            {

                
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
        if (Input.GetKeyDown(ShootLow) && InCover == false && CanShoot && BulletLoaded && !isPaused)
        {

            BulletLoaded = false;
            DeadTime2 = Time.fixedTime; //Starts the timer for the bullet 
            ShotLow = true;
            muz = true;
        }
        if (Input.GetKeyDown(ShootHigh) && InCover == false && CanShoot && BulletLoaded && !isPaused)
        {
            BulletLoaded = false;
            DeadTime2 = Time.fixedTime; //Starts the timer for the bullet 
            ShotHigh = true;
           

            muz = true;
        }
    }

    private void PlayerChoosesRun()
    {
        //************************This handles animating the characters when they change positions ****************************
        //moving to cover 1
        if (ChooseRun && CoverPosition == 0 && !isPaused)
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
        if (ChooseRun && CoverPosition == 1 &&!isPaused)
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
        if (ChooseRun && CoverPosition == 2 && !isPaused)
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
        if (ChooseRun && CoverPosition == 3 && !isPaused)
        {

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover4.transform.position, Speed);


            if (Player.transform.position.x == Cover4.transform.position.x && Player.transform.position.y == Cover4.transform.position.y)
            {

                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
            }

        }
        if (ChooseRun && CoverPosition == 4 && !isPaused)
        {

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover5.transform.position, Speed);


            if (Player.transform.position.x == Cover5.transform.position.x && Player.transform.position.y == Cover5.transform.position.y)
            {

                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
            }

        }
        //moving to cover 4
        if (ChooseRun && CoverPosition == 5 && !isPaused)
        {

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover6.transform.position, Speed);


            if (Player.transform.position.x == Cover6.transform.position.x && Player.transform.position.y == Cover6.transform.position.y)
            {
                Enemy.transform.position = new Vector2(1000, 1000);
                // Player.transform.position = Winner.transform.position;

                hasWon = true;
                InCover = true;


                CanShoot = true;

                ChooseRun = false;

            }

        }
    }

    private void PlayerChoosesJump()
    {
        if (ChooseJump && CoverPosition == 0 && !isPaused)
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
        if (ChooseJump && CoverPosition == 1 && !isPaused)
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
        if (ChooseJump && CoverPosition == 2 && !isPaused)
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
        if (ChooseJump && CoverPosition == 3 && !isPaused)
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
                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseJump = false;
                jumping = false;
            }

        }
        if (ChooseJump && CoverPosition == 4 && !isPaused)
        {

            if (!jumping)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump5.transform.position, Speed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump5.transform.position.x && Player.transform.position.y == Jump5.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover5.transform.position, Speed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover5.transform.position.x && Player.transform.position.y == Cover5.transform.position.y)
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
        if (ChooseJump && CoverPosition == 5 && !isPaused)
        {

            if (!jumping)
            {
                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump6.transform.position, Speed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump6.transform.position.x && Player.transform.position.y == Jump6.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover6.transform.position, Speed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover6.transform.position.x && Player.transform.position.y == Cover6.transform.position.y)
            {
                //reset varibales

                hasWon = true;
                //Player.transform.position = Winner.transform.position;
                InCover = true;
                jumping = false;

                CanShoot = true;

                ChooseJump = false;

            }

        }
    }

}
