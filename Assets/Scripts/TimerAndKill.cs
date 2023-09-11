using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerAndKill : MonoBehaviour
{
    public static TimerAndKill instance;

    public float startingTimer = 30.0f;
    public float timer = 30.0f;  // Start time in seconds
    public GameObject player;    // Reference to the player GameObject
    public TextMeshProUGUI timerText;       // Reference to the UI Text component
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI deathText;
    public Image screen;
    public float respawnDelay = 3.0f; // Time to wait before respawning the player
    public Vector3 respawnPoint;
    public Vector3 startingScreenScale;
    public Vector3 finalScreenScale;

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
        if (timer > 0 && !GameManager.Instance.isPlayerRespawning)
        {
            // Reduce the timer by the time since the last frame
            timer -= Time.deltaTime;

            float lerpValue = 1.0f - (timer / startingTimer);
            screen.transform.localScale = Vector3.Lerp(startingScreenScale, finalScreenScale, lerpValue);

            // Update the timer text
            timerText.text = "Time Left: " + timer.ToString("F2");  // F2 means 2 decimal places
            lifeText.text = "Lives: " + GameManager.Instance.playerLives;
        }

        // Check if the timer has reached zero
        if (timer <= 0 && !GameManager.Instance.isPlayerRespawning)
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
        if (!GameManager.Instance.isPlayerRespawning)
        {
            GameManager.Instance.playerLives -= 1;
        }

        GameManager.Instance.isPlayerRespawning = true;
        screen.transform.localScale = finalScreenScale;
        deathText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        deathText.gameObject.SetActive(false);
        // Disable the player
        player.SetActive(false);

        if(GameManager.Instance.playerLives == 0)
        {
            SceneManager.LoadScene("StartingScene");
            yield break;
        }

        if (!GameManager.Instance.isPlayerRespawning)
        {
            GameManager.Instance.ResetGameAssets();
        }

        // Start moving the camera to the respawn point
        StartCoroutine(CameraFollow.Instance.MoveToPosition(respawnPoint + CameraFollow.Instance.offset, respawnDelay));
        //CameraFollow.Instance.Move(respawnPoint + CameraFollow.Instance.offset);

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

