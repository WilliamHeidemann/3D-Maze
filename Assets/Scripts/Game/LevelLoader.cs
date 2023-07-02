using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
    }
}
