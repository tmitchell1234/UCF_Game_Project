using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class EnemySpawnSystem : MonoBehaviour
{
    // manage spawning enemies and spawn radius
    [SerializeField] GameObject alienEnemyPrefab;
    [SerializeField] float xSpawnRange;
    [SerializeField] float zSpawnRange;

    // reference the parent object
    private GameObject player;

    // measure game time to spawn alien enemies
    private DateTime gameStart;

    // make a boolean check to only allow the spawn to occur once during the 20 second modulus mark
    private bool alreadySpawned;


    // Start is called before the first frame update
    void Start()
    {
        gameStart = DateTime.Now;

        player = this.gameObject;

        // set alreadySpawned to true so that the alien enemies aren't spawned in at the first instant of the game running
        alreadySpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        DateTime now = DateTime.Now;

        // spawn enemies, after all other actions, every 20 seconds. (excluding the first second of the game)
        // recaluclate timeElapsed to apply to the main game timer:
        TimeSpan timeSinceStart = now - gameStart;


        double seconds = Math.Floor(timeSinceStart.TotalSeconds);

        // Debug.Log("Seconds = " + seconds);

        if (!alreadySpawned && seconds % 10 == 0)
        {
            // Debug.Log("Spawning enemies!");
            // SpawnEnemies();
            alreadySpawned = true;
        }
        // prevent the script from resetting alreadyspawned after the first call to spawn
        else if (seconds % 10 == 0)
        {
            alreadySpawned = true;
        }
        else
        {
            alreadySpawned = false;
        }
    }

    void SpawnEnemies()
    {
        // spawn 6 enemies, change range of loop to adjust total number.
        // range of positions is defined in the game engine by xSpawnRange and zSpawnRange.

        for (int i = 0; i < 6; i++)
        {
            // for now, spawn enemies at height 6
            Vector3 position = new Vector3(
                player.transform.position.x + UnityEngine.Random.Range(-xSpawnRange, xSpawnRange),
                player.transform.position.y + 4,
                player.transform.position.z + UnityEngine.Random.Range(-zSpawnRange, zSpawnRange));
        
            GameObject alienEnemy = Instantiate(alienEnemyPrefab, position, Quaternion.identity) as GameObject;
        
        }
    }
}
