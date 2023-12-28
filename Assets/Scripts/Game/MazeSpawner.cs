using Graph;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class MazeSpawner : MonoBehaviour
    {
        [SerializeField] private PlayerMovement playerPrefab;
        [SerializeField] private GameObject goalPrefab;
        [SerializeField] private Maze mazePrefab;
        [SerializeField] private CameraLock cameraLock;
        private Maze _mazeInstance;
        private Square _start;
        private Square _finish;
        private PlayerMovement _playerInstance;

        public void DeSpawnMaze()
        {
            if (_mazeInstance != null) Destroy(_mazeInstance.gameObject);
        }

        public void SpawnRandomMaze()
        {
            var width = Random.Range(1, 7);
            var height = Random.Range(1, 7);
            var depth = Random.Range(1, 7);
            SpawnMaze(width, height, depth);
        }
        
        public void SpawnMaze(int width, int height, int depth)
        {
            DeSpawnMaze();
            _mazeInstance = Instantiate(mazePrefab);
            _mazeInstance.VisualizeMaze(width, height, depth);
        
            (_start, _finish) = _mazeInstance.FurthestApart();
            _playerInstance = Instantiate(playerPrefab, _mazeInstance.transform);
            _playerInstance.SetSquares(_mazeInstance.Positions, _start, _finish);
            _mazeInstance.GetComponent<MazeRotator>().SnapToFace(_start.Orientation);

            var goal = Instantiate(goalPrefab, _mazeInstance.Positions[_finish].position, Quaternion.identity, _mazeInstance.transform);
            goal.transform.localScale = new Vector3(1,1,1);
        
            cameraLock.SetTarget(_mazeInstance.transform);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R)) ResetLevel();
        }

        public void ResetLevel()
        {
            if (_mazeInstance == null) return;
            _playerInstance.SetSquares(_mazeInstance.Positions, _start, _finish);
            _mazeInstance.GetComponent<MazeRotator>().SnapToFace(_start.Orientation);
        }
    }
}
