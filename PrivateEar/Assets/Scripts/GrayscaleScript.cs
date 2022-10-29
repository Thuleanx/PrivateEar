using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrayscaleScript : MonoBehaviour
{
    private Image image;
    private bool buttonClicked;

    public int interpolationFramesCount = 1000;
    private int elapsedFrames = 0;

    // Start is called before the first frame update
    void Start()
    {
        image = null;
        buttonClicked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonClicked && image != null)
        {
            float interpolationRatio = (float)elapsedFrames / interpolationFramesCount;

            float minValue = image.material.GetFloat("_slider");
            float newSliderValue = Mathf.Lerp(minValue, 1, interpolationRatio);
            image.material.SetFloat("_slider", newSliderValue);

            elapsedFrames = (elapsedFrames + 1) % (interpolationFramesCount + 1);
        }
    }

    public void taskOnClick(Image inputImage)
    {
        image = inputImage;
        buttonClicked = true;
    }
}
