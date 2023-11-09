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



    private Vector3 moveDirection;

    private void Awake()
    {
        AlienController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
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
        if (height > 5.2f)
        {
            Debug.Log("Applying gravity!");
            ApplyGravity();
        }



        // face the player
        AlienModel.transform.LookAt(PlayerModel.transform);
        // check if in range of player to attack
        CheckInRange();
        if (!inRange)
        {
            // Debug.Log("Not in range of player!");
            MoveToPlayer();
        }
        else
        {
            // Debug.Log("In range of player!");
        }
        
    }





    private void MoveToPlayer()
    {


        // moveDirection = PlayerModel.transform.position - AlienModel.transform.position;



        
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
