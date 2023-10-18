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


    // used to keep track of time in seconds passed to control multi-attack moves
    private bool clickActive;
    private System.DateTime startTime;
    private System.DateTime currentTime;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_MOVING, false);
        clickActive = false;
    }



    // Update is called once per frame
    void Update()
    {
        currentTime = System.DateTime.Now;


        animator.SetBool(IS_MOVING, playerControllerScript.IsMoving());
        animator.SetBool(HIT_JUMP_KEY, playerControllerScript.HitJumpKey());
        animator.SetBool(IS_FALLING, playerControllerScript.IsFalling());

        // attack animation
        // first, detect if mouse button has been clicked
        if (!clickActive)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Debug.Log("Clicked mouse!");


                animator.SetTrigger(MOUSE_CLICKED);

                clickActive = true;
                startTime = System.DateTime.Now;
            }
        }
        else // click is active, activate second attack if applicable and expire both after 0.5 seconds
        {
            System.TimeSpan timeSpan = currentTime - startTime;

            if (timeSpan.TotalSeconds > 0.5)
            {
                clickActive = false;
                animator.ResetTrigger(MOUSE_CLICKED);
            }
        }
        
            
    }






    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
