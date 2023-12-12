using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private MazeVisualiser mazePrefab;
    private MazeVisualiser _mazeInstance;

    void Start() => FirstMaze();
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
        {
            // FirstMaze();
        }
    }

    public void FirstMaze()
    {
        FindObjectOfType<Timer>()?.ResetTimer();
        FindObjectOfType<Points>()?.ResetPoints();
        NextMaze();
    }

    public void NextMaze()
    {
        if (_mazeInstance != null) Destroy(_mazeInstance.gameObject);
        _mazeInstance = Instantiate(mazePrefab);
        _mazeInstance.VisualizeMaze();
        
        var (start, finish) = _mazeInstance.FurthestApart();
        var player = Instantiate(playerPrefab, _mazeInstance.transform);
        player.SetSquares(_mazeInstance._positions, start);
        player.ObjectiveSquare = finish;
        _mazeInstance.GetComponent<MazeRotator>().SnapToFace(start.Orientation);

        var goal = Instantiate(goalPrefab, _mazeInstance._positions[finish].position, Quaternion.identity, _mazeInstance.transform);
        goal.transform.localScale = new Vector3(1,1,1);
        
        PlayTransitionAnimation();
    }
    
    private void PlayTransitionAnimation()
    {
        var transitionObject = Instantiate(goalPrefab);
        transitionObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transitionObject.AddComponent<TransitionObject>();
    }
}