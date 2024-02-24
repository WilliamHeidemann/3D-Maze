using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public DirectionCalculator directionCalculator;

    public TextMeshProUGUI logicalFace;
    public TextMeshProUGUI logicalFacing;
    public TextMeshProUGUI rotationFace;
    public TextMeshProUGUI rotationFacing;
    // Update is called once per frame
    void Update()
    {
        if (directionCalculator == null) directionCalculator = FindObjectOfType<PlayerMovement>().directionCalculator;
        else
        {
            logicalFace.text = $"Logical Orientation: {directionCalculator.nearestOrientation.ToString()}";
            logicalFacing.text = $"Logical Direction: {directionCalculator.nearestDirection.ToString()}";
            rotationFace.text = $"Rotation Orientation: {directionCalculator.targetOrientation.ToString()}";
            rotationFacing.text = $"Rotation Direction: {directionCalculator.targetDirection.ToString()}";
        }
    }
}
