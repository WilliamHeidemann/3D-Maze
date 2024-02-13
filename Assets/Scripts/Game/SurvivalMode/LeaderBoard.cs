using System;
using System.Collections.Generic;
using System.Linq;
using Steamworks;
using TMPro;
using UnityEngine;
using Steamworks.Data;

namespace Game.SurvivalMode
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> entriesNames;
        [SerializeField] private List<TextMeshProUGUI> entriesScore;

        private Leaderboard leaderboard;
        
        private async void Start()
        {
            var nullableLeaderboard = await SteamUserStats.FindLeaderboardAsync("GlobalHighScores");
            if (nullableLeaderboard.HasValue == false) return;
            leaderboard = nullableLeaderboard.Value;
            DisplayGlobalHighScores();
        }
        
        private async void DisplayGlobalHighScores()
        {
            var globalScores = await leaderboard.GetScoresAsync(10);
            for (int i = 0; i < globalScores.Length; i++)
            {
                entriesNames[i].text = globalScores[i].User.Name;
                entriesScore[i].text = globalScores[i].Score.ToString();
            }
        }

        public async void SubmitScore(int scoreToSubmit)
        {
            var nullableResult = await leaderboard.SubmitScoreAsync(scoreToSubmit);
            if (nullableResult.HasValue == false) return;
            DisplayGlobalHighScores();
        }
    }
}
