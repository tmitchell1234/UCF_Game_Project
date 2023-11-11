using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] Transform camera;



    // get player model to reference the animator controller
    [SerializeField] GameObject playerModel;
    private Animator playerAnimator;




    private CharacterController characterController;


    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private bool isMoving;

    private bool isAttacking = false;

    private bool hitJumpKey;

    private bool isGrounded;
    [SerializeField] private float jumpPower;
    private float gravityValue = -9.81f;
    [SerializeField] private float gravityMultiplier = 0.001f;
    private float verticalVelocity = 0f;



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        // playerModel.GetComponent<NinjaAnimatorController>().getAttackingState();
    }


    // Update is called once per frame
    void Update()
    {
        // isAttacking = animatorController.getAttackingState();

        // do not allow player to move while attacking (even if in the air)
        if (playerModel.GetComponent<NinjaAnimatorController>().isAttacking())
        {
            Debug.Log("Attacking!");
            verticalVelocity = 0f;
            characterController.Move(new Vector3(0, 0, 0));
        }
        else
        {
            if (!characterController.isGrounded) ApplyGravity();
            MovePlayer();
        }

        // MovementJump();

        // characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        //Debug.Log("Current velocity: " + currentVelocity);

    }


    void MovePlayer()
    {
        // player inputs
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // camera direction
        Vector3 cameraForward = camera.forward;
        Vector3 cameraRight = camera.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        // create relative camera direction
        Vector3 forwardRelative = inputVector.y * cameraForward;
        Vector3 rightRelative = inputVector.x * cameraRight;

        moveDirection = forwardRelative + rightRelative;

        // apply vertical velocity (affected by gravity)
        moveDirection.y = verticalVelocity;

        // new method using Character Controller
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        float rotateSpeed = 15f;
        Vector3 cameraFacing = moveDirection;
        cameraFacing.y = 0;
        transform.forward = Vector3.Slerp(transform.forward, cameraFacing, Time.deltaTime * rotateSpeed);


        currentVelocity = characterController.velocity;
        isMoving = (currentVelocity != Vector3.zero);
        hitJumpKey = false; // reset for animation
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
            if (!(verticalVelocity > 25f))
                verticalVelocity += gravityValue * gravityMultiplier * Time.deltaTime;
        }
        
    }


    public void MovementJump(InputAction.CallbackContext context)
    {
        // only do jump on key down
        if (!context.started) return;

        isGrounded = characterController.isGrounded;

        // if player is not on ground, do not allow jump
        if (!isGrounded)
        {
            // todo: implement jump limits, if wanted
        }

        // reset vertical velocity
        verticalVelocity = 0f;
        verticalVelocity += jumpPower;

        hitJumpKey = true;
    } 


    public bool IsMoving()
    {
        return isMoving;
    }

    public bool IsFalling()
    {
        return !characterController.isGrounded;
    }

    public bool HitJumpKey()
    {
        return hitJumpKey;
    }




    // Start is called before the first frame update
    void Start()
    {

    }

}
