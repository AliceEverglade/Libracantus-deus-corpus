using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region components
    Rigidbody2D rb;
    #endregion

    #region Layer Masks
    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    #endregion

    #region movement vars
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float groundLinearDrag;
    private Vector2 move;
    private bool changingDirection => (rb.velocity.x > 0f && move.x < 0f || rb.velocity.x < 0f && move.x > 0f);
    #endregion

    #region jump vars
    [Header("Jump Variables")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float airLinearDrag = 2.5f;
    [SerializeField] private float fallMultiplier = 8f;
    [SerializeField] private float lowJumpFallMultiplier = 5f;
    [SerializeField] private float hangTime = 0.1f;
    [SerializeField] private float jumpBufferLength = 0.1f;
    [SerializeField] private int extraJumps = 1;
    private float hangTimeCounter;
    private float jumpBufferCounter;
    private int extraJumpsValue;
    private bool canJump => jumpBufferCounter > 0 && (hangTimeCounter > 0 || extraJumpsValue > 0);
    #endregion

    #region Collision Variables
    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRayCastLength;
    [SerializeField] private bool isGrounded;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //Fixed Update is called once per time interval
    void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        if (isGrounded)
        {
            extraJumpsValue = extraJumps;
            hangTimeCounter = hangTime;
            ApplyGroundLinearDrag();
        }
        else
        {
            ApplyAirLinearDrag();
            FallMultiplier();
            hangTimeCounter -= Time.deltaTime;
        }
    }


    // Update is called once per frame
    void Update()
    {
        move.x = GetInput().x;
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferLength;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (canJump)
        {
            Jump();
        }
    }

    private static Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(move.x, 0) * acceleration);
        Debug.Log(rb.velocity.x);
        float xVel = rb.velocity.x;
        xVel = Mathf.Clamp(xVel, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(xVel, rb.velocity.y);
        
    }

    private void ApplyGroundLinearDrag()
    {
        if(Mathf.Abs(move.x) < 0.4f || changingDirection)
        {
            rb.drag = groundLinearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }
    
    private void ApplyAirLinearDrag()
    {
            rb.drag = airLinearDrag;
    }

    private void CheckCollisions()
    {
        isGrounded = Physics2D.Raycast(transform.position * groundRayCastLength, Vector2.down, groundRayCastLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRayCastLength);
    }

    private void Jump()
    {
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;
        if (!isGrounded)
        {
            extraJumpsValue--;
        }
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void FallMultiplier()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity *= 0.9f;
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else
        {
            rb.gravityScale *= 1;
        }
    }
}
