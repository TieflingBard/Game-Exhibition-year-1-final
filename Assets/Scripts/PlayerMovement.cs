using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{

   
    
    private float horizontal;
    
    private float speed = 14f;
    private float jumpingPower = 32f;
    private bool isFacingRight = true;
    
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
   
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    private float dashingPowerX = 150f;
    private float dashingPowerY = 150f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.2f;
    private bool canDash = true;
    private bool isdashing;

    private float wallSlideSpeed;

    private bool isisGrappling;

    private Vector2 facingDirection = Vector2.right;
   
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] public float defaultGravity;
    [SerializeField] private TrailRenderer MikeDashTrail;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

   
    
    // Start is called before the first frame update
    void Start()
    {
        rb.gravityScale = defaultGravity;
        isisGrappling = Grapple3.instance.isGrappling;
    }

     



    // Update is called once per frame
    void Update()
    {
        
        horizontal = Input.GetAxisRaw("Horizontal");

        isisGrappling = Grapple3.instance.isGrappling;

        
        

        if (isdashing)
        {
            return;
        }

        if (IsGrounded() && !isdashing)
        {
            canDash = true;
        }
        
        
        if (IsGrounded() || isOnWall())
        {
            coyoteTimeCounter = coyoteTime;

        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            jumpBufferCounter = 0f;
            
        }

       



        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
           rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTimeCounter = 0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        wallSlide();
         Flip();
    }

    private void FixedUpdate()
    {
        if (isdashing)
        {
            return;
        }

        
        
        
        
       if (!isisGrappling)
          {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
          
          }
        else if (isisGrappling & (Input.GetKey(KeyCode.A) || isisGrappling & Input.GetKey(KeyCode.D)))
         {
           rb.AddForce(facingDirection*horizontal, ForceMode2D.Impulse);
           
         }

       
      
        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.05f,0.7f), CapsuleDirection2D.Horizontal,0,groundLayer);
       
    }

    private bool isOnWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void wallSlide()
    {
        if (isOnWall() && !IsGrounded() && horizontal != 0f)
        {
           
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
            
        }
     
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }

    }

   

    private IEnumerator Dash()
    {
        canDash = false;
        isdashing = true;
        rb.gravityScale = 0f;
        MikeDashTrail.emitting = true;
        if (!Input.GetKey(KeyCode.W))
        {
            print("side dash");
            rb.velocity = new Vector2(transform.localScale.x * dashingPowerX, 0f);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)))
        {
            print("diagonal dash");
            rb.velocity = new Vector2(transform.localScale.x * dashingPowerX, transform.localScale.y * dashingPowerY);
        }
        else if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            print("Up Dash");
            rb.velocity = new Vector2(0f, transform.localScale.y * dashingPowerY);


        }
            
           
           yield return new WaitForSeconds(dashingTime);
           isdashing = false;
           MikeDashTrail.emitting = false;
           rb.gravityScale = 20f;
           yield return new WaitForSeconds(0.1f);
           rb.gravityScale = defaultGravity;
           yield return new WaitForSeconds(dashingCooldown);
            
        } 
    }















