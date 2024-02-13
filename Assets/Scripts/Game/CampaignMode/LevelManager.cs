using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.CampaignMode
{
    public class LevelManager : MonoBehaviour
    {
        public int currentLevelIndex;
        public World currentWorld;
        [SerializeField] private MazeSpawner mazeSpawner;
        [SerializeField] private TextMeshProUGUI worldText;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private SaveFileManager saveFileManager;
        [SerializeField] private LevelSelectLogic levelSelectLogic;
        public void AdvanceLevel()
        {
            saveFileManager.LevelComplete(currentLevelIndex, currentWorld);
            currentLevelIndex++;
            if (currentLevelIndex < 50)
            {
                var dimensions = GetDimensions(currentLevelIndex, currentWorld);
                mazeSpawner.SpawnMaze(dimensions.Item1, dimensions.Item2, dimensions.Item3);
            }
            else
            {
                Debug.LogWarning("This should load the level select screen.");
            }
            UpdateLevelText();
        }

        public void UpdateLevelMenu()
        {
            levelSelectLogic.SetWorld(currentWorld);
        }

        public void UpdateLevelText()
        {
            var level = currentLevelIndex + 1;
            worldText.text = $"{currentWorld.AsString()}";
            levelText.text = $"Level {level}";
        }
    
        public static (int, int, int) GetDimensions(int seed, World world)
        {
            var lowWidth = world switch
            {
                World.SmallWorld => 1,
                World.Regular => 3,
                World.Chunks => 4,
                World.LongIsland => 1,
                World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            var highWidth = world switch
            {
                World.SmallWorld => 4,
                World.Regular => 4,
                World.Chunks => 6,
                World.LongIsland => 2,
                World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
        
            var lowHeight = world switch
            {
                World.SmallWorld => 1,
                World.Regular => 3,
                World.Chunks => 4,
                World.LongIsland => 1,
                World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            var highHeight = world switch
            {
                World.SmallWorld => 3,
                World.Regular => 4,
                World.Chunks => 6,
                World.LongIsland => 3,
                World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
        
            var lowDepth = world switch
            {
                World.SmallWorld => 1,
                World.Regular => 3,
                World.Chunks => 4,
                World.LongIsland => 5,
                World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            var highDepth = world switch
            {
                World.SmallWorld => 3,
                World.Regular => 4,
                World.Chunks => 6,
                World.LongIsland => 6,
                World.Massive => 6,
                _ => throw new ArgumentOutOfRangeException()
            };

            Random.InitState(seed);
            var width = Random.Range(lowWidth, highWidth + 1);
            var height = Random.Range(lowHeight, highHeight + 1);
            var depth = Random.Range(lowDepth, highDepth + 1);
            return (width, height, depth);
        }
    }
}
