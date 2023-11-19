using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuHandler : MonoBehaviour
{
    public static bool isPaused = false;

    [Header("Reference to UI panels")]
    [SerializeField] GameObject PauseMenuUI;
    [SerializeField] GameObject DeathMenu;

    [Header("Reference to player script")]
    [SerializeField] PlayerController playerScript;

    [Header("Names of scenes to load")]
    [SerializeField] string menuScene;
    [SerializeField] string levelScene;
    [SerializeField] string creditsScene;

    [Header("Pause screen buttons")]
    [SerializeField] GameObject ContinueButton;
    [SerializeField] GameObject OptionsButton;
    [SerializeField] GameObject MainMenuButton;

    [SerializeField] GameObject OptionsScreen;


    [Header("Death screen buttons")]
    [SerializeField] GameObject RetryButton;
    [SerializeField] GameObject DeathMenuButton;

    [Header("Reference to sound system")]
    [SerializeField] ParkLevelSoundManager soundManager;

    [Header("Enemy spawner")]
    [SerializeField] GameObject enemySpawner;




    

    // Update is called once per frame
    void Update()
    {
        if (playerScript.IsDead())
        {
            DeathScreen();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeMusic();
                Resume();
            }
            else
            {
                StopMusic();
                Pause();
            }
        }
    }

    void StopMusic()
    {
        soundManager.PauseMusic();
    }

    void ResumeMusic()
    {
        soundManager.UnpauseMusic();
    }


    void StopEnemySpawner()
    {
        enemySpawner.SetActive(false);
    }

    void ResumeEnemySpawner()
    {
        enemySpawner.SetActive(true);
    }

    public void DeathScreen()
    {
        DeathMenu.SetActive(true);
        StopMusic();

        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        // stop all music
        //soundManager.StopLevelMusic();
        //soundManager.StopAlienBossMusic();
        StopMusic();

        SceneManager.LoadScene(levelScene);
    }

    public void ReturnToMainMenu()
    {
        StopMusic();

        SceneManager.LoadScene(menuScene);
    }

    public void Resume()
    {
        ResumeMusic();

        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        ResumeEnemySpawner();
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        StopEnemySpawner();
    }


    public void ShowOptions()
    {
        ContinueButton.SetActive(false);
        OptionsButton.SetActive(false);
        MainMenuButton.SetActive(false);

        OptionsScreen.SetActive(true);
    }

    public void ReturnToMenuButton()
    {
        ContinueButton?.SetActive(true);
        OptionsButton?.SetActive(true);
        MainMenuButton?.SetActive(true);

        OptionsScreen.SetActive(false);
    }

    


    public void PlayMouseoverSound()
    {
        soundManager.PlayMouseoverSound();
    }

    public void PlayMenuSelectSound()
    {
        soundManager.PlaySelectSound();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

}
