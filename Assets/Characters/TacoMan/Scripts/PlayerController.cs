using CodeMonkey.HealthSystemCM;
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

    private Vector3 moveDirection;
    private Vector3 currentVelocity;

    private bool isGrounded;





    private CharacterController characterController;
    [SerializeField] private GameInput gameInput;


    



    // hitboxes and hurtboxes

    [Header("Hitboxes and hurtboxes")]
    [Tooltip("The hitbox for regular sword slashes")]
    [SerializeField] GameObject swordHitbox;
    [Tooltip("The hitbox for spin attack")]
    [SerializeField] GameObject spinHitbox;

    // set dashing status for the animator
    private bool isDashing;

    [Tooltip("The hitbox for dash attack")]
    [SerializeField] GameObject dashHitbox;






    // Player health system (CodeMonkey module)
    HealthSystem playerHealthSystem;
    private bool isDead;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = playerObjReference.GetComponent<Animator>();

        // give player 100 health, adjust damage accordingly
        playerHealthSystem = new HealthSystem(100);
        playerHealthSystem.OnDead += PlayerHealthSystem_OnDead;
    }




    private void PlayerHealthSystem_OnDead(object sender, System.EventArgs e)
    {
        // TODO: Implement how to handle player death here
        Debug.Log("PLAYER DIED!");
        isDead = true;
    }





    // Start is called before the first frame update
    void Start()
    {
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        // update and check the current animation state
        AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);



        // if the player is dash attacking, don't do anything else until the animation is over
        if (stateInfo.IsName("DashAttack")) return;

        else
        {
            // when the dash attack is over, turn off the hitbox
            dashHitbox.SetActive(false);
            isDashing = false;
        }
        


        // activate red hitboxes immediately on mouse down to improve responsiveness
        // TODO: tweak how this works to get it to feel right
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // Debug.Log("Activating red semicricle from PlayerController.cs mouse down!");
            redSemiCircle.SetActive(true);
        }




        // stop player moving altogether if attacking (even if in midair):
        if (!(stateInfo.IsName("SwordSlash1") || stateInfo.IsName("SwordSlash2") || stateInfo.IsName("SpinAttack")))
        {
            // hide the red hitboxes if not attacking
            redSemiCircle.SetActive(false);
            redFullCircle.SetActive(false);

            swordHitbox.SetActive(false);
            spinHitbox.SetActive(false);


            
            MovePlayer();
            if (!characterController.isGrounded) ApplyGravity();
            
        }

        
        else
        {
            // TODO: Change this section to differentiate between normal attacks and spin attacks


            // if regular sword slashing, activate the player's sword hitbox
            if (stateInfo.IsName("SwordSlash1") || stateInfo.IsName("SwordSlash2"))
            {
                // if the player is attacking, reset all vertical velocity to 0
                // this lets the player hover in midair when attacking

                // only apply when sword slashing, we want to let the player jump and move around when spin attacking
                verticalVelocity = 0f;

                swordHitbox.SetActive(true);
                redSemiCircle.SetActive(true);
            }
            // if spin attacking, activate the player's spin attack hitbox
            else if (stateInfo.IsName("SpinAttack"))
            {
                // TODO: Deactivate, then reactivate the spin hit box when the player holds down mouse so enemies can be continuously hit

                spinHitbox.SetActive(true);

                // allow the player to continue to move while in spin attack animation
                MovePlayer();
                if (!characterController.isGrounded) ApplyGravity();
            }
            else
            {
                swordHitbox.SetActive(false);
                spinHitbox.SetActive(false);
            }

            // display the red circle hitboxes when attacking
            // redSemiCircle.SetActive(true);
            //redFullCircle.SetActive(true);
        }



        // if player is spin attacking, activate the red hit circle
        /*
        if (stateInfo.IsName("SpinAttack"))
        {
            //Debug.Log("Activating red hit circle!");
            //redFullCircle.SetActive(true);
        }

        else
        {
            //redFullCircle.SetActive(false);
        }
        */

        
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




    // deal damage to Taco Man when hit
    private void OnCollisionEnter(Collision collider)
    {
        Debug.Log("Player hit by: " + collider.gameObject.tag);

        // if hit by alien slap, take 10 damage
        if (collider.gameObject.tag == "AlienSlapbox")
        {
            Debug.Log("10 damage from AlienSlapbox");
            playerHealthSystem.Damage(10);
        }
        
        // take 25 damage from boss slash
        if (collider.gameObject.tag == "SlashHitbox")
        {
            Debug.Log("Taking 25 damage from SlashHitbox");
            playerHealthSystem.Damage(25);
        }

        // take 35 damage from boss slam
        if (collider.gameObject.tag == "SlamHitbox")
        {
            Debug.Log("35 damage from SlamHitbox");
            playerHealthSystem.Damage(35);
        }
    }

    public void Heal(float healAmount)
    {
        Debug.Log("Healing player for " + healAmount);
        playerHealthSystem.Heal(healAmount);
    }




    public bool IsDashing()
    {
        return isDashing;
    }
    public void HandleDashAttack(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        Debug.Log("Hit shift!");

        isDashing = true;

        // activate the dashing hitboxes
        dashHitbox.SetActive(true);

        // reset the player's vertical velocity
        verticalVelocity = 0f;

        // teleport the player forward, in the direction the camera is facing
        Vector3 cameraPosition = camera.position;

        float distance = 15f;

        Vector3 destination = cameraPosition - playerObjReference.transform.position;
        // destination *= distance;


        // determine if the camera is being pointed upward or downward
        float pitchAngle = camera.transform.eulerAngles.x;

        Debug.Log("pitchangle = " + pitchAngle);

        // flip the sign of x and z values (but not y value) to correct the orientation
        destination.x *= -distance;
        destination.z *= -distance;


        // can't quite get the Y position to work well... for now, just keep the player on the same level vertically
        destination.y = playerObjReference.transform.position.y;

        Debug.Log("destination = " + destination);
        transform.position += destination;

        Debug.Log("Player new position = " + transform.position);
        
    }



    // called from PlayerInput object in scene objects hierarchy, under GAME > InputManagers
    public void HandleJump(InputAction.CallbackContext context)
    {
        // only do jump on key down
        if (!context.started) return;

        Debug.Log("Jumping!");


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
        // Debug.Log("Setting hitJumpKey to " + hitJumpKey);
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
        // if (hitJumpKey == true) Debug.Log("Inside HitJumpKey(), returning " + hitJumpKey);
        // else Debug.Log("Inside HitJumpKey(), returning " + hitJumpKey);
        return hitJumpKey;
    }
}
