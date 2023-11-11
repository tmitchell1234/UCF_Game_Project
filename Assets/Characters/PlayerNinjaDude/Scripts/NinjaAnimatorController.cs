using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAnimatorController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Player player;
    [SerializeField] private Transform sword;
    [SerializeField] private CharacterController characterController;

    [SerializeField] private GameObject swordHitBox;

    private bool oneTimeRunCheck;

    private bool isAttackingState;

    private const string IS_MOVING = "IsMoving";
    private const string IS_FALLING = "IsFalling";
    private const string HIT_JUMP_KEY = "HitJumpKey";
    private const string MOUSE_CLICKED = "MouseClicked";
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_MOVING, false);
        oneTimeRunCheck = false;
        characterController = GetComponent<CharacterController>();
    }




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_MOVING, player.IsMoving());
        animator.SetBool(HIT_JUMP_KEY, player.HitJumpKey());
        animator.SetBool(IS_FALLING, player.IsFalling());

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            isAttackingState = true;
        else
            isAttackingState = false;


        // attack animation
        // first, detect if mouse button has been clicked
        if (Input.GetKeyDown(KeyCode.Mouse0))
            animator.SetTrigger(MOUSE_CLICKED);


        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            swordHitBox.SetActive(true);
        else
            swordHitBox.SetActive(false);

        // This is a good idea to start, doesn't work as intended. See function for details
        // if ((!oneTimeRunCheck) && animator.GetCurrentAnimatorStateInfo(0).IsName("Run"))
            // FixSwordRotationOnRun();

        
    }

    public bool isAttacking()
    {
        return isAttackingState;
    }

    void FixSwordRotationOnRun()
    {
        // does not work as intended
        // this sets absolute angle of sword relative to global plane, not relative to character model
        // need to figure out some different method later
        sword.eulerAngles = new Vector3(
            73f, -18f, 203f
        );
        //sword.Rotate(new Vector3(90f, 200f, 0f));
        Debug.Log("Attempted to adjust sword rotation");
    }
}
