using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class BearAnimatorController : MonoBehaviour
{
    [SerializeField] BearController bearScript;
    [SerializeField] GameObject BearModel;
    Animator animator;

    private const string IS_LAZER = "IsLazer";
    private const string IS_SWIPING = "IsSwiping";
    private const string IS_IDLE = "IsIdle";
    private const string IS_WALKING = "IsWalking";
    private const string IS_DEAD = "IsDead";

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(IS_LAZER, bearScript.IsLazering());
        animator.SetBool(IS_SWIPING, bearScript.IsSwiping());
        
        animator.SetBool(IS_WALKING, bearScript.IsWalking());
        animator.SetBool(IS_DEAD, bearScript.IsDead());
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
