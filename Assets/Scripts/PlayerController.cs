using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;  // Movement speed
    public float jumpForce = 5.0f;  // Jump force
    private Rigidbody2D rb;     // Reference to the Rigidbody2D component
    private bool isJumping = false; // Flag to check if the player is currently jumping

    // Start is called before the first frame update
    void Start()
    {
        // Get the Rigidbody2D component attached to this GameObject
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get horizontal input only
        float horizontal = Input.GetAxis("Horizontal");

        // Create a 2D movement vector for the horizontal direction
        Vector2 movement = new Vector2(horizontal, rb.velocity.y);

        // Apply movement to the Rigidbody2D
        rb.velocity = movement * new Vector2(speed, 1);

        // Check for jump input
        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            isJumping = true;
        }
    }

    // OnCollisionEnter2D is called when this collider/rigidbody has begun touching another rigidbody/collider
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset the isJumping flag when player collides with ground
        isJumping = false;
    }
}
