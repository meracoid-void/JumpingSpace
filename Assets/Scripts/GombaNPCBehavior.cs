using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WalkingBehavior
{
    Left = 0,
    Right = 1,
    StandStill = 2,
}

public class GombaNPCBehavior : MonoBehaviour
{
    public WalkingBehavior walkingBehavior;
    public float speed = 1.0f; // Speed of the enemy
    private Rigidbody2D rb; // Rigidbody2D component for physics

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Rigidbody2D component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Movement based on WalkingBehavior
        switch (walkingBehavior)
        {
            case WalkingBehavior.Left:
                rb.velocity = new Vector2(-speed, rb.velocity.y);
                break;
            case WalkingBehavior.Right:
                rb.velocity = new Vector2(speed, rb.velocity.y);
                break;
            case WalkingBehavior.StandStill:
                rb.velocity = new Vector2(0, rb.velocity.y);
                break;
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
