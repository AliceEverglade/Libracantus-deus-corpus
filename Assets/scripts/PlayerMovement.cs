using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region components
    Rigidbody2D rb;
    #endregion

    #region movement vars
    [Header("Movement Variables")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float linearDrag;
    private Vector2 move;
    private bool changingDirection => (rb.velocity.x > 0f && move.x < 0f || rb.velocity.x < 0f && move.x > 0f);
    #endregion

    #region jump vars
    
    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    //Fixed Update is called once per time interval
    void FixedUpdate()
    {
        MoveCharacter();
        ApplyLinearDrag();
    }


    // Update is called once per frame
    void Update()
    {
        move.x = GetInput().x;

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

    private void ApplyLinearDrag()
    {
        if(Mathf.Abs(move.x) < 0.4f || changingDirection)
        {
            rb.drag = linearDrag;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    private void Jump()
    {
        
    }
}
