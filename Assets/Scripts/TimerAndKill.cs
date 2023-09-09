using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerAndKill : MonoBehaviour
{
    public static TimerAndKill instance;

    public float startingTimer = 30.0f;
    public float timer = 30.0f;  // Start time in seconds
    public GameObject player;    // Reference to the player GameObject
    public TextMeshProUGUI timerText;       // Reference to the UI Text component
    public Image screen;
    public float respawnDelay = 3.0f; // Time to wait before respawning the player
    public Vector3 respawnPoint;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            // Reduce the timer by the time since the last frame
            timer -= Time.deltaTime;

            float alphaValue = 1.0f - (timer / 30.0f);
            screen.color = new Color(0, 0, 0, alphaValue);

            // Update the timer text
            timerText.text = "Time Left: " + timer.ToString("F2");  // F2 means 2 decimal places
        }

        // Check if the timer has reached zero
        if (timer <= 0)
        {
            // Trigger the "kill" action here
            StartCoroutine(RespawnPlayer());
        }
    }

    // Function to add time to the timer
    public void AddTime(float additionalTime)
    {
        timer += additionalTime;
    }

    // Function to "kill" the player
    public IEnumerator RespawnPlayer()
    {
        // Disable the player
        player.SetActive(false);
        if (!GameManager.Instance.isPlayerRespawning)
        {
            GameManager.Instance.playerLives -= 1;
            GameManager.Instance.ResetGameAssets();
        }

        GameManager.Instance.isPlayerRespawning = true;


        if(GameManager.Instance.playerLives == 0)
        {
            SceneManager.LoadScene("StartingScene");
            yield break;
        }

        // Start moving the camera to the respawn point
        StartCoroutine(CameraFollow.Instance.MoveToPosition(respawnPoint + CameraFollow.Instance.offset, respawnDelay));

        // Wait for a delay before respawning
        yield return new WaitForSeconds(respawnDelay);

        // Respawn the player at the respawnPoint
        player.transform.position = respawnPoint;

        // Enable the player
        player.SetActive(true);
        timer = startingTimer;
        GameManager.Instance.isPlayerRespawning = false;
    }
}

