using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearActivator : MonoBehaviour
{
    [SerializeField] BossBehavior bossScript;
    [SerializeField] GameObject FakeBearBoss;
    [SerializeField] GameObject RealBearBoss;
    [SerializeField] GameObject TacoModel;

    [SerializeField] BearController bearScript; // used to detect when bear is dead, re-spawn the taco model

    [SerializeField] GameObject ground;

    [SerializeField] GameObject BearTitle;
    [SerializeField] GameObject BearHealthbar;

    [SerializeField] GameObject AlienBossTitle;
    [SerializeField] GameObject AlienBossHealthbar;

    [SerializeField] GameObject MothershipText;
    [SerializeField] GameObject TacoRecoveredText;
    [SerializeField] GameObject EnemySpawner;

    [SerializeField] ParkLevelSoundManager soundManager;

    bool alreadyActivated;

    private void Awake()
    {
        bearScript = RealBearBoss.GetComponent<BearController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        // re-spawn the taco when the bear is dead
        if (bearScript.IsDead())
        {
            DeactivateEnemySpawner();
            Invoke("ActivateTaco", 3f);

            Invoke("DespawnBearOnDeath", 10f);
            Invoke("ActivateTacoRecoveredText", 10f);
        }



        // once the alien boss has died, we want to create:
        // 1. the taco model, after 3.5s
        // 2. The fake bear which falls on the taco, after 6s
        // 3. The real bear, which activates when the fake bear reaches the ground

        if (bossScript.IsDead())
        {
            
            if (alreadyActivated) return;

            // deactivate the boss title and healthbar
            AlienBossTitle.SetActive(false);
            AlienBossHealthbar.SetActive(false);

            // create the taco model, spawn in after 3.5s (after boss death animation has played for a little bit)
            Invoke("ActivateTaco", 3.5f);

            // create the fake bear, which will fall onto the scene on top of the taco
            Invoke("ActivateFakeBear", 6f);

            // after the fake bear has had time to fall onto the taco, disable it, disable the taco, and activate the real bear boss
            // will need to manually adjust times until it feels right
            Invoke("DeactivateFakeBear", 8.5f);

            Invoke("DeactivateTaco", 8.5f);

            Invoke("ActivateRealBear", 8.5f);

            alreadyActivated = true;
        }
    }
    void DeactivateEnemySpawner()
    {
        EnemySpawner.SetActive(false);
    }
    void ActivateTacoRecoveredText()
    {
        TacoRecoveredText.SetActive(true);
    }
    void DespawnBearOnDeath()
    {
        RealBearBoss.SetActive(false);
    }
    void ActivateRealBear()
    {
        RealBearBoss.SetActive(true);

        BearTitle.SetActive(true);
        BearHealthbar.SetActive(true);
    }

    void ActivateFakeBear()
    {
        FakeBearBoss.SetActive(true);

        // activate music here
        soundManager.PlayBearBossMusic();
    }

    void DeactivateFakeBear()
    {
        FakeBearBoss.SetActive(false);
    }
    void ActivateTaco()
    {
        TacoModel.SetActive(true);
    }

    void DeactivateTaco()
    {
        TacoModel.SetActive(false);
    }

    void DeactivateMothershipText()
    {
        MothershipText.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        alreadyActivated = false;

        Invoke("DeactivateMothershipText", 4f);
    }

    
}
