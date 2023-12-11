using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timePassedText;

    [SerializeField] private int levelCompleteTimeBonus;
    private float _timeLeft;
    

    void Start()
    {
        _timeLeft = 120;
    }

    void Update()
    {
        _timeLeft -= Time.deltaTime;
        timePassedText.text = "Time " + FormatTime(_timeLeft);
    }

    public void IncrementTimer()
    {
        _timeLeft += levelCompleteTimeBonus;
    }

    private static string FormatTime(float time)
    {
        if (time < 0)
        {
            return "00:00";
        }

        var minutes = (int)(time / 60);
        var seconds = (int)(time % 60);

        return $"{minutes:D2}:{seconds:D2}";
    }

}
