using System.Collections;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance; // Singleton instance

    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // Offset distance between the player and camera

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Move(Vector3 position)
    {
        transform.position = position;
    }

    // Method to move the camera to a target position over a duration
    public IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.gameObject.activeInHierarchy)
        {
            // Update the camera's position based on the player's position and the offset
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
        }
    }
}
