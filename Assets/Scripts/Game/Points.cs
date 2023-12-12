using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pointsText;
    private int _points;

    public void ResetPoints()
    {
        _points = 0;
        pointsText.text = "0";
    }

    public void IncrementPoints(int squareCount)
    {
        var product = squareCount * 10;
        _points += product;
        pointsText.text = _points.ToString();
    }
}
