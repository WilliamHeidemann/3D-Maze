﻿using UnityEngine;

namespace Game.CampaignMode
{
    public class LevelButton : MonoBehaviour
    {
        public int Seed { get; set; }
        public World World { get; set; }
        public LockedStatus LevelLockedStatus { get; set; }
        [SerializeField] private MazeSpawner mazeSpawner;
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private GameModeManager gameModeManager;
        public void StartLevel()
        {
            if (LevelLockedStatus == LockedStatus.Locked) return;
            gameModeManager.EnterCampaignMode();
            var dimensions = LevelManager.GetDimensions(Seed, World);
            mazeSpawner.SpawnMaze(dimensions.Item1, dimensions.Item2, dimensions.Item3);
            levelManager.currentLevelIndex = Seed;
            levelManager.currentWorld = World;
            levelManager.UpdateLevelText();
        }

        public enum LockedStatus
        {
            Locked,
            Unlocked
        }
    }
}