using System;
using System.IO;
using UnityEngine;

namespace Game.CampaignMode
{
    public class SaveFileManager : MonoBehaviour
    {
        [SerializeField] private int smallLevelsCompleted; 
        [SerializeField] private int regularLevelsCompleted; 
        [SerializeField] private int chunksLevelsCompleted; 
        [SerializeField] private int longIslandLevelsCompleted; 
        [SerializeField] private int massiveLevelsCompleted;

        private void Start()
        {
            LoadSaveFile();
        }

        public int GetLevelsCompleted(World chosenWorld) =>
            chosenWorld switch
            {
                World.SmallWorld => smallLevelsCompleted,
                World.Regular => regularLevelsCompleted,
                World.Chunks => chunksLevelsCompleted,
                World.LongIsland => longIslandLevelsCompleted,
                World.Massive => massiveLevelsCompleted,
                _ => throw new ArgumentOutOfRangeException(nameof(chosenWorld), chosenWorld, null)
            };

        public void LevelComplete(int levelId, World world)
        {
            if (levelId != GetLevelsCompleted(world)) return;
            switch (world)
            {
                case World.SmallWorld:
                    smallLevelsCompleted++;
                    break;
                case World.Regular:
                    regularLevelsCompleted++;
                    break;
                case World.Chunks:
                    chunksLevelsCompleted++;
                    break;
                case World.LongIsland:
                    longIslandLevelsCompleted++;
                    break;
                case World.Massive:
                    massiveLevelsCompleted++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(world), world, null);
            }

            UpdateCloud();
        }

        private void UpdateCloud()
        {
            print("Implementation missing.");
        }

        public string GenerateJson()
        {
            print("Implementation missing.");
            return string.Empty;
        }

        private void LoadSaveFile()
        {
            print("Implementation missing.");
        }
    }
}