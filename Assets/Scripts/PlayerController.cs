using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float minJumpForce = 2.0f;
    public float maxJumpForce = 5.0f;
    public float timeToReachMaxForce = 0.5f;
    public float fastFallSpeed = 10.0f;

    private Rigidbody2D rb;
    private bool isJumping = false;
    private float timeHeld = 0f;
    private bool isFalling = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isPlayingFall = false;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
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
            // Ground check
            Vector2 rayStart = transform.position + new Vector3(0, -0.5f, 0);  // Adjust the -0.5f as needed
            isGrounded = Physics2D.Raycast(rayStart, Vector2.down, 0.1f);
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal != 0)
            {
                Vector2 movement = new Vector2(horizontal, rb.velocity.y);

                if (horizontal > 0)
                {
                    spriteRenderer.flipX = false;
                }
                else
                {
                    spriteRenderer.flipX = true;
                }

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

            if (rb.velocity.y < 0 && !isGrounded)
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

            if (Input.GetButtonDown("Jump") && !isJumping && isGrounded)
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isFalling", false);
                animator.SetBool("isJumping", true);
                isJumping = true;
                timeHeld = 0f;
                AudioClipController.instance.PlayJump();
                isPlayingFall = false;
            }

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

            if (Input.GetButtonUp("Jump"))
            {
                animator.SetBool("isJumping", false);
                isJumping = false;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
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
