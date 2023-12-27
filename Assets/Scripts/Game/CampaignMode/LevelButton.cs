using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.CampaignMode
{
    public class LevelButton : MonoBehaviour
    {
        public int Seed { get; set; }
        public World World { get; set; }
        public LockedStatus LevelLockedStatus { get; set; }
        [SerializeField] private MazeSpawner mazeSpawner;
        [SerializeField] private GameObject levelSelectCanvas;
        [SerializeField] private GameObject campaignCanvas;

        public void StartLevel()
        {
            if (LevelLockedStatus == LockedStatus.Locked) return;
            levelSelectCanvas.SetActive(false);
            campaignCanvas.SetActive(true);
            var dimensions = LevelManager.GetDimensions(Seed, World);
            mazeSpawner.SpawnMaze(dimensions.Item1, dimensions.Item2, dimensions.Item3);
        }

        public enum LockedStatus
        {
            Locked,
            Unlocked
        }
    }
}