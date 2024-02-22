using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.SurvivalMode
{
    public class Points : MonoBehaviour
    {
        [SerializeField] private LeaderBoard leaderBoard;
        [SerializeField] private TextMeshProUGUI pointsText;
        [SerializeField] private Transform pointsAward;
        [SerializeField] private TextMeshProUGUI pointAwardText;
        private int _points;

        [SerializeField] private float timeToScaleUp;
        [SerializeField] private float timeToFadeOut;
        // private static readonly int LevelComplete = Animator.StringToHash("LevelComplete");
        public void ResetPoints()
        {
            _points = 0;
            pointsText.text = "0";
        }

        private void Start()
        {
            pointAwardText.alpha = 0f;
        }

        public void IncrementPoints(int squareCount)
        {
            var randomBonus = Random.Range(0, 10);
            var points = (randomBonus + squareCount) * 10;
            pointAwardText.text = $"+{points} points";
            _points += points;
            pointsText.text = _points.ToString();
            PlayPopUp();
        }

        private void PlayPopUp()
        {
            pointAwardText.color = new Color(pointAwardText.color.r, pointAwardText.color.g, pointAwardText.color.b, 1f);
            // Scale up
            LeanTween.scale(pointsAward.gameObject, Vector3.one * 3f, timeToScaleUp).setEase(LeanTweenType.easeInQuint)
                .setOnComplete(() =>
                {
                    // Scale down a little
                    LeanTween.scale(pointsAward.gameObject, Vector3.one * 2.2f, timeToScaleUp).setEase(LeanTweenType.easeOutQuint)
                        .setOnComplete(() =>
                        {
                            // Fade out
                            LeanTween.value(gameObject, 1f, 0f, timeToFadeOut).setEase(LeanTweenType.easeInQuad)
                                .setOnUpdate(alpha =>
                                {
                                    pointAwardText.color = new Color(pointAwardText.color.r, pointAwardText.color.g,
                                        pointAwardText.color.b, alpha);
                                })
                                .setOnComplete(() =>
                                {
                                    // reset scale to Vector3.one
                                    pointsAward.localScale = Vector3.one;
                                });
                        });
                });
        }

        public void SendScoreToLeaderboard()
        {
            leaderBoard.SubmitScore(_points);
        }
    }
}
