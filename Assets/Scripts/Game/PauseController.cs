using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen; 
    private bool _isPaused;
    
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_isPaused) Pause();
            else Resume();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
    }

    private void Pause()
    {
        _isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0;
        //FindObjectOfType<PlayerMovement>().enabled = false;
    }
    
    private void Resume()
    {
        _isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
        //FindObjectOfType<PlayerMovement>().enabled = true;
    }
}
