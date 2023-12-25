using TMPro;
using UnityEngine;

namespace Game
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timePassedText;
        [SerializeField] private TextMeshProUGUI timeAwardText;
        [SerializeField] private Animator animator;
        [SerializeField] private GameOver gameOver;
        [SerializeField] private int timeBonus;
        [SerializeField] private int maxTimeInSeconds;
        private float _timeLeft;
        private bool _isTimeUp;
        private static readonly int LevelComplete = Animator.StringToHash("LevelComplete");

        void Start()
        {
            _timeLeft = 10000;//maxTimeInSeconds;
            timeAwardText.text = $"+{timeBonus} seconds";
        }

        void Update()
        {
            if (_isTimeUp) return;
            _timeLeft -= Time.deltaTime;
            timePassedText.text = FormatTime(_timeLeft);
            if (_timeLeft < 1)
            {
                _isTimeUp = true;
                gameOver.EndGame();
            }
        }

        public void IncrementTimer()
        {
            _timeLeft += timeBonus;
            _timeLeft = Mathf.Min(_timeLeft, maxTimeInSeconds);
            animator.SetTrigger(LevelComplete);
        }

        public void ResetTimer()
        {
            _isTimeUp = false;
            _timeLeft = maxTimeInSeconds;
            timePassedText.text = FormatTime(_timeLeft);
        }

        private static string FormatTime(float time)
        {
            if (time < 0) return "00:00";
            var minutes = (int)(time / 60);
            var seconds = (int)(time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }

    }
}
