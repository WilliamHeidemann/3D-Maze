using UnityEngine;

namespace Game.SurvivalMode
{
    public class SurvivalModeStarter : MonoBehaviour
    {
        [SerializeField] private MazeSpawner mazeSpawner;

        private readonly int[][] _levels =
        {
            new [] {3, 3, 3},
            new [] {5, 2, 2},
        };

        private int _levelIndex = 0;
        
        public void FirstMaze()
        {
            Random.InitState((int)System.DateTime.Now.Ticks);
            var timer = FindObjectOfType<Timer>();
            timer.ResetTimer();
            timer.StartTimer(true);
            FindObjectOfType<Points>()?.ResetPoints();
            _levelIndex = 0;
            NextMaze();
        }

        public void NextMaze()
        {
            
            if (_levelIndex >= _levels.Length) mazeSpawner.SpawnRandomMaze();
            else
            {
                var width = _levels[_levelIndex][0];
                var height = _levels[_levelIndex][1];
                var depth = _levels[_levelIndex][2];
                mazeSpawner.SpawnMaze(width, height, depth);
            }
            _levelIndex++;
        }
    }
}