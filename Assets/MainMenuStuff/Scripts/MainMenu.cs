using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // required to load different scenes

public class MainMenu : MonoBehaviour
{
    public string firstLevel;
    public void StartGame()
    {
        // TODO: Uncomment this when ready to implement game control flow
        // SceneManager.LoadScene(firstLevel);
    }

    public void OpenSettings()
    {

    }

    public void CloseOptions()
    {

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
