using System;
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

    private Vector3 initialPosition;
    private WalkingBehavior initialWalkingBehavior;

    private float nextChangeTime = 0.0f;
    private float changeCooldown = 0.15f;  // Adjust as needed

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        initialWalkingBehavior = walkingBehavior;
    }

    public void ResetToInitialState()
    {
        transform.position = initialPosition;
        walkingBehavior = initialWalkingBehavior;
        // Add other resets here if needed
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
                    spriteRenderer.flipX = false;
                    break;
                case WalkingBehavior.Right:
                    rb.velocity = new Vector2(speed, rb.velocity.y);
                    spriteRenderer.flipX = true;
                    break;
                case WalkingBehavior.StandStill:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    break;
                case WalkingBehavior.SmartLeft:
                    if (Time.time >= nextChangeTime)
                    {
                        spriteRenderer.flipX = false;
                        RaycastHit2D hitLeft = Physics2D.Raycast(bottomPosition + Vector2.left * 0.1f, Vector2.down, 1.0f, groundLayer);
                        if (hitLeft.collider == null)
                        {
                            walkingBehavior = WalkingBehavior.SmartRight;
                            nextChangeTime = Time.time + changeCooldown;
                            
                        }
                        else
                        {
                            rb.velocity = new Vector2(-speed, rb.velocity.y);
                        }
                    }
                    break;
                case WalkingBehavior.SmartRight:
                    if (Time.time >= nextChangeTime)
                    {
                        spriteRenderer.flipX = true;
                        RaycastHit2D hitRight = Physics2D.Raycast(bottomPosition + Vector2.right * 0.1f, Vector2.down, 1.0f, groundLayer);
                        if (hitRight.collider == null)
                        {
                            walkingBehavior = WalkingBehavior.SmartLeft;
                            nextChangeTime = Time.time + changeCooldown;
                        }
                        else
                        {
                            rb.velocity = new Vector2(speed, rb.velocity.y);
                        }
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
