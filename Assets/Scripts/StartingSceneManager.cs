using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingSceneManager : MonoBehaviour
{
    public static StartingSceneManager instance;
    public float timer = 60.0f;  // Start time in seconds
    public string sceneToLoad;

    private void Awake()
    {
        if (instance == null)
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

    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            // Reduce the timer by the time since the last frame
            timer -= Time.deltaTime;
        }
        if (timer <= 0)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
