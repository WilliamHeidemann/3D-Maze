using UnityEngine;

namespace Game.CampaignMode
{
    public class LevelButton : MonoBehaviour
    {
        public int Seed { get; set; }
        public World World { get; set; }
        public LockedStatus LevelLockedStatus { get; set; }
        [SerializeField] private LevelManager levelManager;
        public void StartLevel()
        {
            if (LevelLockedStatus == LockedStatus.Locked) return;
            levelManager.StartLevel(World, Seed);
        }

        public enum LockedStatus
        {
            Locked,
            Unlocked
        }
    }
}