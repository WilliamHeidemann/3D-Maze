using UnityEngine;

namespace Game.CampaignMode
{
    public class LevelButton : MonoBehaviour
    {
        public int Seed { get; set; }
        [SerializeField] private MazeSpawner mazeSpawner;

        public void StartLevel()
        {
            Random.InitState(Seed);
            var width = Random.Range(1, 7);
            var height = Random.Range(1, 7);
            var depth = Random.Range(1, 7);
            mazeSpawner.SpawnMaze(width, height, depth);
        }
    }
}