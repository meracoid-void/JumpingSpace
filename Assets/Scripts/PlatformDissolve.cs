using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDissolve : MonoBehaviour
{
    public bool isDissolvable = false;  // Boolean to control whether the platform can dissolve
    public float shrinkSpeed = 1f;    // Speed at which the platform should shrink
    public bool isPlayerOnPlatform = false; // Flag to check if the player is on the platform

    // Update is called once per frame
    void Update()
    {
        // If the player is on the platform and it's dissolvable, start shrinking
        if (isPlayerOnPlatform && isDissolvable)
        {
            // Shrink the platform only along the X-axis
            transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, 0, 0);

            // Optional: Remove the platform if it becomes too small
            if (transform.localScale.x <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.position.y > transform.position.y)
            {
                isPlayerOnPlatform = true;
                // Tell the player that it's not falling anymore
                other.GetComponent<PlayerController>().ResetFallingState();
            }
        }
    }

    // OnTriggerExit2D is called when the Collider2D other exits the trigger (2D physics only)
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the other collider is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }
}
