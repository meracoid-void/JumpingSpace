using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WalkingBehavior
{
    Left = 0,
    Right = 1,
    StandStill = 2,
    SmartLeft =3,
    SmartRight = 4,
}

public class GombaNPCBehavior : MonoBehaviour
{
    public WalkingBehavior walkingBehavior;
    public float speed = 1.0f; // Speed of the enemy
    private Rigidbody2D rb; // Rigidbody2D component for physics
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        Vector2 bottomPosition = new Vector2(transform.position.x, transform.position.y - boxCollider.size.y / 2);
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        if (!GameManager.Instance.isPlayerRespawning)
        {
            Vector2 rayOrigin = new Vector2(transform.position.x + 0.1f, transform.position.y - 0.1f);

            // Movement based on WalkingBehavior
            switch (walkingBehavior)
            {
                case WalkingBehavior.Left:
                    rb.velocity = new Vector2(-speed, rb.velocity.y);
                    spriteRenderer.flipX = true;
                    break;
                case WalkingBehavior.Right:
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    spriteRenderer.flipX = false;
                    break;
                case WalkingBehavior.StandStill:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    break;
                case WalkingBehavior.SmartLeft:
                    spriteRenderer.flipX = true;
                    // Cast a ray from the bottom position of the NPC slightly to the left
                    RaycastHit2D hitLeft = Physics2D.Raycast(bottomPosition + Vector2.left * 0.1f, Vector2.down, 1.0f, groundLayer);

                    // If the ray doesn't hit anything, it means there's empty space
                    if (hitLeft.collider != null)
                    {
                        Debug.Log("Hit: " + hitLeft.collider.gameObject.name);
                        rb.velocity = new Vector2(-speed, rb.velocity.y);
                    }
                    else
                    {
                        walkingBehavior = WalkingBehavior.SmartRight;
                    }
                    break;
                case WalkingBehavior.SmartRight:
                    spriteRenderer.flipX = false;
                    // Cast a ray downwards from a point slightly ahead of the NPC to the right
                    RaycastHit2D hitRight = Physics2D.Raycast(rayOrigin + Vector2.right, Vector2.down, -1.0f, groundLayer);

                    // If the ray doesn't hit anything, it means there's empty space
                    if (hitRight.collider == null)
                    {
                        // Logic when empty space is detected (e.g., turn around or stop)
                        walkingBehavior = WalkingBehavior.SmartLeft;
                    }
                    else
                    {
                        // Keep moving to the right
                        rb.velocity = new Vector2(speed, rb.velocity.y);
                    }
                    break;
            }
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the enemy collides with the player
        if (other.gameObject.tag == "Player")
        {
            // Reference to the singleton instance of TimerAndKill
            TimerAndKill timerAndKill = TimerAndKill.instance;

            // Trigger the respawn mechanism
            timerAndKill.timer = 0.0f; // This assumes that the respawn mechanism is triggered when timer reaches 0
        }
        // If the enemy collides with the death plane
        else if (other.gameObject.tag == "KillPlane")
        {
            // Destroy the enemy
            Destroy(gameObject);
        }
    }
}
