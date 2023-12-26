using UnityEngine;

namespace Game.SurvivalMode
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject pauseScreen; 
        private bool _isPaused;
    
    
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!_isPaused) Pause();
                else Resume();
            }
        }

        private void Pause()
        {
            _isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
        }
    
        private void Resume()
        {
            _isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
        }
    }
}
