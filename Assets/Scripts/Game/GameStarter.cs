using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private MazeVisualiser mazePrefab;
    private MazeVisualiser _mazeInstance;

    void Start() => StartLevel();
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.R))
        {
            StartLevel();
        }
    }

    public void StartLevel()
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
    }
}
