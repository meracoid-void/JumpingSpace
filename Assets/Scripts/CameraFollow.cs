using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public Vector3 offset;    // Offset distance between the player and camera

    // Update is called once per frame
    void Update()
    {
        // Update the camera's position based on the player's position and the offset
        transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, offset.z);
    }
}
