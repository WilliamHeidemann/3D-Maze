using UnityEngine;

namespace Game.SurvivalMode
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private SurvivalModeStarter survivalModeStarter;
        [SerializeField] private PauseController pauseController;
        private bool _isGameOver;
        void Update()
        {
            if (_isGameOver == false) return;
            if (Input.GetKey(KeyCode.Space))
            {
                RestartGame();
            }
        }

        public void RestartGame()
        {
            _isGameOver = false;
            pauseController.enabled = true;
            gameOverScreen.SetActive(false);
            Time.timeScale = 1;
            SoundManager.Instance.StartMusic();
            survivalModeStarter.FirstMaze();
        }

        public void EndGame()
        {
            _isGameOver = true;
            gameOverScreen.SetActive(true);
            pauseController.enabled = false;
            SoundManager.Instance.StopMusic();
            Time.timeScale = 0;
        }
    }
}
