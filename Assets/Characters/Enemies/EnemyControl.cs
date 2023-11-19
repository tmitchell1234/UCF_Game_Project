using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;




// TODO: Remove this when our personal health system is implemented
// (or keep it if we like how CodeMonkey's health system works
using CodeMonkey.HealthSystemCM;
using System.Runtime.CompilerServices;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyControl : MonoBehaviour, IGetHealthSystem
{
    [SerializeField] GameObject AlienModel;
    [SerializeField] GameObject PlayerModel;
    private CharacterController AlienController;

    [SerializeField] PlayerController playerScript;

    Animator AlienAnimator;


    // sound script for sound effects
    ParkLevelSoundManager soundScript;
    private bool alreadyPlayedExplosion = false;




    // used only for debugging falling through the floor
    [SerializeField] GameObject ground;


    // [SerializeField] float moveSpeed;
    float moveSpeed = 11f;

    // [SerializeField] float attackDistance;
    float attackDistance = 6f;



    


    private float verticalVelocity = 0f;
    private float gravityValue = -9.81f;
    //[SerializeField] private float gravityMultiplier = 0.001f;
    private float gravityMultiplier = 0.05f;

    private bool inRange;
    private bool currentlyMoving;
    private bool attacking;

    private Vector3 moveDirection;




    // manage hitboxes and hurtboxes
    GameObject hurtBox;
    GameObject slapBox;

    private bool isHit;

    GameObject explosionPrefab;

    GameObject explosionEffectObject;
    ParticleSystem explosionEffect;
    GameObject explosionRingObject;
    ParticleSystem explosionRing;
    GameObject explosionLightObject;
    Light explosionLight;

    




    // measure time to limit how often the enemy can attack
    private DateTime startAttack;
    private DateTime now;

    private DateTime deathStart;


    // used to calculate, on death, the direction the alien will fly back and die
    Vector3 backwardsDirection;


    HealthSystem healthSystem;
    bool isDead;

    private void Awake()
    {
        AlienController = GetComponent<CharacterController>();
        inRange = false;
        currentlyMoving = false;
        attacking = false;
        isHit = false;
        isDead = false;


        AlienAnimator = GetComponent<Animator>();


        // set initial value of startAttack to avoid null reference error in checking attack animation
        startAttack = DateTime.Now;


        AlienModel = this.gameObject;
        PlayerModel = GameObject.Find("TacoManModel (PLAYER)");
        playerScript = PlayerModel.GetComponent<PlayerController>();

        soundScript = GameObject.Find("SoundManager").GetComponent<ParkLevelSoundManager>();


        ground = GameObject.Find("Ground");



        // this might work? can only get child object by index, not by tag or name.
        hurtBox = AlienModel.transform.GetChild(2).gameObject;

        slapBox = AlienModel.transform.GetChild(4).gameObject;

        if (slapBox.tag == "AlienSlapbox")
            // Debug.Log("Successfully got alien slap box!");


        // max health set to 100
        healthSystem = new HealthSystem(100);

        healthSystem.OnDead += HealthSystem_OnDead;

        // explosionEffect = GetComponent<ParticleSystem>();
        explosionPrefab = AlienModel.transform.GetChild(3).gameObject;
        
        explosionEffectObject = explosionPrefab.transform.GetChild(0).gameObject;
        explosionRingObject = explosionPrefab.transform.GetChild(1).gameObject;
        explosionLightObject = explosionPrefab.transform.GetChild(2).gameObject;

        explosionEffect = explosionPrefab.transform.GetChild(0).GetComponent<ParticleSystem>();
        explosionRing = explosionPrefab.transform.GetChild(1).GetComponent<ParticleSystem>();
        explosionLight = explosionPrefab.transform.GetChild(2).GetComponent<Light>();


    }






    // Start is called before the first frame update
    void Start()
    {
        inRange = false;

        // set initial value of startAttack to avoid null reference error in checking attack animation
        startAttack = DateTime.Now;


        // if (hurtBox.tag == "AlienHurtbox") Debug.Log("Got hurtbox successfully!");
    }




    // Update is called once per frame
    void Update()
    {
        // if the alien is dead, stop doing everything else
        if (isDead)
        {

            FlyBackwards();

            // let the alien play it's death animation for 5 seconds
            now = DateTime.Now;

            TimeSpan timeSpan = now - deathStart;

            if (timeSpan.TotalSeconds > 3)
            {
                explosionEffectObject.SetActive(true);
                explosionRingObject.SetActive(true);
                explosionLightObject.SetActive(true);
                
                explosionEffect.Play();
                explosionRing.Play();

                // play explosion sound effect (ONLY ONCE)
                if (!alreadyPlayedExplosion)
                {
                    PlayExplosionSound();
                    alreadyPlayedExplosion = true;
                }

                
            }

            if (timeSpan.TotalSeconds > 4)
            {
                Destroy(gameObject);
            }



            return;
        }


        
        // if the alien is hit, do nothing until the stunned animation stops playing
        AnimatorStateInfo enemyAnimationState = AlienAnimator.GetCurrentAnimatorStateInfo(0);
        if (enemyAnimationState.IsName("Stunned"))
        {
            
            // reset the value of the boolean to toggle it off in the animator
            isHit = false;
            return;
        }

        // activate the hit box when attacking
        if (enemyAnimationState.IsName("Melee Hit"))
        {
            DateTime current = DateTime.Now;

            TimeSpan timeSinceAnimationStart = current - startAttack;

            // activate the slap hitbox after the animation has played for 0.8 seconds
            if (timeSinceAnimationStart.TotalSeconds > 0.8)
                slapBox.SetActive(true);

            // reset position of hitbox (since it keeps flying away for some reason...)
            // slapBox.transform.position = AlienModel.transform.position;
            // slapBox.transform.position = new Vector3(slapBox.transform.position.x, slapBox.transform.position.y, slapBox.transform.position.z);
            // slapBox.transform.rotation = AlienModel.transform.rotation;
            // slapBox.transform.eulerAngles = new Vector3(90, 0, 0);

        }
        else
        {
            slapBox.SetActive(false);
        }



        float height = AlienController.transform.position.y - ground.transform.position.y;

        // apply gravity if not on ground
        // update: now applying gravity is controlled separately from horizontal movement
        // if (!AlienController.isGrounded)

        // nuclear option: no longer trust Unity to give us accurate information
        if (height > 0.5f)
        {
            // Debug.Log("Applying gravity!");
            ApplyGravity();
        }



        // face the player
        AlienModel.transform.LookAt(PlayerModel.transform);

        // fix the model's vertical rotation
        AlienModel.transform.eulerAngles = new Vector3(0f, AlienModel.transform.eulerAngles.y, AlienModel.transform.eulerAngles.z);


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

            TimeSpan timeElapsed = now - startAttack;

            // Debug.Log("Time between attacks (timeElapsed) = " + timeElapsed.TotalSeconds);

            if (timeElapsed.TotalSeconds > 3)
            {
                // startAttack = DateTime.Now;
                attacking = true;
                startAttack = DateTime.Now;


                // start timer to activate the slap hitbox at appropriate times
            }
            else
            {
                attacking = false;
            }
        }
    }

    public void PlaySlapSound()
    {
        soundScript.PlayAlienSlapSound();
    }

    public void PlayExplosionSound()
    {
        soundScript.PlayExplosionSound();
    }

    public bool IsDead()
    {
        return isDead;
    }



    public void Damage(float damageAmount)
    {
        healthSystem.Damage(damageAmount); 

    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        isDead = true;

        deathStart = DateTime.Now;

        // handle the death process at the top of update.
        // we want to activate the death animation, let the alien fall to the ground, then explode

        // subtracting the vectors will give a vector between the two players. multiply it by -1 to point away from the player
        backwardsDirection = -1 * (PlayerModel.transform.position - AlienModel.transform.position);

        backwardsDirection.y = 0;

        // set move speed of fly backwards to 1.1
        backwardsDirection = backwardsDirection * 1.1f * Time.deltaTime;
    }



    private void OnCollisionEnter(Collision collider)
    {
        // Debug.Log("Registered collision on enemy!");
        if (collider.gameObject.tag == "SwordHitbox")
        {
            // Debug.Log("Enemy hit by SwordHitbox!");
            isHit = true;
            Damage(35);

            // heal the player when they deal damage
            playerScript.Heal(5);
        }

        if (collider.gameObject.tag == "SwordHitbox2")
        {
            isHit = true;
            Damage(65);

            playerScript.Heal(15);
        }
        if (collider.gameObject.tag == "SpinHitbox")
        {
            // Debug.Log("Enemy hit by SpinHitbox!");
            isHit = true;
            Damage(15);

            playerScript.Heal(1);
        }
    }

    public bool IsHit()
    {
        return isHit;
    }


    public bool IsAttacking()
    {
        return attacking;
    
    }
    public bool IsMoving()
    {
        return currentlyMoving;
    }

    private void FlyBackwards()
    {
        AlienModel.transform.position += backwardsDirection;
    }

    private void MoveToPlayer()
    {
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

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
}
