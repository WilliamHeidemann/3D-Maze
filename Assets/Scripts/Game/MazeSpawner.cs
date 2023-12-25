using UnityEngine;

namespace Game
{
    public class MazeSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerPrefab;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private Maze mazePrefab;
        [SerializeField] private CameraLock cameraLock;
        private Maze _mazeInstance;

        public void DeSpawnMaze()
        {
            if (_mazeInstance != null) Destroy(_mazeInstance.gameObject);
        }
    
        public void SpawnMaze(int width, int height, int depth)
        {
            DeSpawnMaze();
            _mazeInstance = Instantiate(mazePrefab);
            _mazeInstance.VisualizeMaze(width, height, depth);
        
            var (start, finish) = _mazeInstance.FurthestApart();
            var player = Instantiate(playerPrefab, _mazeInstance.transform);
            player.SetSquares(_mazeInstance._positions, start, finish);
            _mazeInstance.GetComponent<MazeRotator>().SnapToFace(start.Orientation);

            var goal = Instantiate(goalPrefab, _mazeInstance._positions[finish].position, Quaternion.identity, _mazeInstance.transform);
            goal.transform.localScale = new Vector3(1,1,1);
        
            cameraLock.SetTarget(_mazeInstance.transform);
        }
    }
}
