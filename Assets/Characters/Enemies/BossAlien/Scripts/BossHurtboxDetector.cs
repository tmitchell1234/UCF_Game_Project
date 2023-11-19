using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHurtboxDetector : MonoBehaviour
{

    [Header("Reference to boss controller script")]
    [SerializeField] BossBehavior bossScript;

    [SerializeField] PlayerController playerScript;

    [SerializeField] GameObject Boss;

    private void OnCollisionEnter(Collision collider)
    {
        
        // Debug.Log("Registered hit on capsule collider");

        if (collider.gameObject.tag == "SwordHitbox")
        {
            Debug.Log("Boss hit by SwordHitbox, dealing 30 damage!");

            bossScript.Damage(30);

            // heal the player when they deal damage
            playerScript.Heal(10);
            
        }

        else if (collider.gameObject.tag == "SwordHitbox2")
        {
            Debug.Log("Boss hit by SwordHitbox2, dealing 60 damage!");

            bossScript.Damage(60);

            playerScript.Heal(25);
        }
        else if (collider.gameObject.tag == "SpinHitbox")
        {
            Debug.Log("Boss hit by SpinHitbox, dealing 20 damage!");

            bossScript.Damage(20);

            playerScript.Heal(5);
        }

        
    }
    // Start is called before the first frame update
    void Start()
    {
        bossScript = Boss.GetComponent<BossBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
