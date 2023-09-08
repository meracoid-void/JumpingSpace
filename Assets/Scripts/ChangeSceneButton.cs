using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    public string sceneToLoad;  // Name of the scene to load

    // This function will be called when the button is clicked
    public void OnButtonClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
