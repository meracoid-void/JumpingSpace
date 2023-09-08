using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool isCheckpoint; // Boolean to determine if the power-up acts as a checkpoint
    public float addTime; // Amount of time to add to the timer when the power-up is not a checkpoint

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // OnTriggerEnter2D is called when the Collider2D other enters the trigger (2D physics only)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Reference to the singleton instance of TimerAndKill
            TimerAndKill timerAndKill = TimerAndKill.instance;

            if (isCheckpoint)
            {
                // Reset the timer to the starting time
                timerAndKill.timer = timerAndKill.startingTimer;

                // Update the respawn point
                timerAndKill.respawnPoint = transform.position;
            }
            else
            {
                // Increase the timer
                timerAndKill.timer += addTime;
            }

            // Destroy the power-up
            Destroy(gameObject);
        }
    }

}
