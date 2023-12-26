using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.CampaignMode
{
    public class CampaignModeLogic : MonoBehaviour
    {
        public World world;
        public List<Image> levelImages;
        [SerializeField] private Color completed;
        [SerializeField] private Color open;
        [SerializeField] private Color locked;
        [SerializeField] private TextMeshProUGUI title;

        public void SetWorld(World chosenWorld)
        {
            if (world == chosenWorld) return;
            world = chosenWorld;
            UpdateLevelImages(chosenWorld);
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

        private void UpdateLevelImages(World chosenWorld)
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

            Random.InitState((int)chosenWorld);
            int levelsCompleted = Random.Range(0, 50);
        
            for (var i = 0; i < levelImages.Count; i++)
            {
                levelImages[i].color = levelsCompleted switch
                {
                    _ when i < levelsCompleted => completed,
                    _ when i == levelsCompleted => open,
                    _ when i > levelsCompleted  => locked,
                    _ => throw new ArgumentOutOfRangeException() // Not possible
                };
            }
        }

        public enum World
        {
            SmallWorld,
            Regular,
            Chunks,
            LongIsland,
            Massive
        }
    }
}
