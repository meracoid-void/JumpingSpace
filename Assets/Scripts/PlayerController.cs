using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;  // Movement speed
    public float minJumpForce = 2.0f;  // Minimum jump force for a quick tap
    public float maxJumpForce = 5.0f;  // Maximum jump force for holding the button
    public float timeToReachMaxForce = 0.5f; // Time in seconds to reach max jump force when button is held
    public float fastFallSpeed = 10.0f;

    private Rigidbody2D rb;     // Reference to the Rigidbody2D component
    private bool isJumping = false; // Flag to check if the player is currently jumping
    private float timeHeld = 0f;  // Time the jump button is held
    private bool isFalling = false;  // Flag to check if the player is currently falling
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isPlayingFall = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Transform childTransform = transform.Find("PlayerSprite");
        spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.isPlayerRespawning)
        {

            // Get horizontal input only
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal != 0)
            {
                // Create a 2D movement vector for the horizontal direction
                Vector2 movement = new Vector2(horizontal, rb.velocity.y);

                if (horizontal > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }

                // Apply movement to the Rigidbody2D
                rb.velocity = movement * new Vector2(speed, 1);
                animator.SetBool("isWalking", true);
                if (!isJumping && !isFalling)
                {
                    isPlayingFall = false;
                }
            }
            else
            {
                animator.SetBool("isWalking", false);
            }


            // Update the isFalling flag based on the vertical velocity
            if (rb.velocity.y < 0)
            {
                if (!isPlayingFall)
                {
                    isPlayingFall = true;
                    AudioClipController.instance.PlayFall();
                }
                animator.SetBool("isWalking", false);
                animator.SetBool("isFalling", true);
                animator.SetBool("isJumping", false);
                isFalling = true;
            }
            else
            {
                animator.SetBool("isFalling", false);
                isFalling = false;
            }

            // Detect if the jump button is pressed
            if (Input.GetButtonDown("Jump") && !isJumping && !isFalling)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", true);
                isJumping = true;
                timeHeld = 0f;
                AudioClipController.instance.PlayJump();
                isPlayingFall = false;
            }

            // Detect if the jump button is being held
            if (Input.GetButton("Jump") && isJumping && !isFalling)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", true);
                timeHeld += Time.deltaTime;
                float percentage = Mathf.Clamp01(timeHeld / timeToReachMaxForce);
                if (percentage < 1)
                {
                    float currentJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, percentage);
                    rb.velocity = new Vector2(rb.velocity.x, currentJumpForce);
                }
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                rb.velocity = new Vector2(rb.velocity.x, -fastFallSpeed);
            }

            // Detect if the jump button is released
            if (Input.GetButtonUp("Jump"))
            {
                animator.SetBool("isJumping", false);
                isJumping = false;
            }
        }
    }


    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Check the collision from above
            if (collision.contacts[0].normal.y > 0.5f)
            {
                isJumping = false;
                isFalling = false;
                animator.SetBool("isFalling", false);
            }
        }
    }

    public void ResetFallingState()
    {
        isFalling = false;
        animator.SetBool("isFalling", false);
    }
}