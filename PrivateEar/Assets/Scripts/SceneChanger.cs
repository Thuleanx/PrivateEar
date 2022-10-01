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
    public Canvas fadeCanvas;
    float time = 0f;
    bool fadeOut;
    public SceneReference sceneReference;

    public void Start()
    {
        //Set fade screen to top layer
        //NOTE: THIS REQUIRES SETTING UP SPECIFIC LAYERS TO DISTINGUISH FADE
        //blackScreen.layer = LayerMask.NameToLayer("IgnoreRaycast");
        fadeOut = false;
        blackScreen.GetComponent<Image>().color = new Color(0,0,0,1);
        fadeCanvas.GetComponent<CanvasGroup>().alpha = 1;
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
        fadeCanvas.GetComponent<CanvasGroup>().alpha += fadeSpeed * Time.deltaTime;
        if (fadeCanvas.GetComponent<CanvasGroup>().alpha >= 1)
        {
            ChangeScene();
        }
    }

    void FadeIn()
    {
        fadeCanvas.GetComponent<CanvasGroup>().alpha -= fadeSpeed * Time.deltaTime;
        if (fadeCanvas.GetComponent<CanvasGroup>().alpha <= 0)
        {
            fadeOut = true;
        }
    }

    void ChangeScene()
    {
        sceneReference.LoadScene();
    }
}
