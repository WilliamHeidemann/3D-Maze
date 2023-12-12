using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timePassedText;

    [SerializeField] private int timeBonus;
    [SerializeField] private int maxTimeInSeconds;
    private float _timeLeft;
    public static bool timeIsRunning;

    void Start()
    {
        _timeLeft = maxTimeInSeconds;
        timeIsRunning = true;
    }

    void Update()
    {
        if (timeIsRunning)
        {
            _timeLeft -= Time.deltaTime;
            timePassedText.text = FormatTime(_timeLeft);
            if (_timeLeft < 0)
            {
                timeIsRunning = false;
            }
        }
    }

    public void IncrementTimer()
    {
        _timeLeft += timeBonus;
        _timeLeft = Mathf.Min(_timeLeft, maxTimeInSeconds);
    }

    public void ResetTimer()
    {
        _timeLeft = maxTimeInSeconds;
        timePassedText.text = FormatTime(_timeLeft);
        timeIsRunning = true;
    }

    private static string FormatTime(float time)
    {
        if (time < 0) return "00:00";
        var minutes = (int)(time / 60);
        var seconds = (int)(time % 60);
        return $"Time {minutes:D2}:{seconds:D2}";
    }

}
