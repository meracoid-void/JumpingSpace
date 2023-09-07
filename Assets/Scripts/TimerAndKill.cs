using TMPro;
using UnityEngine;

public class TimerAndKill : MonoBehaviour
{
    public float timer = 30.0f;  // Start time in seconds
    public GameObject player;    // Reference to the player GameObject
    public TextMeshProUGUI timerText;       // Reference to the UI Text component

    // Update is called once per frame
    void Update()
    {
        // Reduce the timer by the time since the last frame
        timer -= Time.deltaTime;

        // Update the timer text
        timerText.text = "Time Left: " + timer.ToString("F2");  // F2 means 2 decimal places

        // Check if the timer has reached zero
        if (timer <= 0)
        {
            // Trigger the "kill" action here
            KillPlayer();
        }
    }

    // Function to add time to the timer
    public void AddTime(float additionalTime)
    {
        timer += additionalTime;
    }

    // Function to "kill" the player
    private void KillPlayer()
    {
        // Implement your logic here to "kill" the player
        player.SetActive(false);

        // Optionally, you can reset or stop the timer
        timer = 0;
    }
}

