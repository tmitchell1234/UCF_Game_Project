using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkLevelSoundManager : MonoBehaviour
{



    [Header("Music")]
    [SerializeField] AudioSource LevelMusicSource;
    [SerializeField] AudioSource AlienBossMusicSource;
    [SerializeField] AudioSource BearMusicSource;


    
    

    [Header("Sound effects audiosources")]
    [SerializeField] AudioSource PlayerWalkSource;
    [SerializeField] AudioSource SwordSwingSource;
    [SerializeField] AudioSource SwordSpinSource;
    [SerializeField] AudioSource PlayerJumpAndDashSource;
    [SerializeField] AudioSource ExplosionSource;
    [SerializeField] AudioSource AlienSlapSource;
    [SerializeField] AudioSource BossThunderingFootstepSource;
    [SerializeField] AudioSource AlienBossRoarSource;
    [SerializeField] AudioSource BossSlashSource;
    [SerializeField] AudioSource BearRoarSource;
    [SerializeField] AudioSource BearLazerSource;


    [Header("Pause menu audiosources")]
    [SerializeField] AudioSource MouseoverSource;
    [SerializeField] AudioSource MenuSelectsource;


    // reference the player object and script to play sounds when attacking / taking damage
    [SerializeField] GameObject PlayerPrefab;
    [SerializeField] PlayerController PlayerScript;
    CharacterController PlayerCharacterController;
    Animator PlayerAnimator;

    // bosses
    [SerializeField] GameObject AlienBoss;
    [SerializeField] GameObject BearBoss;


    [SerializeField] AudioClip PlayerWalk; // DONE
    private bool alreadyStartedWalkSound;
    
    [SerializeField] AudioClip SwordSwing; // DONE
    [SerializeField] AudioClip SwordSpin;  // DONE
    [SerializeField] AudioClip PlayerDash;
    [SerializeField] AudioClip PlayerJumpSound;

    [SerializeField] AudioClip Explosion;  // Alien explosion - DONE
                                           // Bear explosions - DONE

    [SerializeField] AudioClip AlienSlap;  // DONE


    [SerializeField] AudioClip ThunderingFootstep; // Alien Boss
                                                   // Bear Boss

    [SerializeField] AudioClip BossAlienRoar;  // DONE
    [SerializeField] AudioClip BossSlash;      // DONE


    [SerializeField] AudioClip BearRoar;  // DONE
    [SerializeField] AudioClip BearSlash; // DONE
    [SerializeField] AudioClip BearLazer; // DONE





    private AudioSource previouslyPlayed;
    // TODO:
    // Put music tracks in on a loop.
    // Make special music for boss battles when they spawn in.
    // Make a sound for when enemies get hit.

    private void Awake()
    {
        PlayerCharacterController = PlayerPrefab.GetComponent<CharacterController>();
        PlayerAnimator = PlayerPrefab.GetComponent<Animator>();

        Invoke("PlayLevelMusic", 1.5f);
    }
    

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo PlayerCurrentAnimationState = PlayerAnimator.GetCurrentAnimatorStateInfo(0);

        // When player is moving and on ground, set an invokerepeating one time for the walking sound.
        // when player is no longer moving or on the ground, stop the invokerepeating on the playermovesound.
        // when the player is slash attacking or spin attacking, stop the playermovesound.
        
        // conversely, only play the walking sound when player is running.
        if (!alreadyStartedWalkSound && PlayerCurrentAnimationState.IsName("Run"))
        {
            InvokeRepeating("PlayerMoveSound", 0f, 0.18f);
            alreadyStartedWalkSound = true;
        }
        else if (!PlayerCurrentAnimationState.IsName("Run"))
        {
            CancelInvoke("PlayerMoveSound");
            alreadyStartedWalkSound = false;
        }
        

        // this method doesn't really work just yet. keeping it here in case I figure something out about it later.
        /*
        if (!alreadyStartedWalkSound && PlayerCharacterController.isGrounded && PlayerScript.IsMoving())
        {
            InvokeRepeating("PlayerMoveSound", 0f, 0.19f);
            alreadyStartedWalkSound = true;
        }
        else if (!PlayerCharacterController.isGrounded)
        {
            CancelInvoke("PlayerMoveSound");
            alreadyStartedWalkSound = false;
        }
        else if (!PlayerScript.IsMoving())
        {
            CancelInvoke("PlayerMoveSound");
            alreadyStartedWalkSound = false;
        }
        else if (PlayerCurrentAnimationState.IsName("SwordSlash1") || PlayerCurrentAnimationState.IsName("SwordSlash2") ||
                 PlayerCurrentAnimationState.IsName("SpinAttack"))
        {
            CancelInvoke("PlayerMoveSound");
            alreadyStartedWalkSound = false;
        }
        */
        
    }

    // for the pause menu, pause/resume the appropriate music
    public void PauseMusic()
    {
        if (LevelMusicSource.isPlaying)
            previouslyPlayed = LevelMusicSource;

        else if (AlienBossMusicSource.isPlaying)
            previouslyPlayed = AlienBossMusicSource

    ;
        else if (BearMusicSource.isPlaying)
            previouslyPlayed = BearMusicSource;

        previouslyPlayed.Pause();
    }

    public void UnpauseMusic()
    {
        previouslyPlayed.UnPause();
    }
    // music tracks
    public void PlayLevelMusic()
    {
        LevelMusicSource.Play();
    }
    
    public void StopLevelMusic()
    {
        LevelMusicSource.Stop();
    }

    public void PlayAlienBossMusic()
    {
        AlienBossMusicSource.Play();
    }

    public void StopAlienBossMusic()
    {
        AlienBossMusicSource.Stop();
    }

    public void PlayBearBossMusic()
    {
        BearMusicSource.Play();
    }

    public void PlayerMoveSound()
    {
        // FootstepSource.PlayOneShot(PlayerWalk);
        PlayerWalkSource.Play();
    }

    public void PlaySwordSwing()
    {
        // SoundEffectSource.PlayOneShot(SwordSwing);
        SwordSwingSource.Play();
    }

    public void PlaySwordSpin()
    {
        // SoundEffectSource.PlayOneShot(SwordSpin);
        SwordSpinSource.Play();
    }

    public void PlayDashSound()
    {
        // FootstepSource.PlayOneShot(PlayerDash);
        PlayerJumpAndDashSource.Play();
    }

    public void PlayJumpSound()
    {
        // FootstepSource.PlayOneShot(PlayerJumpSound);
        PlayerJumpAndDashSource.Play();
    }

    public void PlayAlienSlapSound()
    {
        // SoundEffectSource.PlayOneShot(AlienSlap);
        AlienSlapSource.Play();
    }

    public void PlayExplosionSound()
    {
        // ExplosionSource.PlayOneShot(Explosion);
        ExplosionSource.Play();
    }


    // BOSS SOUNDS
    public void PlayThunderingFootsteps()
    {
        // SoundEffectSource.PlayOneShot(ThunderingFootstep);
        BossThunderingFootstepSource.Play();
    }

    public void PlayBossRoar()
    {
        // SoundEffectSource.PlayOneShot(BossAlienRoar);
        AlienBossRoarSource.Play();
    }

    public void PlayBossSlash()
    {
        // SoundEffectSource.PlayOneShot(BossSlash);
        BossSlashSource.Play();
    }

    // BEAR SOUMDS
    public void PlayBearRoar()
    {
        // BearRoarSource.PlayOneShot(BearRoar);
        BearRoarSource.Play();
    }

    public void PlayBearSlashSound()
    {
        // SoundEffectSource.PlayOneShot(BearSlash);
        BossSlashSource.Play();
    }
    public void PlayLazerSound()
    {
        // SoundEffectSource.PlayOneShot(BearLazer);
        BearLazerSource.Play();
    }

    public void PlayMouseoverSound()
    {
        MouseoverSource.Play();
    }

    public void PlaySelectSound()
    {
        MenuSelectsource.Play();
    }


    public void PlayMusic()
    {

    }


    // Start is called before the first frame update
    void Start()
    {
        alreadyStartedWalkSound = false;
    }
}
