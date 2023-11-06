using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // required to load different scenes

public class MainMenu : MonoBehaviour
{
    public string firstLevel;

    public GameObject optionsScreen;

    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject exitButton;

    public void StartGame()
    {
        // TODO: Uncomment this when ready to implement game control flow
        // SceneManager.LoadScene(firstLevel);
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);

        playButton.SetActive(false);
        optionsButton.SetActive(false);
        exitButton.SetActive(false);
    }

    public void CloseOptions()
    {
        optionsScreen.SetActive(false);

        playButton.SetActive(true);
        optionsButton.SetActive(true);
        exitButton.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
