using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public static AudioClipController instance;
    public AudioClip jump;
    public AudioClip fall;
    public AudioClip powerUp;
    private AudioSource source;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Ensure the instance isn't destroyed when loading a new scene
        }
        else
        {
            Destroy(gameObject);
            return;  // Destroy the current instance and exit the Awake function
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        if (source == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Your code here
    }

    public void PlayJump()
    {
        if (source != null && jump != null)
        {
            source.PlayOneShot(jump);
        }
        else
        {
            Debug.LogError("Either AudioSource or Jump AudioClip is null.");
        }
    }

    public void PlayFall()
    {
        if (source != null && fall != null)
        {
            source.PlayOneShot(fall);
        }
        else
        {
            Debug.LogError("Either AudioSource or Fall AudioClip is null.");
        }
    }

    // New method to play the power-up sound
    public void PlayPowerUp()
    {
        if (source != null && powerUp != null)
        {
            source.PlayOneShot(powerUp);
        }
        else
        {
            Debug.LogError("Either AudioSource or PowerUp AudioClip is null.");
        }
    }
}