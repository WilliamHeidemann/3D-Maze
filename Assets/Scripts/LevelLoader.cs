using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public MazeVisualiser maze;
    public void NextLevel()
    {
        // maze.transform.parent = null;
        // var childCount = maze.transform.childCount;
        // for (int i = 0; i < childCount; i++)
        // {
        //     Destroy(maze.transform.GetChild(0).gameObject);
        // }
        // maze.CreateFaceCube();
        // maze.SetPlayerAndObjective();
        SceneManager.LoadScene(0);
    }
}
