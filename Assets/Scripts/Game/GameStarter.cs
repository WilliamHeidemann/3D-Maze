using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerPrefab;
    [SerializeField] private GameObject goalPrefab;
    [SerializeField] private MazeVisualiser mazePrefab;
    [SerializeField] private CameraLock cameraLock;
    private MazeVisualiser _mazeInstance;

    private readonly int[][] _sizes =
    {
        new [] {3, 3, 3},
        new [] {4, 3, 3},
        new [] {4, 4, 3},
        new [] {4, 4, 4},
        new [] {5, 3, 3},
        new [] {5, 4, 3},
        new [] {5, 4, 4},
        new [] {5, 5, 3},
        new [] {5, 5, 4},
        new [] {5, 5, 5},
        new [] {6, 3, 3},
        new [] {6, 4, 4},
        new [] {6, 5, 4},
        new [] {6, 6, 3},
        new [] {6, 5, 5},
        new [] {6, 6, 6}
    };

    private int _levelIndex = 0;

    
    void Start() => FirstMaze();

    public void FirstMaze()
    {
        FindObjectOfType<Timer>()?.ResetTimer();
        FindObjectOfType<Points>()?.ResetPoints();
        _levelIndex = 0;
        NextMaze();
    }

    public void NextMaze()
    {
        if (_mazeInstance != null) Destroy(_mazeInstance.gameObject);
        _mazeInstance = Instantiate(mazePrefab);
        var width = _sizes[_levelIndex][0];
        var height = _sizes[_levelIndex][1];
        var depth = _sizes[_levelIndex][2];
        _levelIndex = Mathf.Min(_levelIndex + 1, _sizes.Length - 1);
        _mazeInstance.VisualizeMaze(width, height, depth);
        
        var (start, finish) = _mazeInstance.FurthestApart();
        var player = Instantiate(playerPrefab, _mazeInstance.transform);
        player.SetSquares(_mazeInstance._positions, start);
        player.ObjectiveSquare = finish;
        _mazeInstance.GetComponent<MazeRotator>().SnapToFace(start.Orientation);

        var goal = Instantiate(goalPrefab, _mazeInstance._positions[finish].position, Quaternion.identity, _mazeInstance.transform);
        goal.transform.localScale = new Vector3(1,1,1);
        
        cameraLock.SetTarget(_mazeInstance.transform);
    }
}