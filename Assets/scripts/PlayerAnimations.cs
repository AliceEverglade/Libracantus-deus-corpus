using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    private Animator animator;
    private void OnEnable()
    {
        PlayerMovement.Jumping += JumpingAnimation;
        PlayerMovement.Falling += FallingAnimation;
        PlayerMovement.Grounded += IsGrounded;
        PlayerMovement.Moving += WalkingAnimation;
        PlayerMovement.Standing += IdleAnimation;
    }
    private void OnDisable()
    {
        PlayerMovement.Jumping -= JumpingAnimation;
        PlayerMovement.Falling -= FallingAnimation;
        PlayerMovement.Grounded -= IsGrounded;
        PlayerMovement.Moving -= WalkingAnimation;
        PlayerMovement.Standing -= IdleAnimation;
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void IsGrounded(bool isGrounded)
    {
        Debug.Log("grounded info updated");
        animator.SetBool("isGrounded", isGrounded);
    }
    
    private void IdleAnimation()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", false);
    }

    private void WalkingAnimation(float horizontalDirection)
    {
        animator.SetFloat("HorizontalMovement", horizontalDirection);
    }

    private void JumpingAnimation()
    {
        animator.SetBool("isJumping", true);
        animator.SetBool("isFalling", false);
    }

    private void FallingAnimation()
    {
        animator.SetBool("isJumping", false);
        animator.SetBool("isFalling", true);
    }

    private void AttackingAnimation()
    {

    }

    private void MagicAnimation()
    {

    }

    private void UltimateAnimation()
    {

    }
}
