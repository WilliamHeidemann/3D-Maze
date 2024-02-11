using System;
using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.CampaignMode
{
    public class LevelSelectLogic : MonoBehaviour
    {
        [SerializeField] private World world;
        [SerializeField] private List<LevelButton> levelButtons;
        [SerializeField] private Image[] levelImages;
        [SerializeField] private Color completed;
        [SerializeField] private Color open;
        [SerializeField] private Color locked;
        [SerializeField] private TextMeshProUGUI title;

        private void Start()
        {
            SetWorld(world);
            for (int i = 0; i < levelButtons.Count; i++)
            {
                levelButtons[i].Seed = i;
            }
        }

        public void TrySetWorld(World chosenWorld)
        {
            if (world == chosenWorld) return;
            SetWorld(chosenWorld);
        }

        private void SetWorld(World chosenWorld)
        {
            world = chosenWorld;
            levelButtons.ForEach(button => button.World = chosenWorld);
            UpdateLevelLockedStatus(chosenWorld);
            UpdateTitle(chosenWorld);
        }

        private void UpdateTitle(World chosenWorld)
        {
            title.text = chosenWorld switch
            {
                World.SmallWorld => "Small World",
                World.Regular => "Regular",
                World.Chunks => "Chunks",
                World.LongIsland => "Long Island",
                World.Massive => "Massive",
                _ => throw new ArgumentOutOfRangeException(nameof(chosenWorld), chosenWorld, null)
            };
        }

        private void UpdateLevelLockedStatus(World chosenWorld)
        {
            // if (!SteamManager.Initialized) return;

            var id = chosenWorld switch
            {
                World.SmallWorld => "levelsCompleted0",
                World.Regular => "levelsCompleted1",
                World.Chunks => "levelsCompleted2",
                World.LongIsland => "levelsCompleted3",
                World.Massive => "levelsCompleted4",
                _ => throw new ArgumentOutOfRangeException(nameof(chosenWorld), chosenWorld, null)
            };

            // if (!SteamUserStats.GetStat(id, out int levelsCompleted)) return;
            int levelsCompleted = 0;
            
            // Debugging without steam manager initialized:
            // Random.InitState((int)chosenWorld);
            // int levelsCompleted = Random.Range(0, 50);
        
            for (var i = 0; i < levelImages.Length; i++)
            {
                levelImages[i].color = levelsCompleted switch
                {
                    _ when i < levelsCompleted => completed,
                    _ when i == levelsCompleted => open,
                    _ when i > levelsCompleted  => locked,
                    _ => throw new ArgumentOutOfRangeException() // Not possible
                };
            }
            
            for (var i = 0; i < levelButtons.Count; i++)
            {
                levelButtons[i].LevelLockedStatus = i <= levelsCompleted
                    ? LevelButton.LockedStatus.Unlocked
                    : LevelButton.LockedStatus.Locked;
            }
        }
    }
}
