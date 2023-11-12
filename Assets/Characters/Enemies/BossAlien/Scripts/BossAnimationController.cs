using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;

    [Header("Reference to boss control script")]
    [SerializeField] BossBehavior bossScript;

    private const string IS_DEAD = "IsDead";
    private const string IS_MOVING = "IsMoving";
    private const string SLASH = "Slash";
    private const string SLAM = "Slam";
    private const string ROAR = "Roar";


    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_MOVING, bossScript.IsMoving());
        animator.SetBool(IS_DEAD, bossScript.IsDead());
        animator.SetBool(SLASH, bossScript.GetSlashState());
        animator.SetBool(SLAM, bossScript.GetSlamState());
        animator.SetBool(ROAR, bossScript.GetRoarState());
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
}
