using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.SurvivalMode
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timePassedText;
        [SerializeField] private GameOver gameOver;
        [SerializeField] private int timeBonus;
        [SerializeField] private int maxTimeInSeconds;
        [SerializeField] private Slider slider;
        [SerializeField] private float _timeLeft;
        [SerializeField] private bool _isRunning;
        [SerializeField] private Image sliderFillImage;
        [SerializeField] private Color sliderColor;
        [SerializeField] private Color sliderAlmostEmptyColor;
        private bool _isTimeUp;

        void Start()
        {
            _timeLeft = maxTimeInSeconds;
        }

        void Update()
        {
            if (!_isRunning) return;
            if (_isTimeUp) return;
            _timeLeft -= Time.deltaTime;
            timePassedText.text = FormatTime(_timeLeft);
            var value = _timeLeft / maxTimeInSeconds;
            slider.value = value;
            sliderFillImage.color = Color.Lerp(sliderAlmostEmptyColor, sliderColor, value * 3);
            if (_timeLeft < 0.1f)
            {
                _isTimeUp = true;
                gameOver.EndGame();
            }
        }

        public void IncrementTimer()
        {
            _timeLeft += timeBonus;
            _timeLeft = Mathf.Min(_timeLeft, maxTimeInSeconds);
        }

        public void ResetTimer()
        {
            _isTimeUp = false;
            _timeLeft = maxTimeInSeconds;
            timePassedText.text = FormatTime(_timeLeft);
        }

        public void StartTimer(bool running)
        {
            _isRunning = running;
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
