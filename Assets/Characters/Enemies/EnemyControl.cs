using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    [SerializeField] GameObject AlienModel;
    [SerializeField] GameObject PlayerModel;
    private CharacterController AlienController;

    [SerializeField] GameObject CameraLookAt;




    // used only for debugging falling through the floor
    [SerializeField] GameObject ground;


    [SerializeField] float moveSpeed;

    [SerializeField] float attackDistance;



    private float verticalVelocity = 0f;
    private float gravityValue = -9.81f;
    [SerializeField] private float gravityMultiplier = 0.001f;

    private bool inRange;
    private bool currentlyMoving;


    private bool attacking;



    private Vector3 moveDirection;






    // measure time to limit how often the enemy can attack
    private System.DateTime startAttack;
    private System.DateTime now;

    private void Awake()
    {
        AlienController = GetComponent<CharacterController>();
        inRange = false;
        currentlyMoving = false;
        attacking = false;


        // set initial value of startAttack to avoid null reference error in checking attack animation
        startAttack = DateTime.Now;

    }

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;

        // set initial value of startAttack to avoid null reference error in checking attack animation
        startAttack = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Value of Alien.isGrounded = " + AlienController.isGrounded);


        float height = AlienController.transform.position.y - ground.transform.position.y;

        Debug.Log("Value of height is " + height);

        // apply gravity if not on ground
        // update: now applying gravity is controlled separately from horizontal movement
        // if (!AlienController.isGrounded)

        // nuclear option: no longer trust Unity to give us accurate information
        if (height > 3.5f)
        {
            Debug.Log("Applying gravity!");
            ApplyGravity();
        }



        // face the player
        AlienModel.transform.LookAt(PlayerModel.transform);
        // check if in range of player to attack
        CheckInRange();


        attacking = false;

        if (!inRange)
        {
            attacking = false;

            // Debug.Log("Not in range of player!");
            MoveToPlayer();
        }
        else
        {
            // Debug.Log("In range of player!");




            // for use in animation control
            currentlyMoving = false;

            // when in range, attack the player
            // issue command to the animation script to begin the attack animation

            // only allow attacking every 3 seconds, starting at the start of the attack animation, adjust to make feel better.




            now = DateTime.Now;

            System.TimeSpan timeElapsed = now - startAttack;

            Debug.Log("Time between attacks (timeElapsed) = " + timeElapsed.TotalSeconds);

            if (timeElapsed.TotalSeconds > 3)
            {
                // startAttack = DateTime.Now;
                attacking = true;
                startAttack = DateTime.Now;
            }
            else
            {
                attacking = false;
            }
            
        }
        
    }


    public bool IsAttacking()
    {
        return attacking;
    
    }
    public bool IsMoving()
    {
        return currentlyMoving;
    }


    private void MoveToPlayer()
    {


        // moveDirection = PlayerModel.transform.position - AlienModel.transform.position;
       

        currentlyMoving = true;
        
        // move the enemy's position towards the player
        moveDirection = AlienModel.transform.forward * moveSpeed * Time.deltaTime;
        // moveDirection.y = verticalVelocity;
        moveDirection.y = 0f;

        AlienModel.transform.position += moveDirection;
    
    }

    private void CheckInRange()
    {
        float dist = Vector3.Distance(AlienModel.transform.position, PlayerModel.transform.position);

        // Debug.Log("Distance = " + dist);
        if (dist > attackDistance)
        {
            inRange = false;
        }
        else
        {
            inRange = true;
        }
        
    }

    private void ApplyGravity()
    {
        if (AlienController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            // if (!(verticalVelocity > 5f)) { }
            verticalVelocity += gravityValue * gravityMultiplier * Time.deltaTime;
            // then, actually move the model down
            Vector3 down = new Vector3(0, verticalVelocity, 0);
            AlienModel.transform.position += down;
        }

    }
}
