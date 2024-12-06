
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Player reference
    public float chaseSpeed = 2f; // Speed for chasing
    public float jumpForce = 5f; // Force applied for jumps
    public LayerMask groundLayer; // Ground detection layer
    public int damage = 1;

    private Rigidbody2D rb; // Rigidbody component
    private bool isGrounded; // Ground check
    private bool shouldJump; // Jump flag

    private float detectionRange = 10f; // Distance within which the enemy chases the player

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get Rigidbody2D component
    }

    void Update()
    {
        // Check if the enemy is grounded using a raycast
        //isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer); // Extend distance

        // Calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Calculate direction towards the player
            float direction = Mathf.Sign(player.position.x - transform.position.x);

            if (isGrounded)
            {
                // Chase the player
                rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

                // Raycasts to check for obstacles and gaps
                RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 1f, groundLayer);
                RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 1f, groundLayer);

                // Jump if there's an obstacle or a gap ahead
                if (!groundInFront.collider || !gapAhead.collider)
                {
                    shouldJump = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isGrounded && shouldJump)
        {
            shouldJump = false;

            // Jump upward
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
    
    // Optional: Visualize raycasts in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Ground detection
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1f);

        // Forward detection
        float direction = (player != null && player.position.x > transform.position.x) ? 1f : -1f;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * 1f);

        // Gap detection
        Gizmos.DrawLine(transform.position + new Vector3(direction, 0, 0), transform.position + new Vector3(direction, -1f, 0));
    }
    
}
