using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Steamworks;
using UnityEngine;

// En class som lokalt holder på hvor mange baner er klaret for hver bane. 
// Klassen opdateres lokalt når en base klares og opdaterer filen i cloud. 
// Det gør den ved at generer en JSON baseret på sine fields, som den gemmer. 
// Når en spiller logger ind, skal klassen starte med at hente den korrekte save json file i cloud.
// Filen læses, og dataene læses ind i den lokale klasse. 
// Lav den lokale klasse som en monobehaviour inkl. et editor-script. 
// Editor script bruges til at teste generation af JSON-fil og læsning af selv samme, samt upload af data.

namespace Game.CampaignMode
{
    public class SaveFileManager : MonoBehaviour
    {
        [SerializeField] private int smallLevelsCompleted; 
        [SerializeField] private int regularLevelsCompleted; 
        [SerializeField] private int chunksLevelsCompleted; 
        [SerializeField] private int longIslandLevelsCompleted; 
        [SerializeField] private int massiveLevelsCompleted;
        private static string UniqueLevelDataFileName => SteamClient.SteamId + "/levelData";
        [SerializeField] private LevelSelectLogic levelSelectLogic;

        private void Start()
        {
            LoadSaveFile();
            levelSelectLogic.Initialize();
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

        public void UpdateCloud()
        {
            if (SteamClient.IsValid == false) return;
            if (SteamClient.IsLoggedOn == false) return;
            var jsonData = GenerateJson();
            var bytes = Encoding.UTF8.GetBytes(jsonData);
            var memoryStream = new MemoryStream(bytes);
            var writeSuccess = SteamRemoteStorage.FileWrite(UniqueLevelDataFileName, memoryStream.ToArray());
        }

        private string GenerateJson()
        {
            var gameData = new GameData
            {
                small = smallLevelsCompleted,
                regular = regularLevelsCompleted,
                chunks = chunksLevelsCompleted,
                longIsland = longIslandLevelsCompleted,
                massive = massiveLevelsCompleted
            };
            return JsonConvert.SerializeObject(gameData);
        }

        public void LoadSaveFile()
        {
            if (SteamRemoteStorage.FileExists(UniqueLevelDataFileName) == false) return;
            var content = SteamRemoteStorage.FileRead(UniqueLevelDataFileName);
            var json = Encoding.UTF8.GetString(content);
            var gameData = JsonConvert.DeserializeObject<GameData>(json);
            if (gameData == null) return;
            smallLevelsCompleted = gameData.small;
            regularLevelsCompleted = gameData.regular;
            chunksLevelsCompleted = gameData.chunks;
            longIslandLevelsCompleted = gameData.longIsland;
            massiveLevelsCompleted = gameData.massive;
        }
    }

    public class GameData
    {
        public int small;
        public int regular;
        public int chunks;
        public int longIsland;
        public int massive;
    }
}