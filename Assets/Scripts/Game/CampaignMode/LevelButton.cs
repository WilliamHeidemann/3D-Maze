using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.CampaignMode
{
    public class LevelButton : MonoBehaviour
    {
        public int Seed { get; set; }
        public CampaignModeLogic.World World { get; set; }
        public LockedStatus LevelLockedStatus { get; set; }
        [SerializeField] private MazeSpawner mazeSpawner;
        [SerializeField] private GameObject levelSelectCanvas;
        [SerializeField] private GameObject campaignCanvas;

        public void StartLevel()
        {
            if (LevelLockedStatus == LockedStatus.Locked) return;
            levelSelectCanvas.SetActive(false);
            campaignCanvas.SetActive(true);
            var dimensions = GetDimensions();
            mazeSpawner.SpawnMaze(dimensions.Item1, dimensions.Item2, dimensions.Item3);
        }

        private (int, int, int) GetDimensions()
        {
            var lowWidth = World switch
            {
                CampaignModeLogic.World.SmallWorld => 1,
                CampaignModeLogic.World.Regular => 2,
                CampaignModeLogic.World.Chunks => 4,
                CampaignModeLogic.World.LongIsland => 1,
                CampaignModeLogic.World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            var highWidth = World switch
            {
                CampaignModeLogic.World.SmallWorld => 4,
                CampaignModeLogic.World.Regular => 4,
                CampaignModeLogic.World.Chunks => 6,
                CampaignModeLogic.World.LongIsland => 2,
                CampaignModeLogic.World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var lowHeight = World switch
            {
                CampaignModeLogic.World.SmallWorld => 1,
                CampaignModeLogic.World.Regular => 2,
                CampaignModeLogic.World.Chunks => 4,
                CampaignModeLogic.World.LongIsland => 1,
                CampaignModeLogic.World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            var highHeight = World switch
            {
                CampaignModeLogic.World.SmallWorld => 3,
                CampaignModeLogic.World.Regular => 4,
                CampaignModeLogic.World.Chunks => 6,
                CampaignModeLogic.World.LongIsland => 3,
                CampaignModeLogic.World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            var lowDepth = World switch
            {
                CampaignModeLogic.World.SmallWorld => 1,
                CampaignModeLogic.World.Regular => 2,
                CampaignModeLogic.World.Chunks => 4,
                CampaignModeLogic.World.LongIsland => 5,
                CampaignModeLogic.World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            var highDepth = World switch
            {
                CampaignModeLogic.World.SmallWorld => 3,
                CampaignModeLogic.World.Regular => 4,
                CampaignModeLogic.World.Chunks => 6,
                CampaignModeLogic.World.LongIsland => 6,
                CampaignModeLogic.World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };

            Random.InitState(Seed);
            var width = Random.Range(lowWidth, highWidth + 1);
            var height = Random.Range(lowHeight, highHeight + 1);
            var depth = Random.Range(lowDepth, highDepth + 1);
            return (width, height, depth);
        }

        public enum LockedStatus
        {
            Locked,
            Unlocked
        }
    }
}