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
        PlayerMovement.Dashing += DashingAnimation;
        PlayerCombat.Attack += AttackingAnimation;
        PlayerCombat.ResetAttack += ResetAttackingAnimation;
        PlayerCombat.ResetCombo += ResetAttackCombo;
    }
    private void OnDisable()
    {
        PlayerMovement.Jumping -= JumpingAnimation;
        PlayerMovement.Falling -= FallingAnimation;
        PlayerMovement.Grounded -= IsGrounded;
        PlayerMovement.Moving -= WalkingAnimation;
        PlayerMovement.Standing -= IdleAnimation;
        PlayerMovement.Dashing -= DashingAnimation;
        PlayerCombat.Attack -= AttackingAnimation;
        PlayerCombat.ResetAttack -= ResetAttackingAnimation;
        PlayerCombat.ResetCombo -= ResetAttackCombo;
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

    private void DashingAnimation(bool input)
    {
        animator.SetBool("isDashing", input);
    }
    private void AttackingAnimation(int attackNumber)
    {
        Debug.Log("Attack Animations Started");
        animator.SetBool("isAttacking", true);
        switch (attackNumber)
        {
            case 1:
                animator.SetBool("attack1", true);
                break;
            case 2:
                animator.SetBool("attack2", true);
                break;
            case 3:
                animator.SetBool("attack3", true);
                break;
        }
    }
    private void ResetAttackCombo()
    {
        animator.SetBool("attack1", false);
        animator.SetBool("attack2", false);
        animator.SetBool("attack3", false);
    }
    private void ResetAttackingAnimation()
    {
        animator.SetBool("isAttacking", false);
    }

    private void MagicAnimation()
    {

    }

    private void UltimateAnimation()
    {

    }
}
