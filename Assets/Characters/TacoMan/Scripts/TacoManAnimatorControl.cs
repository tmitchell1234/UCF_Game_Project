using RPGCharacterAnims.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacoManAnimatorControl : MonoBehaviour
{
    // Class to help handle animation states and transitions for Taco Man



    private Animator animator;
    [SerializeField] private PlayerController playerControllerScript;





    private const string IS_MOVING = "IsMoving";
    private const string IS_FALLING = "IsFalling";
    private const string HIT_JUMP_KEY = "HitJumpKey";
    private const string MOUSE_CLICKED = "MouseClicked";
    private const string DOUBLECLICK = "DoubleClicked";
    private const string HOLDINGT = "HoldingT";
    private const string HOLDINGCLICK = "HoldingClick";
    private const string HITSHIFT = "HitShift";



    // used to keep track of time in seconds passed to control multi-attack moves
    private bool clickActive;
    
    private System.DateTime clickTimerStart;
    private System.DateTime clickTimerCurrent;

    private bool jumpActive; // used to fix jump animation activation
    private System.DateTime spaceTimerStart;
    private System.DateTime spaceTimerCurrent;




    // used to track if T and mouse held down for spin attack
    private bool holdingT;
    private bool holdingClick;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_MOVING, false);
        clickActive = false;
        jumpActive = false;
        holdingT = false;
        holdingClick = false;
    }



    // Update is called once per frame
    void Update()
    {
        // if dash attacking, handle this first thing to make the animations and controls feel more responsive
        bool isDashing = playerControllerScript.IsDashing();

        if (isDashing)
        {
            animator.SetBool(HITSHIFT, true);
        }
        else
        {
            animator.SetBool(HITSHIFT, false);
        }



        clickTimerCurrent = System.DateTime.Now;
        spaceTimerCurrent = System.DateTime.Now;

        


        animator.SetBool(IS_MOVING, playerControllerScript.IsMoving());
        animator.SetBool(IS_FALLING, playerControllerScript.IsFalling());




        // if (playerControllerScript.HitJumpKey()) Debug.Log("Inside TacoManAnimatorControl.cs: HitJumpKey = true!");
        // animator.SetBool(HIT_JUMP_KEY, playerControllerScript.HitJumpKey());
        



        // jump animation
        if (!jumpActive)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Debug.Log("Hit space bar!");
                animator.SetBool(HIT_JUMP_KEY, true);
                jumpActive = true;
                spaceTimerStart = System.DateTime.Now;
            }
        }
        else if (jumpActive)
        {
            // expire jump active trigger after 0.3 seconds (possibly readjust this time to change feel of multijump)
            System.TimeSpan timeSpan = spaceTimerCurrent - spaceTimerStart;

            if (timeSpan.TotalSeconds > 0.2)
            {
                // Debug.Log("Setting HitJumpKey to false!");
                jumpActive = false;
                animator.SetBool(HIT_JUMP_KEY, false);
            }
        }



        // check for spin attack input first, then check for a mouse click
        if (Input.GetKey(KeyCode.T))
        {
            holdingT = true;
            animator.SetBool(HOLDINGT, true);


            if (Input.GetKey(KeyCode.Mouse0))
            {
                holdingClick = true;
                animator.SetBool(HOLDINGCLICK, true);
            }
        }

        // else if the player is not holding down T, play the regular attack animation

        else
        {
            // allow player to keep spin attacking if T is let go of but mouse is still held down
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack"))
            {
                if (Input.GetKey(KeyCode.Mouse0))
                    return;
            }


            // set the holding T and holding click booleans to false
            holdingT = false;
            animator.SetBool(HOLDINGT, false);
            holdingClick = false;
            animator.SetBool(HOLDINGCLICK, false);


            // attack animation
            // first, detect if mouse button has been clicked
            if (!clickActive)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // Debug.Log("Clicked mouse!");


                    animator.SetBool(MOUSE_CLICKED, true);

                    clickActive = true;
                    clickTimerStart = System.DateTime.Now;
                }
            }
            else if (clickActive)// click is active, activate second attack if applicable and expire both after 0.3 seconds
            {
                System.TimeSpan timeSpan = clickTimerCurrent - clickTimerStart;

                if (timeSpan.TotalSeconds > 0.3)
                {
                   //  Debug.Log("Deactivating clickActive in TacoManAnimatorControl.cs");
                    clickActive = false;
                    animator.SetBool(MOUSE_CLICKED, false);
                    animator.SetBool(DOUBLECLICK, false);
                }
                else if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    // Debug.Log("Registered DoubleClick!");
                    animator.SetBool(DOUBLECLICK, true);
                }
            }
        }
        
        
            
    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
