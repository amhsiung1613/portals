using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    bool isFacingRight = true; // if sprite is facing right bu default, else, set to false
    public ParticleSystem smokeFX;

    [Header("Movement")]
    public float moveSpeed = 5f;

    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps= 2;
    int jumpsRemaining;

    [Header("Ground Check")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Wall Check")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    [Header("Wall Movement")]
    public float wallSlideSpeed = 2;
    bool isWallSliding;

    //wall jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.5f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);

    [Header("Gravity")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fallSpeedMult = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        GroundCheck();
        //WallCheck();
        processGravity();
        processWallSlide();
        processWallJump();
        //Flip();

        if(!isWallJumping) {
            rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            Flip();
        }
        
    }

    public void Move(InputAction.CallbackContext context) 
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        smokeFX.Play();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //double jumps aren't working again?
        if (jumpsRemaining > 0) {
            if (context.performed) {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;

                smokeFX.Play();
            }
            else if (context.canceled) {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;

                smokeFX.Play();
            }
        }
        
        //wall jump isnt working either
        //wall jump
        if (context.performed && wallJumpTimer > 0f) {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0;

            smokeFX.Play();

            //force flip
            if(transform.localScale.x != wallJumpDirection) {
                 isFacingRight = !isFacingRight;
                Vector3 ls = transform.localScale;
                ls.x *= -1f;
                transform.localScale = ls;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); //wall jump lasts 0.f sec, can jump again after 0.6 sec
        }
    }

    private void GroundCheck() {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)) {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        } else {
            isGrounded = false;
        }
        
    }

    private bool WallCheck() {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }


    private void processGravity() {
        if (rb.velocity.y < 0) {
            rb.gravityScale = baseGravity * fallSpeedMult;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }
        else {
            rb.gravityScale = baseGravity;
        }
    }

    private void processWallSlide() {
        if (!isGrounded && WallCheck() && horizontalMovement != 0) {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, - wallSlideSpeed));
        } else {
            isWallSliding = false;
        }
    }

    private void processWallJump() {
        if (isWallSliding) {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(CancelWallJump));
        } else if(wallJumpTimer > 0f) {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump() {
        isWallJumping = false;
    }

    private void Flip() {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0) {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;

            if (rb.velocity.y == 0) {
                smokeFX.Play();
            }
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }
}
