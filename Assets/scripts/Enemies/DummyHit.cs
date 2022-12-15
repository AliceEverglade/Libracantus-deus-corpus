using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DummyHit : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;

    [SerializeField] private float maxHP = 9999;
    [SerializeField] private float currentHP;
    private System.Random RNG = new System.Random();
    [SerializeField] private float damage => RNG.Next(20,30);
    [SerializeField] private float healAmount = 200;
    [SerializeField] private float healTimer = 2f;
    [SerializeField] private float healTimerValue;
    [SerializeField] private float animDelay;
    [SerializeField] private float animDelayValue;

    public static event Action AddScore;
    public static event Action<float> DamagePlayer;

    [SerializeField] private float movementSpeed = 30;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        Heal();
        Move();
        if (animDelayValue < 0)
        {
            anim.SetBool("isDamaged", false);
        }
        else
        {
            animDelayValue -= Time.deltaTime;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(movementSpeed, rb.velocity.y);
    }

    public void TakeDamage(float damage)
    {
        anim.SetBool("isDamaged", true);
        animDelayValue = animDelay;
        if(currentHP - damage <= 0)
        {
            AddScore();
            Destroy(gameObject);
        }
        else
        {
            currentHP -= damage;
        }
    }

    private void Heal()
    {
        if(healTimerValue < 0)
        {
            if(currentHP + healAmount > maxHP)
            {
                currentHP = maxHP;
            }
            else
            {
                currentHP += healAmount;
            }
            healTimerValue = healTimer;
        }
        else
        {
            healTimerValue -= Time.deltaTime;
        }
    }
}