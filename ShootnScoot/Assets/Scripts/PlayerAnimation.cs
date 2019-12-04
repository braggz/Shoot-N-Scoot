using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private AnimationState animationState = AnimationState.NONE;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (animationState)
        {
            case AnimationState.NONE:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                break;
            case AnimationState.IDLE:
                animator.SetBool("isIdle", true);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", false);
                break;
            case AnimationState.RUNNING:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", true);
                animator.SetBool("isJumping", false);
                break;
            case AnimationState.JUMP:
                animator.SetBool("isIdle", false);
                animator.SetBool("isRunning", false);
                animator.SetBool("isJumping", true);
                break;
            default:
                break;
        }
    }
}

public enum AnimationState
{
    NONE = 0,
    IDLE = 1,
    RUNNING = 2,
    JUMP = 3
};
