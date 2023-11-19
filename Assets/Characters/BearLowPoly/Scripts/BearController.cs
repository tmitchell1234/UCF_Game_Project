using CodeMonkey.HealthSystemCM;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BearController : MonoBehaviour, IGetHealthSystem
{
    [Header("Game object references")]
    [SerializeField] GameObject BearModel;
    [SerializeField] GameObject PlayerModel;
    PlayerController playerScript;
    [SerializeField] ShootLazers LazerScript;


    [Header("Physics variables")]
    [SerializeField] float moveSpeed;

    [Header("Walking points")]
    [SerializeField] GameObject point1;
    [SerializeField] GameObject point2;
    [SerializeField] GameObject point3;
    [SerializeField] GameObject point4;

    GameObject chosenPoint;


    [Header("Hitboxes")]
    [SerializeField] GameObject SwipeHitbox;
    [SerializeField] GameObject SlamHitbox;

    // played when landing on ground and when finishing lazer beam attack
    [Header("Explosion effect")]
    [SerializeField] GameObject explosionEffectObject;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] GameObject explosionRingObject;
    [SerializeField] ParticleSystem explosionRing;
    [SerializeField] GameObject explosionLightObject;

    [Header("Sound effect script")]
    [SerializeField] ParkLevelSoundManager soundScript;


    // animator - to access and do things based on current animation state
    private Animator animator;



    // private class stuff
    private HealthSystem bearHealthSystem;
    private bool isDead;

    HealthSystem playerHealthSystem;



    // variables to handle current state, and booleans for use in the animator controller script
    private string state;
    private const string WALKING = "walking";
    private const string SWIPING = "swiping";
    private const string LAZER = "lazer";
    // private const string IDLE = "idle";

    private const string UNDECIDED = "undecided";

    private bool isWalking;
    private bool isSwiping;
    private bool isLazering;

    private bool alreadyLazered;

    private Vector3 moveDirection;

    private DateTime timeLastChoiceMade;
    
    


    void Awake()
    {
        // give BEAR 2000 health (raise if it feels like this is too easy)
        // bearHealthSystem = new HealthSystem(1700);
        bearHealthSystem = new HealthSystem(2000);
        isDead = false;

        bearHealthSystem.OnDead += BearHealthSystem_OnDead;

        playerScript = PlayerModel.GetComponent<PlayerController>();
        playerHealthSystem = playerScript.GetHealthSystem();

        // make the bear do it's lazer attack for the first thing
        state = LAZER;
        isLazering = true;
        Invoke("SetLazeringToFalse", 2);

        isWalking = false;
        isSwiping = false;

        animator = GetComponent<Animator>();


        // scratch this, we want to start playing the bear music when the fake one spawns
        // soundScript.PlayBearBossMusic();
        
    }

    private void BearHealthSystem_OnDead(object sender, System.EventArgs e)
    {
        isDead = true;
        Debug.Log("BEAR is dead!");

        // the bear will stand on it's hind legs when dying.
        // make it explode repeatedly until it disappears from the scene:
        InvokeRepeating("ExplodeOnDie", 2.5f, 0.5f);
        Invoke("DestroyBear", 10f);
    }

    private void ExplodeOnDie()
    {
        explosionEffectObject.SetActive(true);
        explosionRingObject.SetActive(true);
        explosionLightObject.SetActive(true);

        explosionEffect.Play();
        explosionRing.Play();
        soundScript.PlayExplosionSound();
    }

    private void DestroyBear()
    {
        Destroy(this);
    }


    // Update is called once per frame
    void Update()
    {
        // don't do anything if BEAR is dead
        if (isDead) return;
        
        // make a decision of what to do once every 7 seconds
        if (state == UNDECIDED)
        {
            // make the bear model face the player when idling
            BearModel.transform.LookAt(PlayerModel.transform.position);

            // fix the horizontal rotation
            BearModel.transform.eulerAngles = new Vector3(0f, BearModel.transform.eulerAngles.y, BearModel.transform.eulerAngles.z);

            DateTime now = DateTime.Now;

            TimeSpan span = now - timeLastChoiceMade;

            if (span.TotalSeconds < 7)
            {
                return;
            }

            Debug.Log("BEAR is choosing a new state:");

            // make a choice on what to do next.
            // roll a randum number between 0 and 100:
            // if 0 - 15: pick a spot to walk to
            // if 16 - 65: swipe attack
            // if 66 - 100: roar and shoot lazers


            int randomNumber = UnityEngine.Random.Range(0, 101);

            if (randomNumber < 16)
            {
                state = WALKING;
                timeLastChoiceMade = DateTime.Now;
                isWalking = true;

                // pick the spot to walk to
                int newRandomNum = UnityEngine.Random.Range(1, 5);

                if (newRandomNum == 1) chosenPoint = point1;
                if (newRandomNum == 2) chosenPoint = point2;
                if (newRandomNum == 3) chosenPoint = point3;
                if (newRandomNum == 4) chosenPoint = point4;

                Debug.Log("BEAR walking to point " + newRandomNum);
            }

            if (randomNumber >= 16 && randomNumber < 66)
            {
                Debug.Log("BEAR is swiping!");
                state = SWIPING;
                isSwiping = true;

                timeLastChoiceMade = DateTime.Now;
                Invoke("SetSwipingToFalse", 2);

                // activate the swipe hitbox in 0.5 seconds, then disable it again in 3.5 seconds
                Invoke("EnableSwipeHitbox", 0.5f);
                Invoke("DisableSwipeHitbox", 3.5f);
            }

            if (randomNumber >= 66)
            {
                Debug.Log("BEAR is lazering!");
                state = LAZER;
                timeLastChoiceMade = DateTime.Now;
                isLazering = true;

                alreadyLazered = false;

                Invoke("SetLazeringToFalse", 2);
            }
        }

        // call functions to handle the appropriate state
        if (state == WALKING) HandleWalking();
        if (state == SWIPING) HandleSwipeAttack(); // deprecated - not using anymore
        if (state == LAZER) HandleLazer();

        // once 6.5 seconds have passed, make the bear undecided again
        DateTime current = DateTime.Now;
        TimeSpan time = current - timeLastChoiceMade;

        if (time.TotalSeconds > 6.5)
        {
            state = UNDECIDED;
            isWalking = false;
            
            
        }
        
    }


    public void PlayBearRoarSound()
    {
        soundScript.PlayBearRoar();
    }

    public void PlayBearSlashSound()
    {
        soundScript.PlayBearSlashSound();
    }

    public void PlayFootsteps()
    {
        soundScript.PlayThunderingFootsteps();
    }

    void SetSwipingToFalse()
    {
        isSwiping = false;
    }

    void SetLazeringToFalse()
    {
        isLazering = false;
    }


    void HandleIdle()
    {

    }
    void HandleSwipeAttack()
    {
        // activate the swipe hitbox
        // SwipeHitbox.gameObject.SetActive(true);

        // isSwiping = false;
    }

    void EnableSwipeHitbox()
    {
        SwipeHitbox.gameObject.SetActive(true);
    }

    void DisableSwipeHitbox()
    {
        SwipeHitbox.gameObject.SetActive(false);
    }

    void HandleWalking()
    {
        // make the bear model face the direction it's walking
        BearModel.transform.LookAt(chosenPoint.transform.position);

        // fix the horizontal rotation
        BearModel.transform.eulerAngles = new Vector3(0f, BearModel.transform.eulerAngles.y, BearModel.transform.eulerAngles.z);


        // move BEAR horizontally towards the target point
        // move boss horizontally towards the target point
        moveDirection = BearModel.transform.forward * moveSpeed * Time.deltaTime;
        moveDirection.y = 0f;

        BearModel.transform.position += moveDirection;


        // isWalking = false;
    }

    void HandleLazer()
    {
        // prevent the script from calling invokerepeating every frame

        if (alreadyLazered) return;

        // repeatedly shoot lazers, starting in 1.5 seconds, repeat every 0.15 seconds (chosen based on feel)
        InvokeRepeating("ShootLazer", 1.75f, 0.15f);

        // stop the lazering after 5.9 seconds
        Invoke("StopLazering", 4.8f);

        alreadyLazered = true;

        // activate the slam hitbox and explosion effects when the bear lands, which is approx. 5.5 seconds after the animation starts
        Invoke("EnableBearSlambox", 4.9f);
        Invoke("DisableBearSlambox", 6.5f);
    }

    void EnableBearSlambox()
    {
        SlamHitbox.SetActive(true);

        explosionEffectObject.SetActive(true);
        explosionRingObject.SetActive(true);
        explosionLightObject.SetActive(true);

        explosionEffect.Play();
        explosionRing.Play();

        soundScript.PlayExplosionSound();
    }

    void DisableBearSlambox()
    {
        SlamHitbox.SetActive(false);
        explosionEffectObject.SetActive(false);
        explosionRingObject.SetActive(false);
        explosionLightObject.SetActive(false);

    }

    void ShootLazer()
    {
        LazerScript.ShootLazer();
        soundScript.PlayLazerSound();
    }

    void StopLazering()
    {
        CancelInvoke("ShootLazer");
    }
    

    private void OnCollisionEnter(Collision collider)
    {
        // Debug.Log("Bear hit by " + collider.gameObject.name);
        if (collider.gameObject.tag == "SwordHitbox")
        {
            Debug.Log("Bear hit by SwordHitbox! Dealing 40 damage");

            bearHealthSystem.Damage(40);

            // heal player 20
            playerHealthSystem.Heal(20);
        }

        if (collider.gameObject.tag == "SwordHitbox2")
        {
            Debug.Log("Bear hit by SwordHitbox2! Dealing 80 damage");

            bearHealthSystem.Damage(80);

            playerHealthSystem.Heal(40);

        }
        if (collider.gameObject.tag == "SpinHitbox")
        {
            Debug.Log("Bear hit by SpinHitbox! Dealing 20 damage");

            bearHealthSystem.Damage(20);

            // heal player 5
            playerHealthSystem.Heal(5);
        }
    }


    public bool IsWalking()
    {
        return isWalking;
    }

    public bool IsSwiping()
    {
        return isSwiping;
    }

    public bool IsLazering()
    {
        return isLazering;
    }

    public bool IsDead()
    { 
        return isDead;
    }



    public HealthSystem GetHealthSystem()
    {
        return bearHealthSystem;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeLastChoiceMade = DateTime.Now;

    }

}
