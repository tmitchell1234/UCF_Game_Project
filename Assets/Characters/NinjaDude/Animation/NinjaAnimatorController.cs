using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAnimatorController : MonoBehaviour
{
    [SerializeField] private Player player;

    private const string IS_MOVING = "IsMoving";
    private const string IS_FALLING = "IsFalling";
    private const string HIT_JUMP_KEY = "HitJumpKey";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool(IS_MOVING, false);
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
        
    }
}
