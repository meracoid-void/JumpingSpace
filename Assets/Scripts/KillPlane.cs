using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
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

            // Trigger the respawn mechanism
            timerAndKill.timer = 0.0f; 
        }
    }
}
