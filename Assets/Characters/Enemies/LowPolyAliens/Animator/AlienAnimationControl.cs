using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimationControl : MonoBehaviour
{
    private Animator animator;

    private const string IS_MOVING = "IsMoving";
    private const string IS_ATTACKING = "IsAttacking";

    private bool attacking;
    private bool isMoving;

    [SerializeField] EnemyControl enemyControlScript;






    public void Attack()
    {
        animator.SetBool(IS_ATTACKING, true);
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        attacking = false;
        isMoving = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // reset boolean of attacking after animation has played
        attacking = false;
        animator.SetBool(IS_ATTACKING, enemyControlScript.IsAttacking());

        animator.SetBool(IS_MOVING, enemyControlScript.IsMoving());

    }
}
