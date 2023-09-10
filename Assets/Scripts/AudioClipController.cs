using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipController : MonoBehaviour
{
    public static AudioClipController instance;
    public AudioClip jump;
    public AudioClip fall;
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
}
