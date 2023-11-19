using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource mainMenuAudioSource;
    [SerializeField] AudioSource mainMenuMusicSource;
    [SerializeField] AudioClip mouseOverSound;
    [SerializeField] AudioClip clickButtonSound;
    [SerializeField] AudioClip mainMenuSong;


    // Update is called once per frame
    void Update()
    {

    }

    private void Awake()
    {
        Invoke("PlayMainMenuSong", 1f);
    }

    void PlayMainMenuSong()
    {
        mainMenuMusicSource.PlayOneShot(mainMenuSong);
    }


    public void PlayMouseOverButtonSound()
    {
        mainMenuAudioSource.PlayOneShot(mouseOverSound);
    }

    public void PlayClickButtonSound()
    {
        mainMenuAudioSource.PlayOneShot(clickButtonSound);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
