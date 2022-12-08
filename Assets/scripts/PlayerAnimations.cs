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
        PlayerMovement.dashing += DashingAnimation;
    }
    private void OnDisable()
    {
        PlayerMovement.Jumping -= JumpingAnimation;
        PlayerMovement.Falling -= FallingAnimation;
        PlayerMovement.Grounded -= IsGrounded;
        PlayerMovement.Moving -= WalkingAnimation;
        PlayerMovement.Standing -= IdleAnimation;
        PlayerMovement.dashing -= DashingAnimation;
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
        Debug.Log(horizontalDirection);
        animator.SetFloat("HorizontalDirection", Mathf.Abs(horizontalDirection));
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

    private void DashingAnimation()
    {
        animator.SetBool("isDashing", true);
    }

    private void AttackingAnimation(int attackNumber)
    {

    }

    private void MagicAnimation()
    {

    }

    private void UltimateAnimation()
    {

    }
}
