using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public static event Action<int> Attack;
    private int attackNumber = 0;
    private bool isGrounded;
    private float attackResetCounter;
    private float attackResetCounterMax;

    private void OnEnable()
    {
        PlayerMovement.Grounded += IsGrounded;
    }
    private void OnDisable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(attackResetCounter <= 0) { attackResetCounter -= Time.deltaTime; }
        else { ResetAttack; }
        if (Input.GetButtonDown("BasicAttack"))
        {
            CheckAttack(attackNumber);
        }
    }

    private void IsGrounded(bool _isGrounded)
    {
        isGrounded = _isGrounded;
    }

    private void CheckAttack(int attackCount)
    {
        switch (attackCount)
        {
            case 0:
                attackCount++;
                break;
            case 1:
                attackCount++;
                break;
            case 2:
                attackCount = 0;
                break;
        }
        Attack(attackNumber);
    }

    private void ResetAttack()
    {
        attackNumber = 0;
    }
}
