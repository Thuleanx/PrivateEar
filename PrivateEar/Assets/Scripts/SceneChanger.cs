using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PrivateEar.Utils;

public class SceneChanger : MonoBehaviour
{
    public float fadeSpeed = 1f;
    public GameObject blackScreen;
    float time = 0f;
    bool fadeOut = false;
    public SceneReference sceneReference;

    public void Start()
    {
        //Set fade screen to top layer
        //NOTE: THIS REQUIRES SETTING UP SPECIFIC LAYERS TO DISTINGUISH FADE
        //blackScreen.layer = LayerMask.NameToLayer("IgnoreRaycast");
        blackScreen.GetComponent<SpriteRenderer>().color = new Color(0,0,0,1);
    }

    public void Update()
    {
        if (!fadeOut && time > 2f)
        {
            FadeIn();
        }
       
        if (time > 6f && fadeOut) {
            //Debug.Log("Change Scene");
            FadeOut();
        }

        time += Time.deltaTime;
        
    }

    void FadeOut()
    {
        //Alternative Option: alter canvas group alpha

        Color fadeColor = blackScreen.GetComponent<Image>().color;
        //Debug.Log("Fading out");
        fadeColor.a += fadeSpeed * Time.deltaTime;
        blackScreen.GetComponent<Image>().color = fadeColor;
    }

    void FadeIn()
    {
        Color fadeColor = blackScreen.GetComponent<Image>().color;
        //Debug.Log("Fading in");
        fadeColor.a -= fadeSpeed * Time.deltaTime;
        blackScreen.GetComponent<Image>().color = fadeColor;
        if (fadeColor.a <= 0)
        {
            fadeOut = true;
        }
    }

    void ChangeScene()
    {
        //Changes scene sequentially, if other order is required, implement scene specific function
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Use sceneReference (safe as index can change)
    }
}
