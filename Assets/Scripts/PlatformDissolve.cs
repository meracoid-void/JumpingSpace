using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformDissolve : MonoBehaviour
{
    public bool isDissolvable = false;
    public float shrinkSpeed = 1f;
    public bool isPlayerOnPlatform = false;

    // Update is called once per frame
    void Update()
    {
        if (isPlayerOnPlatform && isDissolvable)
        {
            transform.localScale -= new Vector3(shrinkSpeed * Time.deltaTime, 0, 0);
            if (transform.localScale.x <= 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }

    // OnTriggerStay2D is called once per frame for every Collider2D other
    // that is touching the trigger (2D physics only).
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.transform.position.y > transform.position.y)
            {
                isPlayerOnPlatform = true;
                other.GetComponent<PlayerController>().ResetFallingState();
            }
        }
    }

    // OnTriggerExit2D is called when the Collider2D other exits the trigger (2D physics only).
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
        }
    }
}
