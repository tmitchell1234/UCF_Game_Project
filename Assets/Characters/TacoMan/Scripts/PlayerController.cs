using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // This controller is specifically for Taco Man!

    [Header("Player Object Reference")]
    [Tooltip("For use in getting the current animation state")]
    [SerializeField] GameObject playerObjReference;
    private Animator playerAnimator;


    // reference the red hitboxes to activate during attacks
    [Tooltip("Red semi circle hitbox to display during attacks")]
    [SerializeField] GameObject redSemiCircle;

    [Tooltip("Red full circle hitbox to display during spin attack")]
    [SerializeField] GameObject redFullCircle;



    [Header("3D Movement")]
    [Tooltip("Reference to the Cinemachine FreeLook camera used for the player")]
    [SerializeField] Transform camera;

    [Tooltip("Movement variables")]
    [SerializeField] float moveSpeed = 30f;
    [SerializeField] float jumpPower;

    private float verticalVelocity = 0f;

    private bool hitJumpKey;
    private bool isMoving;


    [Tooltip("Physics variables")]
    [SerializeField] float gravityValue = -9.81f;
    [Tooltip("Adjust how fast player model falls downward")]
    [SerializeField] private float gravityMultiplier;


    private CharacterController characterController;
    [SerializeField] private GameInput gameInput;



    private Vector3 moveDirection;
    private Vector3 currentVelocity;
    private bool isGrounded;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = playerObjReference.GetComponent<Animator>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // update and check the current animation state
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);


        // activate red hitboxes immediately on mouse down to improve responsiveness
        // TODO: tweak how this works to get it to feel right
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Activating red semicricle from PlayerController.cs mouse down!");
            redSemiCircle.SetActive(true);
        }




        // stop player moving altogether if attacking (even if in midair):
        if (!(stateInfo.IsName("SwordSlash1") || stateInfo.IsName("SwordSlash2")))
        {
            // hide the red hitboxes if not attacking
            redSemiCircle.SetActive(false);
            redFullCircle.SetActive(false);

            MovePlayer();
            if (!characterController.isGrounded) ApplyGravity();
        }

        // if the player is attacking, reset all vertical velocity to 0
        // this lets the player hover in midair when attacking
        else
        {
            verticalVelocity = 0f;



            // TODO: Change this section to differentiate between normal attacks and spin attacks

            // display the red circle hitboxes when attacking
            redSemiCircle.SetActive(true);
            //redFullCircle.SetActive(true);
        }



        // if player is spin attacking, activate the red hit circle
        if (stateInfo.IsName("SpinAttack"))
        {
            Debug.Log("Activating red hit circle!");
            redFullCircle.SetActive(true);
        }

        else
        {
            redFullCircle.SetActive(false);
        }

        
        //HandleJump();
    }

    public void MovePlayer()
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

        // rotate taco man model to forward direction with smooth, interpolated motion
        transform.forward = Vector3.Slerp(transform.forward, cameraFacing, Time.deltaTime * rotateSpeed);


        currentVelocity = characterController.velocity; // for potential use in future by other systems


        // reset for animation control
        hitJumpKey = false;

        //  set class variable to adjust animation state
        isMoving = (currentVelocity != Vector3.zero);
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
            if (!(verticalVelocity > 25f)) { }
                // verticalVelocity += gravityValue * gravityMultiplier * Time.deltaTime;
        }
    }


    // called from PlayerInput object in scene objects hierarchy, under GAME > InputManagers
    public void HandleJump(InputAction.CallbackContext context)
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
        Debug.Log("Setting hitJumpKey to " + hitJumpKey);
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
        if (hitJumpKey == true) Debug.Log("Inside HitJumpKey(), returning " + hitJumpKey);
        // else Debug.Log("Inside HitJumpKey(), returning " + hitJumpKey);
        return hitJumpKey;
    }
}
