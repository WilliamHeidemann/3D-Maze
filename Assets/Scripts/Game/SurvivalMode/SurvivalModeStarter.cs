using System;
using UnityEngine;

namespace Game
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

    
        // void Start() => FirstMaze();

        private void OnEnable() => FirstMaze();

        public void FirstMaze()
        {
            FindObjectOfType<Timer>()?.ResetTimer();
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
            // _levelIndex = Mathf.Min(_levelIndex + 1, _levels.Length - 1);
        }
    }
}