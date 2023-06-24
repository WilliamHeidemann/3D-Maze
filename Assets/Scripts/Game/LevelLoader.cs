using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public MazeVisualiser maze;
    public void NextLevel()
    {
        SceneManager.LoadScene(0);
    }
}
