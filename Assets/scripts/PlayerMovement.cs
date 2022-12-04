using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    #region components
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject effects;
    #endregion

    #region Layer Masks
    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask cornerCorrectionLayer;
    #endregion

    #region movement vars
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float groundLinearDrag;
    private Vector2 move;
    private bool changingDirection => (rb.velocity.x > 0f && HorizontalDirection < 0f || rb.velocity.x < 0f && HorizontalDirection > 0f);
    private bool canMove => !wallGrab;

    private float HorizontalDirection
    {
        get
        {
            return this.move.x;
        }
        set
        {
            if(this.move.x != value)
            {
                this.move.x = value;
                Moving(value);
            }
        }
    }
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
    [SerializeField] private float hangTimeCounter;
    private float jumpBufferCounter;
    private int extraJumpsValue;
    private bool canJump => jumpBufferCounter > 0f && (hangTimeCounter > 0f || extraJumpsValue > 0f || onWall);
    private bool isJumping = false;
    #endregion

    #region Wall Movement Variables
    [Header("Wall Movement Variables")]
    [SerializeField] private float wallSlideModifier = 0.35f;
    [SerializeField] private float wallClimbModifier = 0.85f;
    [SerializeField] private float wallJumpXVelocityHaltDelay = 0.2f;
    private bool wallGrab => onWall && !IsGrounded && Input.GetButton("WallGrab");
    private bool wallSlide => onWall && !IsGrounded && !Input.GetButton("WallGrab") && rb.velocity.y < 0f && !wallClimb;
    private bool wallClimb => onWall && move.y > 0f && wallGrab;
    #endregion

    #region Collision Variables
    [Header("Ground Collision Variables")]
    [SerializeField] private float groundRayCastLength;
    [SerializeField] private Vector3 groundRayCastOffset;
    [SerializeField] private bool isGrounded;

    private bool IsGrounded
    {
        get
        {
            return isGrounded;
        }
        set
        {
            if(isGrounded != value)
            {
                isGrounded = value;
                Grounded(value);
            }
        }
    }
    #endregion

    #region Wall Collision Variables
    [Header("Wall Collision Variabless")]
    [SerializeField] private float wallRayCastLength;
    [SerializeField] private Vector3 wallRayCastOffset;
    [SerializeField] private bool onWall;
    [SerializeField] private bool onRightWall;
    #endregion

    #region Dash Variables
    [Header("Dash Variables")]
    [SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashLength = 0.3f;
    [SerializeField] private float dashBufferLength = 0.1f;
    [SerializeField] private float dashBufferCounter;
    [SerializeField] private bool isDashing;
    [SerializeField] private bool hasDashed;
    private bool canDash => dashBufferCounter > 0f && !hasDashed;
    #endregion

    #region Corner Correction Variables
    [Header("Corner Correction Variables")]
    [SerializeField] private float topRayCastLength;
    [SerializeField] private Vector3 edgeRayCastOffset;
    [SerializeField] private Vector3 innerRayCastOffset;

    private bool canCornerCorrect;
    #endregion

    #region Physics Variables
    [Header("Physics Variables")]
    [SerializeField] private float gravityFraction = 0.3f;
    #endregion

    #region Combat Variables
    [Header("Combat Variables")]
    [SerializeField] private bool isAttacking;
    [SerializeField] private bool attack1Done;
    [SerializeField] private bool attack2Done;
    [SerializeField] private float lastClickedTime;
    [SerializeField] private float maxComboDelay;
    #endregion

    #region Animation Variables
    [Header("Animation Variables")]
    [SerializeField] private bool facingRight;
    #endregion

    #region Actions
    public static event Action Jumping;
    public static event Action Falling;
    public static event Action Standing;
    public static event Action<bool> Grounded;
    public static event Action<float> Moving;
    #endregion

    #region Enables and Disable functions
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }
    #endregion

    #region Start function
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    #endregion

    #region Fixed Update and Update
    //Fixed Update is called once per time interval
    void FixedUpdate()
    {
        CheckCollisions();
        if (!isDashing)
        {
            if (canMove) { MoveCharacter(); }
            else { rb.velocity = Vector2.Lerp(rb.velocity, (new Vector2(HorizontalDirection * maxSpeed, rb.velocity.y)), 0.5f * Time.deltaTime); }
            if (IsGrounded)
            {
                hasDashed = false;
                extraJumpsValue = extraJumps;
                hangTimeCounter = hangTime;
                ApplyGroundLinearDrag();

                //Animation
                Standing();
            }
            else
            {
                ApplyAirLinearDrag();
                FallMultiplier();
                hangTimeCounter -= Time.fixedDeltaTime;
                if (!onWall || rb.velocity.y < 0f || wallClimb) { isJumping = false; }
            }
            if (canJump)
            {
                if (onWall && !IsGrounded)
                {
                    if (!wallClimb && (onRightWall && HorizontalDirection > 0f || !onRightWall && HorizontalDirection < 0f)) { StartCoroutine(NeutralWallJump()); }
                    else { WallJump(); }
                    Flip();
                }
                else { Jump(Vector2.up); }
            }
            if (!isJumping)
            {
                if (wallGrab) { WallGrab(); }
                if (wallSlide) { WallSlide(); }
                if (wallClimb) { WallClimb(); }
                if (onWall) { StickToWall(); }
            }
        }

        if (canDash) { StartCoroutine(Dash(HorizontalDirection, move.y)); }
        if (canCornerCorrect) { CornerCorrect(rb.velocity.y); }
    }


    // Update is called once per frame
    void Update()
    {
        HorizontalDirection = GetInput().x;
        move.y = GetInput().y;
        CalculateVariables();

        if (Input.GetButtonDown("Attack")) { Attack(); }
        if (Input.GetButtonDown("Jump")) { jumpBufferCounter = jumpBufferLength; }
        else { jumpBufferCounter -= Time.deltaTime; }
        if (Input.GetButtonDown("Dash")) { dashBufferCounter = dashBufferLength; }
        else { dashBufferCounter -= Time.deltaTime; }

        //Animation
        animator.SetBool("IsGrounded", IsGrounded);
        animator.SetFloat("horizontalDirection", Mathf.Abs(HorizontalDirection));

        if(HorizontalDirection < 0f && facingRight) { Flip(); }
        else if (HorizontalDirection > 0f && !facingRight) { Flip(); }

        if(rb.velocity.y < 0f)
        {
            Falling();
        }
    }
    #endregion

    private static Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    private void CalculateVariables()
    {
        fallMultiplier = jumpForce * gravityFraction;
        lowJumpFallMultiplier = fallMultiplier * 1.5f;
        if(dashBufferCounter < 0) { dashBufferCounter = 0; }
        if(Mathf.Abs(rb.velocity.x) > 0.1f) { animator.SetFloat("runSpeed", rb.velocity.x / maxSpeed); }
    }

    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(HorizontalDirection, 0) * acceleration);
        float xVel = rb.velocity.x;
        xVel = Mathf.Clamp(xVel, -maxSpeed, maxSpeed);
        rb.velocity = new Vector2(xVel, rb.velocity.y);
        
    }

    private void ApplyGroundLinearDrag()
    {
        if(Mathf.Abs(HorizontalDirection) < 0.4f || changingDirection)
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
        //ground collision
        IsGrounded =    Physics2D.Raycast(  new Vector3(transform.position.x + groundRayCastOffset.x, 
                                            transform.position.y + groundRayCastOffset.y, 
                                            transform.position.z + groundRayCastOffset.z), 
                                            Vector2.down, 
                                            groundRayCastLength, 
                                            groundLayer) ||
                        Physics2D.Raycast(  new Vector3(transform.position.x - groundRayCastOffset.x, 
                                            transform.position.y + groundRayCastOffset.y, 
                                            transform.position.z + groundRayCastOffset.z), 
                                            Vector2.down, 
                                            groundRayCastLength, 
                                            groundLayer);

        //corner correction collision
        canCornerCorrect =  Physics2D.Raycast(transform.position + edgeRayCastOffset, Vector2.up, topRayCastLength, cornerCorrectionLayer) &&
                            !Physics2D.Raycast(transform.position + innerRayCastOffset, Vector2.up, topRayCastLength, cornerCorrectionLayer) ||
                            Physics2D.Raycast(transform.position - edgeRayCastOffset, Vector2.up, topRayCastLength, cornerCorrectionLayer) &&
                            !Physics2D.Raycast(transform.position - innerRayCastOffset, Vector2.up, topRayCastLength, cornerCorrectionLayer);

        //wall collision
        onWall = Physics2D.Raycast(transform.position + wallRayCastOffset, Vector2.right, wallRayCastLength, wallLayer) ||
                    Physics2D.Raycast(transform.position + wallRayCastOffset, Vector2.left, wallRayCastLength, wallLayer);
        onRightWall = Physics2D.Raycast(transform.position + wallRayCastOffset, Vector2.right, wallRayCastLength, wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        //ground check
        Gizmos.DrawLine(    new Vector3(transform.position.x + groundRayCastOffset.x, 
                            transform.position.y + groundRayCastOffset.y,
                            transform.position.z + groundRayCastOffset.z),
                            new Vector3(transform.position.x + groundRayCastOffset.x, 
                            transform.position.y + groundRayCastOffset.y, 
                            transform.position.z + groundRayCastOffset.z) + Vector3.down * groundRayCastLength);

        Gizmos.DrawLine(    new Vector3(transform.position.x - groundRayCastOffset.x, 
                            transform.position.y + groundRayCastOffset.y,
                            transform.position.z + groundRayCastOffset.z),
                            new Vector3(transform.position.x - groundRayCastOffset.x, 
                            transform.position.y + groundRayCastOffset.y, 
                            transform.position.z + groundRayCastOffset.z) + Vector3.down * groundRayCastLength);

        //corner check
        Gizmos.DrawLine(transform.position + edgeRayCastOffset, transform.position + edgeRayCastOffset + Vector3.up * topRayCastLength);
        Gizmos.DrawLine(transform.position - edgeRayCastOffset, transform.position - edgeRayCastOffset + Vector3.up * topRayCastLength);
        Gizmos.DrawLine(transform.position + innerRayCastOffset, transform.position + innerRayCastOffset + Vector3.up * topRayCastLength);
        Gizmos.DrawLine(transform.position - innerRayCastOffset, transform.position - innerRayCastOffset + Vector3.up * topRayCastLength);

        //corner distance check
        Gizmos.DrawLine(transform.position - innerRayCastOffset + Vector3.up * topRayCastLength,
                        transform.position - innerRayCastOffset + Vector3.up * topRayCastLength + Vector3.left * topRayCastLength);
        Gizmos.DrawLine(transform.position + innerRayCastOffset + Vector3.up * topRayCastLength,
                        transform.position + innerRayCastOffset + Vector3.up * topRayCastLength + Vector3.right * topRayCastLength);

        //wall check
        Gizmos.DrawLine(transform.position + wallRayCastOffset, transform.position + wallRayCastOffset + Vector3.right * wallRayCastLength);
        Gizmos.DrawLine(transform.position + wallRayCastOffset, transform.position + wallRayCastOffset + Vector3.left * wallRayCastLength);
    }

    private void Jump(Vector2 direction)
    {

        if (hangTimeCounter < 0 && !onWall) { extraJumpsValue--; }
        hangTimeCounter = 0f;
        jumpBufferCounter = 0f;
        isJumping = true;
        ApplyAirLinearDrag();
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(direction * jumpForce, ForceMode2D.Impulse);

        //Animation
        Jumping();
    }
    
    #region Wall Movement
    private void WallJump()
    {
        Vector2 jumpDirection = onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
    }

    IEnumerator NeutralWallJump()
    {
        Vector2 jumpDirection = onRightWall ? Vector2.left : Vector2.right;
        Jump(Vector2.up + jumpDirection);
        yield return new WaitForSeconds(wallJumpXVelocityHaltDelay);
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    private void WallGrab()
    {
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
    }

    private void WallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, -maxSpeed * wallSlideModifier);
    }

    private void WallClimb()
    {
        rb.velocity = new Vector2(rb.velocity.x, move.y * maxSpeed * wallClimbModifier);
    }

    private void StickToWall()
    {
        if (onRightWall && HorizontalDirection >= 0f)
        {
            rb.velocity = new Vector2(1f, rb.velocity.y);
        }
        else if (onRightWall && HorizontalDirection >= 0f)
        {
            rb.velocity = new Vector2(-1f, rb.velocity.y);
        }
        if (onRightWall && !facingRight)
        {
            Flip();
        }
        else if (!onRightWall && facingRight)
        {
            Flip();
        }
    }
    #endregion

    private void FallMultiplier()
    {
        if (rb.velocity.y <= 0) { rb.gravityScale = fallMultiplier; }
        else if (hasDashed && !isDashing) { rb.gravityScale = fallMultiplier; }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity *= 0.9f;
            rb.gravityScale = lowJumpFallMultiplier;
        }
        else { rb.gravityScale = 1f; }
    }

    private void CornerCorrect(float Yvelocity)
    {
        //push player to the right
        RaycastHit2D hit = Physics2D.Raycast(transform.position - innerRayCastOffset + Vector3.up * topRayCastLength, Vector3.left, topRayCastLength, cornerCorrectionLayer);
        if(hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * topRayCastLength,
                transform.position - edgeRayCastOffset + Vector3.up * topRayCastLength);
            transform.position = new Vector3(transform.position.x + newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
            return;
        }

        //push player to the left
        hit = Physics2D.Raycast(transform.position + innerRayCastOffset + Vector3.up * topRayCastLength, Vector3.right, topRayCastLength, groundLayer);
        if (hit.collider != null)
        {
            float newPos = Vector3.Distance(new Vector3(hit.point.x, transform.position.y, 0f) + Vector3.up * topRayCastLength,
                transform.position + edgeRayCastOffset + Vector3.up * topRayCastLength);
            transform.position = new Vector3(transform.position.x - newPos, transform.position.y, transform.position.z);
            rb.velocity = new Vector2(rb.velocity.x, Yvelocity);
        }
    }

    IEnumerator Dash(float x, float y)
    {
        float dashStartTime = Time.time;
        hasDashed = true;
        isDashing = true;
        IsGrounded = false;
        isJumping = false;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.drag = 0f;
        Vector2 dir;
        if(x != 0f && y != 0f) { dir = new Vector2(x, y); } 
        else
        {
            if (facingRight) { dir = new Vector2(1f, 0); }
            else { dir = new Vector2(-1f, 0); }
        }
        while(Time.time < dashStartTime + dashLength)
        {
            rb.velocity = dir.normalized * dashSpeed;
            yield return null;
        }
        isDashing = false;
    }

    private void Attack()
    {
        
        if (!isAttacking)
        {
            isAttacking = true;

        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}
 