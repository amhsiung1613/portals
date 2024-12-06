// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Enemy : MonoBehaviour
// {
//     public Transform player;
//     public float chaseSpeed = 2f;
//     public float jumpForce = 2f;
//     public LayerMask groundLayer;

//     private Rigidbody2D rb;
//     private bool isGrounded;
//     private bool shouldJump;

//     // Start is called before the first frame update
//     void Start()
//     {
//        rb = GetComponent<Rigidbody2D>();

//     }

//     // Update is called once per frame
//     void Update()
//     {
//         //is grounded
//         isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

//         //direction
//         float direction = Mathf.Sign(player.position.x - transform.position.x);

//         //player above
//         bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);


//         if (isGrounded) {
//             //chase player
//             rb.velocity = new Vector2(direction * chaseSpeed, rb.velocity.y);

//             //jump if no gap ahead and no ground in front

//             //else if player above and platform above

//             //if ground
//             RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);
//             //if gap
//             RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);
//             //platform above
//             RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

//             if (!groundInFront.collider && !gapAhead.collider) {
//                 shouldJump = true;
//             } else if (isPlayerAbove && platformAbove.collider) {
//                 shouldJump = true;
//             }
//         }   
//     }

//     private void FixedUpdate() {
//         if (isGrounded && shouldJump) {
//             shouldJump = false;
//             Vector2 direction = (player.position - transform.position).normalized;
//             Vector2 jumpDirection = direction * jumpForce;

//             rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
//         }
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player; // Player reference
    public float chaseSpeed = 2f; // Speed for chasing
    public float jumpForce = 5f; // Force applied for jumps
    public LayerMask groundLayer; // Ground detection layer

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
