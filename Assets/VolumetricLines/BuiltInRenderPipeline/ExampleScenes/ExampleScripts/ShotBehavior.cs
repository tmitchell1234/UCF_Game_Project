using UnityEngine;
using System.Collections;
using CodeMonkey.HealthSystemCM;

public class ShotBehavior : MonoBehaviour
{
	GameObject PlayerModel;
	PlayerController playerScript;
	HealthSystem playerHealthSystem;

	bool alreadyHitPlayer;

	// Use this for initialization
	void Start ()
	{
		
	}

    private void Awake()
    {
        PlayerModel = GameObject.Find("TacoManModel (PLAYER)");
        playerScript = PlayerModel.GetComponent<PlayerController>();
        playerHealthSystem = playerScript.GetHealthSystem();

		alreadyHitPlayer = false;
    }

    // Update is called once per frame
    void Update () {
		transform.position += transform.forward * Time.deltaTime * 700f;

		// since hit detection doesn't seem to be working with these, we will manually calculate the distance to the player and damage them
		if (!alreadyHitPlayer)
		{
            Vector3 dist = PlayerModel.transform.position - this.transform.position;

			if (dist.magnitude < 4)
			{
				Debug.Log("Lazer got close to player, dealing 7 damage!");
				playerHealthSystem.Damage(7);
				alreadyHitPlayer = true;
			}
        }
		
	
	}

	/*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
		{
			Debug.Log("Player hit by lazer! 4 damage");
			playerHealthSystem.Damage(4);
		}
    }
	*/
}
