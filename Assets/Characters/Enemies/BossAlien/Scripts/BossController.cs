using CodeMonkey.HealthSystemCM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BossBehavior : MonoBehaviour
{
    [Header("Movement control variables")]
    [SerializeField] float moveSpeed;

    private Vector3 moveDirection;


    // may be used to apply gravity if necessary
    private CharacterController bossCharacterController;


    [Header("Hitboxes and hurtboxes")]
    [SerializeField] GameObject BossHitboxSlash;
    [SerializeField] GameObject BossHitboxSlam;
    [SerializeField] GameObject BossHurtbox;


    [Header("Object references")]
    [SerializeField] GameObject BossModel;
    [SerializeField] GameObject PlayerModel;
    [SerializeField] BossAnimationController BossAnimationScript;

    Animator bossAnimator;

    [Header("Set movement points")]
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] GameObject point3;
    [SerializeField] GameObject point4;

    // only visible in this class, for internal management only
    GameObject chosenTarget;

    DateTime relocateStart;




    // class-wide boolean to track if the boss has made an action
    private bool madeDecision;

    // timer to keep track of when to make the decisions
    private DateTime decisionMade;
    private DateTime now;

    // activate the hitboxes during specific time windows of the animation
    private DateTime slamStart;
    private DateTime slashStart;

    // variable to determine update reroute to appropriate behavior subroutine
    string bossState;

    private const string START = "start";
    private const string IDLE = "idle";
    private const string RELOCATE = "relocate";
    private const string SLASH = "slash";
    private const string SLAM = "slam";
    private const string ROAR = "roar";

    // boolean values to control animation states
    private bool relocate;
    private bool slashing;
    private bool slamming;
    private bool roaring;

    



    // BOSS HEALTH - using CodeMonkey's simple health system
    HealthSystem bossHealthSystem;
    private bool isDead;


    private void Awake()
    {
        bossAnimator = GetComponent<Animator>();
        bossCharacterController = GetComponent<CharacterController>();
        isDead = false;

        // initialize to 1000 health for now
        bossHealthSystem = new HealthSystem(1000);
        bossHealthSystem.OnDead += BossHealthSystem_OnDead;

        bossState = START;
    }

    



    // BOSS CONTROL LOGIC:
    // I want to make him take a random action once every 7 seconds.
    // Each outcome has different probabilities.
    // The boss will choose from:
    // 1.) Walk to random position (that's relatively close by) - 15%
    // 2.) Perform slash attack (attacks in front of boss)      - 35%
    // 3.) Perform slam attack (attacks in radius around boss)  - 35%
    // 4.) Roar (does nothing except play roaring sound)        - 15%

    // Update is called once per frame
    void Update()
    {
        // if the boss is dead, do not allow anything else to happen
        if (isDead) return;



        // if the boss is roaring, refrain from making a decision (or doing anything else)
        AnimatorStateInfo stateInfo = bossAnimator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("MutantRoar"))
        {
            return;
        }

        now = DateTime.Now;




        // if the boss is in a state (that's not "start"), reroute to the appropriate method to update behavior
        if (bossState != START)
        {
            if (bossState == RELOCATE)
            {
                // make the boss stay in the relocate state for 5.5 seconds
                TimeSpan relocateTime = now - relocateStart;

                if (relocateTime.TotalSeconds < 5.5)
                {
                    MoveToTarget();

                    return;
                }
                else
                {
                    // boss does nothing when idle, should go back to deciding next state
                    bossState = IDLE;
                    madeDecision = false;

                    relocate = false;

                    // make the function start from scratch on the next frame update
                    return;
                }
                
            }

            else if (bossState == ROAR)
            {
                // reset the roaring bool to stop it in the animator
                roaring = false;
            }


            else if (bossState == SLASH)
            {
                Slash();

                // deactivate the slashing state when finished
                slashing = false;
            }

            else if (bossState == SLAM)
            {
                Slam();

                // reset the slamming bool to stop it in the animator
                slamming = false;
            }
        }





        // this is the section where a decision of what action to take is made
        if (!madeDecision)
        {
            Debug.Log("Boss script: making decision:");

            decisionMade = DateTime.Now;


            


            // roll a random number between 0 and 100, call appropriate function behavior
            float randomNumber = UnityEngine.Random.Range(0, 101);

            if (randomNumber < 15)
            {
                Debug.Log("Relocating!");

                // choose new random position to walk to
                int random = UnityEngine.Random.Range(1, 5);

                Debug.Log("Choosing random point " + random);

                if (random == 1) chosenTarget = point1;
                if (random == 2) chosenTarget = point2;
                if (random == 3) chosenTarget = point3;
                if (random == 4) chosenTarget = point4;

                bossState = RELOCATE;

                relocateStart = DateTime.Now;

                relocate = true;
            }

            else if (randomNumber >= 15 && randomNumber < 50)
            {
                Debug.Log("Slash attacking!");

                // perform slash attack
                bossState = SLASH;
                slashing = true;

                slashStart = DateTime.Now;
            }

            else if (randomNumber >= 50 && randomNumber < 85)
            {
                Debug.Log("Slam attacking!");

                // perform slam attack
                bossState = SLAM;
                slamming = true;

                slamStart = DateTime.Now;
            }

            else if (randomNumber >= 85)
            {
                Debug.Log("Roaring!");

                // just roar and play sound effect (when we get it)
                bossState = ROAR;
                roaring = true;
            }

            madeDecision = true;
        }




        // check when now - madeDecision is greater than 7 seconds
        TimeSpan timeSpan = now - decisionMade;

        if (timeSpan.TotalSeconds > 7)
        {
            Debug.Log("Boss script: resetting decisionMade to false!");
            madeDecision = false;
        }

        // always rotate boss towards the player (unless it is walking towards a position)
        if (bossState != "relocate")
        {
            BossModel.transform.LookAt(PlayerModel.transform);

            // fix vertical rotation when player gets close
            BossModel.transform.eulerAngles = new Vector3(0f, BossModel.transform.eulerAngles.y, BossModel.transform.eulerAngles.z);
        }

    }


    private void OnCollisionEnter(Collision collider)
    {
        // Debug.Log("Registered hit on Boss!");

        if (collider.gameObject.tag == "SwordHitbox")
        {
            // Debug.Log("Boss hit by SwordHitbox!");
        }

        else if (collider.gameObject.tag == "SpinHitbox")
        {
            // Debug.Log("Boss hit by SpinHitbox!");
        }
    }

    



    void MoveToTarget()
    {
        relocate = true;

        // rotate the boss to face the chosen
        BossModel.transform.LookAt(chosenTarget.transform);

        // fix vertical rotation when boss gets close
        BossModel.transform.eulerAngles = new Vector3(0f, BossModel.transform.eulerAngles.y, BossModel.transform.eulerAngles.z);


        // move boss horizontally towards the target point
        moveDirection = BossModel.transform.forward * moveSpeed * Time.deltaTime;
        moveDirection.y = 0f;

        BossModel.transform.position += moveDirection;
    }

    void Slash()
    {
        
        // keep the slash hitbox active for the first 2.5 seconds of the slash animation
        TimeSpan slashTimespan = now - slashStart;

        if (slashTimespan.TotalSeconds < 2.5)
        {
            BossHitboxSlash.SetActive(true);
        }
        else
        {
            BossHitboxSlash.SetActive(false);
        }
    }

    void Slam()
    {
        // keep the slam hitbox active for seconds 2 through 4 of the slam animation
        TimeSpan slamtimespan = now - slamStart;

        if (slamtimespan.TotalSeconds > 2 && slamtimespan.TotalSeconds < 4)
        {
            BossHitboxSlam.SetActive(true);
        }
        else
        {
            BossHitboxSlam.SetActive(false);
        }
    }



    private void BossHealthSystem_OnDead(object sender, EventArgs e)
    {
        isDead = true;

        Debug.Log("Boss is dead!");

        // TODO: Implement big death explosion
    }

    public void Damage(float damageAmount)
    {
        bossHealthSystem.Damage(damageAmount);
    }



    public bool GetSlashState()
    {
        return slashing;
    }

    public bool GetSlamState()
    {
        return slamming;
    }

    public bool GetRoarState()
    {
        return roaring;
    }

    public bool IsMoving()
    {
        return relocate;
    }


    public bool IsDead()
    {
        return isDead;
    }

    // Start is called before the first frame update
    void Start()
    {
        madeDecision = false;
        isDead = false;
    }

    
}
