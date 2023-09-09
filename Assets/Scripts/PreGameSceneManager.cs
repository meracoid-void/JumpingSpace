using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreGameSceneManager : MonoBehaviour
{
    public string startingScene;
    public Image imageObj;
    public TextMeshProUGUI textObj;
    public List<Sprite> sceneSprites;
    public List<string> sceneDialog;
    public float textDelay = 0.1f;
    public float sceneDelay = 1.0f;

    private bool inCutscene = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(startingScene);
        }

        if(!inCutscene)
        {
            inCutscene = true;
            StartCoroutine(TypeDialog());
        }
    }

    IEnumerator TypeDialog()
    {
        for (int i = 0; i < sceneSprites.Count; i++)
        {
            if (sceneSprites[i] != null)
            {
                imageObj.sprite = sceneSprites[i];
            }
            if (sceneDialog[i] != null)
            {
                textObj.text = "";
                foreach (char c in sceneDialog[i])
                {
                    textObj.text += c;
                    yield return new WaitForSeconds(textDelay);
                }
            }
            yield return new WaitForSeconds(sceneDelay);
        }
        SceneManager.LoadScene(startingScene);
        
    }
}