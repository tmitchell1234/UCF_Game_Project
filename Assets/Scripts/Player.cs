using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 30f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] Transform camera;


    private CharacterController characterController;




    private Vector2 playerMovementInput;
    private Vector3 moveDirection;



    private bool isMoving;



    private bool isGrounded;
    [SerializeField] private float jumpPower;
    private bool jumpPressed = false;
    private float gravityValue = -9.81f;
    [SerializeField] private float gravityMultiplier = 0.0000001f;
    private float verticalVelocity = 0f;

    // Update is called once per frame
    void Update()
    {
        if (!characterController.isGrounded) ApplyGravity();
        MovePlayer();
        // MovementJump();

        // characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        return;

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

        // Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
        Vector3 moveDirection = forwardRelative + rightRelative;

        // apply vertical velocity (affected by gravity)
        moveDirection.y = verticalVelocity;

        // new method using Character Controller
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

        isMoving = (moveDirection != Vector3.zero);

        float rotateSpeed = 15f;
        Vector3 cameraFacing = moveDirection;
        cameraFacing.y = 0;
        //transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        transform.forward = Vector3.Slerp(transform.forward, cameraFacing, Time.deltaTime * rotateSpeed);

    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = 0f;
        }
        else
        {
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

        
    } 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

}
