using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] GameObject FakeBoss;
    [SerializeField] GameObject BossPrefab;

    [SerializeField] GameObject BossTitle;
    [SerializeField] GameObject BossHealthbar;

    [SerializeField] GameObject BossLight;

    [SerializeField] BossBehavior bossScript;

    bool isDead;

    [SerializeField] ParkLevelSoundManager soundManager;



    private void OnCollisionEnter(Collision collider)
    {
        Debug.Log("Detected collision on boss trigger!");

        if (collider.gameObject.tag == "Player")
        {
            Debug.Log("Player passed boss trigger! Activating real boss");


            FakeBoss.SetActive(false);
            BossPrefab.SetActive(true);

            // disable the boss trigger. I'm not sure, at the moment, how to avoid the boss trigger being a solid box.
            this.gameObject.SetActive(false);

            BossTitle.SetActive(true);
            BossHealthbar.SetActive(true);

            BossLight.SetActive(true);

            soundManager.StopLevelMusic();
            soundManager.PlayAlienBossMusic();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
