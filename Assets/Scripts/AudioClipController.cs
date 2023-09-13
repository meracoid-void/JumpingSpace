using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public static AudioClipController instance;
    public AudioClip jump;
    public AudioClip fall;
    public AudioClip playerHit; // New AudioClip for when the player is hit
    public AudioClip powerUp;
    private AudioSource source;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayJump()
    {
        source.PlayOneShot(jump);
    }
    public void PlayFall()
    {
        source.PlayOneShot(fall);
    }
    // New method for when the player is hit
    public void PlayPlayerHit()
    {
        source.Pause();  // Pause all other audio
        source.PlayOneShot(playerHit);  // Play the player hit sound

        // Resume all audio after a short delay
        StartCoroutine(ResumeAudioAfterDelay(1.0f)); // You can change the delay duration here
    }

    // New method for when the player gets a power-up
    public void PlayPowerUp()
    {
        source.PlayOneShot(powerUp);
    }

    // Coroutine to resume audio after a delay
    IEnumerator ResumeAudioAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        source.UnPause();
    }
}
