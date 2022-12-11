using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCombat : MonoBehaviour
{
    public static event Action<int> Attack;
    public static event Action ResetCombo;
    public static event Action ResetAttack;
    [SerializeField] private GameObject dart;
    [SerializeField] private int attackNumber = 1;
    [SerializeField] private int dartCount = 10;
    [SerializeField] private float castTime = 2f;

    private bool canAttack => attackResetCounter > 0;
    private bool resetAttack => attackResetCounter < 0;
    [SerializeField] private bool isAttacking;
    private bool isGrounded;
    [SerializeField] private float attackResetCounter = -1f;
    private float attackResetCounterMax = 3f;
    private System.Random RNG = new System.Random();

    private void OnEnable()
    {
        PlayerMovement.Grounded += IsGrounded;
    }
    private void OnDisable()
    {
        PlayerMovement.Grounded -= IsGrounded;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckAttack();
        CheckMagicalAttack();
    }


    private void IsGrounded(bool _isGrounded)
    {
        isGrounded = _isGrounded;
    }

    private void CheckAttack()
    {


        //single attack test
        /*if (Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack Called");
            isAttacking = true;
            AttackCall();
        }*/



        if (Input.GetButtonDown("Attack"))
        {
            Debug.Log("Attack Called");
            attackResetCounter = attackResetCounterMax;
        }
        if (attackResetCounter > -0.1f)
        {
            attackResetCounter -= Time.deltaTime;
        }
        if (canAttack && !isAttacking)
        {
            Debug.Log("Attack Initiated");
            isAttacking = true;
            AttackCall();
        }
        if (resetAttack)
        {
            ComboReset();
        }
    }

    private void AttackCall()
    {
        Attack(attackNumber);

        switch (attackNumber)
        {
            case 1:
                attackNumber++;
                break;
            case 2:
                attackNumber++;
                break;
            case 3:
                ComboReset();
                break;
        }
    }

    private void ComboReset()
    {
        attackNumber = 1;
        EndAttack();
    }

    private void CheckMagicalAttack()
    {
        if (Input.GetButtonDown("MagicAttack"))
        {
            StartCoroutine(MagicalAttack());
        }
    }

    IEnumerator MagicalAttack()
    {

        for(int i = 0; i < dartCount; i++)
        {
            yield return new WaitForSeconds(1/dartCount * castTime);
            Instantiate(dart, transform.position,Quaternion.Euler(0,0,RNG.Next(0,360)));
            Debug.Log("Missle Fired");
        }
    }

    //call with Animation Event after the hit has been detected but before exit time
    private void EndAttack()
    {
        isAttacking = false;
        ResetAttack();
    }

}
