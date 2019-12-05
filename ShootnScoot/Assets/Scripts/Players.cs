using System.Collections;
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
    public float RunSpeed =0.25f;
    public float JumpSpeed =.25f;// How fast the character moves
    public GameObject Cover1;  //First Position
    public GameObject Cover2; //Second Position
    public GameObject Cover3; // Third Position
    public GameObject Cover4; //Fourth Position
    public GameObject Jump1;
    public GameObject Jump2;
    public GameObject Jump3;
    public GameObject Jump4;
    public Camera camera;
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

    public float MuzzleFlashTime = 0.1f; // Determines how long the muzzle flash stays on screen
    public float BulletHitTime = 0.1f; //Determines the delay from when player shoots to when it hits
    public float BulletMissTime = 0.6f; //Determines how long befoe the enemy is safe
    public float ReloadTime = 1f;
    private float rlt1;
    private float rlt2;
    private bool BulletLoaded;
    //private bool enemydead;
  //  public GameObject MuzzleFlash; //the muzzle flash game object
    private bool EnemyCover; //Determines if the enemy is in cover
    private bool CanShoot; //Determines if the player has the ability to shoot
    private bool reloading;
    private bool muz; // keeps trakc if muzzle flash is active
    private bool muz2;
    public bool isPaused;
    private bool InCover; //Determines whether the player is in cover or not
    public GameObject Perks;
    // Start is called before the first frame update
    void Start()
    {
        InCover = false; //Players start in cover
      //  MuzzleFlash.SetActive(false); 
        CanShoot = true;
        BulletLoaded = true;
        reloading = false;
        hasWon = false;
        animator = GetComponent<Animator>();

    }

    private AnimationState animationState = AnimationState.NONE;
    private Animator animator;

    public enum AnimationState
    {
        NONE = 0,
        IDLE = 1,
        RUNNING = 2,
        JUMP = 3,
        CROUCH =4,
        SHOOTLOW =5
        
    };

    // Update is called once per frame
    void Update()
    {
        if(Player.name=="Player 1")
        {
            RunSpeed = Perks.GetComponent<PerkScript>().Player1RunSpeed;
            JumpSpeed = Perks.GetComponent<PerkScript>().Player1JumpSpeed;
            ReloadTime = Perks.GetComponent<PerkScript>().Player1ReloadSpeed;
        }
        else
        {
            RunSpeed = Perks.GetComponent<PerkScript>().Player2RunSpeed;
            JumpSpeed = Perks.GetComponent<PerkScript>().Player2JumpSpeed;
            ReloadTime = Perks.GetComponent<PerkScript>().Player2ReloadSpeed;
        }

        switch (animationState)
        {
            case AnimationState.NONE:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isCrouch", false);
                animator.SetBool("ShootLow", false);
                break;
            case AnimationState.IDLE:
                animator.SetBool("isIdle", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isCrouch", false);
                animator.SetBool("ShootLow", false);

                break;
            case AnimationState.RUNNING:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", true);
                animator.SetBool("isJumping", false);
                animator.SetBool("isCrouch", false);
                animator.SetBool("ShootLow", false);

                break;
            case AnimationState.JUMP:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", true);
                animator.SetBool("isCrouch", false);
                animator.SetBool("ShootLow", false);

                break;
            case AnimationState.CROUCH:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isCrouch", true);
                animator.SetBool("ShootLow", false);
                break;
            case AnimationState.SHOOTLOW:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                animator.SetBool("isCrouch", false);
                animator.SetBool("ShootLow", true);
                break;



            default:
                break;
        }

        if (Player.gameObject.name == "Player 1")
        {
            isPaused = GetComponent<PauseMenuScript>().isPaused;
            
        }
        else
        {
            isPaused = Enemy.GetComponent<PauseMenuScript>().isPaused;
        }

        EnemyCover = Enemy.GetComponent<Players>().InCover; // checks if the enemy is in cover
        MuzzleFlashFunct(); //Function that handles muzzle flashes
        //***************This makes the player pull out their gun when comming out of cover***************************
        if (InCover)

        {

          //  Gun.SetActive(false);
        }
        else
        {
           // Gun.SetActive(true);
        }
        if(InCover && !ChooseJump && !ChooseRun)
        {
            animationState = AnimationState.CROUCH;
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
            InCover = false;
            
        }

        

        //****************************This handles when a player hits another player ******************************

        if (((DeadTime1 - DeadTime2) > BulletHitTime) && ShotLow && !EnemyCover && !Enemy.GetComponent<Players>().ChooseJump) //Determines the delay between bullet firing and hitting target
        {


            Enemy.transform.position = Enemy.GetComponent<Players>().Spawn.transform.position; //Respawns the hit player
            //resets variables
            Enemy.GetComponent<Players>().CoverPosition = 0;
            Enemy.GetComponent<Players>().CanShoot = true;
            Enemy.GetComponent<Players>().InCover = false;
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
        if (Input.GetKeyDown(Run) && InCover == false && !reloading &&!ChooseJump && !isPaused)
        {


            if (CoverPosition == 0)
            {

                
                CanShoot = false;
                ChooseRun = true;
               // Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);

            }
            else if (CoverPosition == 1)
            {
                ChooseRun = true;
                CanShoot = false;
               // Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);

            }
            else if (CoverPosition <= 2)
            {
                ChooseRun = true;
                CanShoot = false;
              //  Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);

            }
            else if (CoverPosition <= 3)
            {
                CanShoot = false;
                ChooseRun = true;
               // Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);



            }


        }


        if (Input.GetKeyDown(Jump) && InCover == false && !reloading && !ChooseRun && !isPaused)
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
        if (Input.GetKeyDown(Peak) && InCover == true && CanShoot && !isPaused)
        {
          //  Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y + .7f);
            InCover = false;
            // Debug.Log("test1");
            animationState = AnimationState.IDLE;
        }
        if (Input.GetKeyDown(Duck) && InCover == false && CanShoot && !isPaused)
        {
          // Player.transform.position = new Vector2(Player.transform.position.x, Player.transform.position.y - .7f);
            InCover = true;
            //animationState = AnimationState.Crouch;
            // Debug.Log("test");
        }
        
       

        rlt1 = Time.fixedTime;
        if (Input.GetKeyDown(Reload) && InCover && !reloading && !isPaused && !BulletLoaded)
        {
            Debug.Log("reloading");
            reloading = true;
            rlt2 = Time.fixedTime;

        }


        else if ((rlt1 - rlt2) >= ReloadTime && reloading &&!BulletLoaded)
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

           // MuzzleFlash.SetActive(true);

            MuzTime2 = Time.fixedTime; //starts the timer that determines how much time has passed
            muz = false;
            muz2 = true;


        }
        if ((MuzTime1 - MuzTime2) > MuzzleFlashTime && muz2 == true)
        {
           // MuzzleFlash.SetActive(false);
            bool HasShotP1 = (Player.gameObject.name == "Player 1" && ShotHigh); //This determine which player is shooting
            bool HasShotP2 = (Player.gameObject.name == "Player 2" && ShotHigh);
             animationState = AnimationState.IDLE;
            
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
        if (Input.GetKeyDown(ShootLow) && InCover == false && CanShoot && BulletLoaded &&!isPaused)
        {

            BulletLoaded = false;
            DeadTime2 = Time.fixedTime; //Starts the timer for the bullet 
            ShotLow = true;
            muz = true;
            animationState = AnimationState.SHOOTLOW;
            // animationState = AnimationState.IDLE;
            
        }
        if (Input.GetKeyDown(ShootHigh) && InCover == false && CanShoot && BulletLoaded &&!isPaused)
        {
            BulletLoaded = false;
            DeadTime2 = Time.fixedTime; //Starts the timer for the bullet 
            ShotHigh = true;
            animationState = AnimationState.SHOOTLOW;

            muz = true;
        }
        
    }

    private void PlayerChoosesRun()
    {
        //************************This handles animating the characters when they change positions ****************************
        //moving to cover 1
        if (ChooseRun && CoverPosition == 0)
        {
            animationState = AnimationState.RUNNING;

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover1.transform.position, RunSpeed); //moves the player towards a postion

            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover1.transform.position.x && Player.transform.position.y == Cover1.transform.position.y)
            {
                //reset varibales
                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
                animationState = AnimationState.IDLE;

            }

        }
        //moving to cover 2
        if (ChooseRun && CoverPosition == 1)
        {
            animationState = AnimationState.RUNNING;

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover2.transform.position,RunSpeed);


            if (Player.transform.position.x == Cover2.transform.position.x && Player.transform.position.y == Cover2.transform.position.y)
            {

                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
                animationState = AnimationState.IDLE;

            }

        }
        //moving to cover 3
        if (ChooseRun && CoverPosition == 2)
        {
            animationState = AnimationState.RUNNING;

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover3.transform.position, RunSpeed);


            if (Player.transform.position.x == Cover3.transform.position.x && Player.transform.position.y == Cover3.transform.position.y)
            {

                InCover = true;
                ++CoverPosition;
                CanShoot = true;
                ChooseRun = false;
                animationState = AnimationState.IDLE;

            }

        }
        //moving to cover 4
        if (ChooseRun && CoverPosition == 3)
        {
            animationState = AnimationState.RUNNING;

            Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover4.transform.position, RunSpeed);


            if (Player.transform.position.x == Cover4.transform.position.x && Player.transform.position.y == Cover4.transform.position.y)
            {
               
                // Player.transform.position = Winner.transform.position;
                hasWon = true;
                /* if (Player.gameObject.tag == "Player 1")
                 {
                     hasWon = true;
                 }
                 else
                 {

                 }*/
                InCover = false ;


                CanShoot = true;

                ChooseRun = false;
                animationState = AnimationState.IDLE;


            }

        }
    }

    private void PlayerChoosesJump()
    {
        if (ChooseJump && CoverPosition == 0)
        {
            if (!jumping)
            {
                animationState = AnimationState.JUMP;

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump1.transform.position, JumpSpeed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump1.transform.position.x && Player.transform.position.y == Jump1.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover1.transform.position, JumpSpeed);

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

                animationState = AnimationState.IDLE;

            }

        }

        //moving to cover 2
        if (ChooseJump && CoverPosition == 1)
        {

            if (!jumping)
            {
                animationState = AnimationState.JUMP;

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump2.transform.position, JumpSpeed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump2.transform.position.x && Player.transform.position.y == Jump2.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover2.transform.position, JumpSpeed);

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
                animationState = AnimationState.IDLE;

            }

        }
        //moving to cover 3
        if (ChooseJump && CoverPosition == 2)
        {

            if (!jumping)
            {
                animationState = AnimationState.JUMP;

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump3.transform.position, JumpSpeed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump3.transform.position.x && Player.transform.position.y == Jump3.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover3.transform.position, JumpSpeed);

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
                animationState = AnimationState.IDLE;

            }

        }
        //moving to cover 4
        if (ChooseJump && CoverPosition == 3)
        {

            if (!jumping)
            {
                animationState = AnimationState.JUMP;

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Jump4.transform.position, JumpSpeed); //moves the player towards a postion
            }
            if (Player.transform.position.x == Jump4.transform.position.x && Player.transform.position.y == Jump4.transform.position.y)
            {

                jumping = true;
            }
            if (jumping)
            {

                Player.transform.position = Vector2.MoveTowards(Player.transform.position, Cover4.transform.position, JumpSpeed);

            }
            //this checks of the player has reached the next position
            if (Player.transform.position.x == Cover4.transform.position.x && Player.transform.position.y == Cover4.transform.position.y)
            {
                hasWon = true;
                //reset varibales
                if (Player.gameObject.tag == "Player 1")
                {
                    
                }
                else
                {
                    
                }
                
                //Player.transform.position = Winner.transform.position;
                InCover = true;
                jumping = false;

                CanShoot = true;

                ChooseJump = false;
                animationState = AnimationState.IDLE;


            }

        }
    }
    
}
