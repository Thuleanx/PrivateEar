using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseCanvas;

    static int PAUSE = 0, ENDPAUSE = 1, NEUTRAL = 2;
    int pauseState = 2;

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.GetComponent<Transform>().position.Set(0, -10, 0);
        pauseCanvas.SetActive(false);
        //PauseAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseState==PAUSE)
        {
            PauseAnimation();
        }
        if (pauseState == ENDPAUSE)
        {
            ExitPauseAnimation();
        }
    }

    public void TriggerPauseScreen() => pauseState = PAUSE;

    public void TriggerExitPause() => pauseState = ENDPAUSE;

    void PauseAnimation()
    {
        //This is if we want a pause animation from below into position
        //Additionally, do we want to blur the background?
        /*if (false)//pauseMenu.GetComponent<Transform>().position.y != targetPos)
        {
            float y = pauseMenu.GetComponent<Transform>().position.y + 1;
            pauseMenu.GetComponent<Transform>().position.Set(0,y,0);
        }
        else
        {
            pauseCanvas.SetActive(true);
            pauseState = NEUTRAL;
        }*/
        pauseCanvas.SetActive(true);
        pauseState = NEUTRAL;
    }

    void ExitPauseAnimation()
    {
        pauseCanvas.SetActive(false);

    }

}
