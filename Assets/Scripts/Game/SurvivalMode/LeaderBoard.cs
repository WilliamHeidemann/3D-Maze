using System.Collections.Generic;
using Steamworks;
using TMPro;
using UnityEngine;

namespace Game.SurvivalMode
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> entriesNames;
        [SerializeField] private List<TextMeshProUGUI> entriesScore;

        private const string GlobalLeaderboardName = "GlobalHighscores";
        private SteamLeaderboard_t _leaderboard;
        private CallResult<LeaderboardScoresDownloaded_t> _onLeaderboardScoresDownloaded;
    
        private void Start()
        {
            if (!SteamManager.Initialized) return;
            var handle = SteamUserStats.FindLeaderboard(GlobalLeaderboardName);
            _leaderboard = new SteamLeaderboard_t(handle.m_SteamAPICall);
            _onLeaderboardScoresDownloaded = CallResult<LeaderboardScoresDownloaded_t>.Create(OnLeaderboardScoresDownloaded);
            _onLeaderboardScoresDownloaded.Set(handle);
        }

        private void OnLeaderboardScoresDownloaded(LeaderboardScoresDownloaded_t pCallback, bool bIOFailure)
        {
            if (bIOFailure) { Debug.LogError("Leaderboard fetch failed!"); return; }

            _leaderboard = pCallback.m_hSteamLeaderboard;
        
            var numEntries = Mathf.Min(pCallback.m_cEntryCount, 10);
            var entries = new List<LeaderboardEntry_t>();
            for (var i = 0; i < numEntries; i++)
            {
                SteamUserStats.GetDownloadedLeaderboardEntry(pCallback.m_hSteamLeaderboardEntries, i, out var entry, null, 0);
                entries.Add(entry);
            }
            DisplayEntries(entries);
            print("No more entries.");

            UploadScore(100);
        }

        private void DisplayEntries(List<LeaderboardEntry_t> entries)
        {
            // Make no connection or not enough entries be empty strings once game is connected to steam
            for (int i = 0; i < 10; i++)
            {
                var playerName = entries.Count > i ? entries[i].m_steamIDUser.ToString() : "Test Player";
                var playerScore = entries.Count > i ? entries[i].m_nScore.ToString() : "99999";
                entriesNames[i].text = playerName;
                entriesScore[i].text = playerScore;
            }
        }

        private void UploadScore(int score)
        {
            SteamUserStats.UploadLeaderboardScore(_leaderboard,
                ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest, score, null, 0);
            print($"Score of {score} uploaded.");
        }

        public void FetchLeaderboard()
        {
            if (!SteamManager.Initialized) return;
            var leaderboardCallback = SteamUserStats.FindLeaderboard(GlobalLeaderboardName);
            var leaderboard = new SteamLeaderboard_t(leaderboardCallback.m_SteamAPICall);
        
            var entriesCallback = SteamUserStats.DownloadLeaderboardEntries(leaderboard,
                ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 10);
    
            var entries = new SteamLeaderboardEntries_t(entriesCallback.m_SteamAPICall);
        }
    }
}
