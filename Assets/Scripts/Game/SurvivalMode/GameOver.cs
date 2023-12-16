using UnityEngine;

namespace Game
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
                _isGameOver = false;
                pauseController.enabled = true;
                gameOverScreen.SetActive(false);
                Time.timeScale = 1;
                survivalModeStarter.FirstMaze();
            }
        }

        public void EndGame()
        {
            _isGameOver = true;
            gameOverScreen.SetActive(true);
            pauseController.enabled = false;
            Time.timeScale = 0;
        }
    }
}
