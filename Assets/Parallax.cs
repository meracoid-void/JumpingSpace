using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds; // Array of all the back- and foregrounds to be parallaxed.
    public float smoothing = 1f;   // Amount of smoothing to apply. Set this above 0.

    private float[] parallaxScales; // Proportion of the camera's movement to move the backgrounds by.
    private Transform cam;          // Reference to the main camera's transform.
    private Vector3 previousCamPos; // Position of the camera in the previous frame.

    // Start is called before the first frame update
    void Start()
    {
        // Reference to the main camera's transform
        cam = Camera.main.transform;

        // Store the position of the camera from the previous frame
        previousCamPos = cam.position;

        // Initialize the array of parallax scales
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // Calculate the parallax effect (the distance the camera has moved multiplied by the parallax scale)
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // Calculate the target position for the background
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a target position which is the background's current position but with its target x position
            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Smoothly transition between the current position and the target position
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);

            Debug.Log("Camera movement detected: " + (previousCamPos.x - cam.position.x));

            Debug.Log($"Background {i} is being moved by: {parallax}");
        }

        // Update the previous camera position to the camera's position at the end of the frame
        previousCamPos = cam.position;
    }
}
