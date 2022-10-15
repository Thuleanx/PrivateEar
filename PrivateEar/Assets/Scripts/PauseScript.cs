using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pauseScript : MonoBehaviour
{
    public GameObject pauseMenu;
    float targetPos = 0f;


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.GetComponent<Transform>().position.Set(0, -10, 0);
        //PauseAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PauseAnimation()
    {
        //Not the most efficient way of doing this, maybe just make an animation
        while (pauseMenu.GetComponent<Transform>().position.y != targetPos)
        {
            //pauseMenu.GetComponent<Transform>().position.Set();
        }
    }

}
