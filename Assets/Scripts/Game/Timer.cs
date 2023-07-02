using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private TextMeshProUGUI timePassedText;
    public bool levelComplete;
    private float timePassed;
    public TextMeshProUGUI bestTimeText;

    // Start is called before the first frame update
    void Start()
    {
        timePassedText = GetComponent<TextMeshProUGUI>();
        var bestTime = PlayerPrefs.GetFloat("BestTime");
        bestTimeText.text = "Best time: " + FormatTime(bestTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!levelComplete)
        {
            timePassed += Time.deltaTime;
            timePassedText.text = "Time: " + FormatTime(timePassed);
        }
    }

    public void RecordAttempt()
    {
        var previousBest = PlayerPrefs.GetFloat("BestTime");
        var best = Mathf.Min(previousBest, timePassed);
        PlayerPrefs.SetFloat("BestTime", best);
    }
    
    public string FormatTime(float time)
    {
        int totalMilliseconds = (int)(time * 1000);
        int seconds = totalMilliseconds / 1000;
        int milliseconds = totalMilliseconds % 1000;

        string formattedSeconds = seconds.ToString().PadLeft(2, '0');
        string formattedMilliseconds = milliseconds.ToString().PadLeft(3, '0').Substring(0, 2);

        return $"{formattedSeconds}:{formattedMilliseconds}";
    }

}
