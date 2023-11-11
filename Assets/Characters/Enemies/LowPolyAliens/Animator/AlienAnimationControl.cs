using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienAnimationControl : MonoBehaviour
{
    private Animator animator;

    private const string IS_MOVING = "IsMoving";
    private const string IS_ATTACKING = "IsAttacking";
    private const string IS_HIT = "IsHit";
    private const string IS_DEAD = "IsDead";

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
        // check if alien is dead first. we don't want any other animations to trigger if it is:
        animator.SetBool(IS_DEAD, enemyControlScript.IsDead());
        if (enemyControlScript.IsDead())
        {
            return;
        }

        // reset boolean of attacking after animation has played
        attacking = false;

        // check for being stunned first
        animator.SetBool(IS_HIT, enemyControlScript.IsHit());

        // skip the other checks for the other animation states if the alien is hit
        // TODO: double check this part for bugs
        if (enemyControlScript.IsHit())
            return;


        animator.SetBool(IS_ATTACKING, enemyControlScript.IsAttacking());

        animator.SetBool(IS_MOVING, enemyControlScript.IsMoving());
        

    }
}
