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
            new [] {4, 3, 3},
            new [] {4, 4, 3},
            new [] {4, 4, 4},
            new [] {4, 2, 2},
            new [] {4, 4, 4},
            new [] {5, 3, 3},
            new [] {5, 4, 3},
            new [] {5, 4, 4},
            new [] {5, 5, 1},
            new [] {5, 5, 3},
            new [] {5, 5, 4},
            new [] {6, 1, 1},
            new [] {5, 5, 5},
            new [] {6, 3, 3},
            new [] {6, 4, 4},
            new [] {6, 5, 4},
            new [] {6, 6, 3},
            new [] {6, 5, 5},
            new [] {6, 6, 6},
            new [] {6, 6, 1},
            new [] {6, 6, 6},
            new [] {1, 1, 1},
            new [] {6, 6, 6},
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
            var width = _levels[_levelIndex][0];
            var height = _levels[_levelIndex][1];
            var depth = _levels[_levelIndex][2];
            _levelIndex = Mathf.Min(_levelIndex + 1, _levels.Length - 1);
            mazeSpawner.SpawnMaze(width, height, depth);
        }
    }
}