using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI pointAwardText;
    [SerializeField] private Animator animator;
    private int _points;
    private static readonly int LevelComplete = Animator.StringToHash("LevelComplete");

    public void ResetPoints()
    {
        _points = 0;
        pointsText.text = "0";
    }

    public void IncrementPoints(int squareCount)
    {
        var randomBonus = Random.Range(0, 10);
        var points = (randomBonus + squareCount) * 10;
        pointAwardText.text = $"+{points} points";
        _points += points;
        pointsText.text = _points.ToString();
        animator.SetTrigger(LevelComplete);
    }
}
