using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using Maze;
using UnityEngine;

public class MazeRotator : MonoBehaviour
{
    private Quaternion target;
    private Quaternion tempRotation;

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 2f);
    }

    public void SnapToFace(Orientation face)
    {
        target = face switch
        {
            Orientation.Front => Quaternion.Euler(0,0,0),
            Orientation.Back => Quaternion.Euler(180, 0, 180),
            Orientation.Right => Quaternion.Euler(0, 90, 0),
            Orientation.Left => Quaternion.Euler(0, -90, 0),
            Orientation.Up => Quaternion.Euler(-90, 0, 0),
            Orientation.Down => Quaternion.Euler(90, 0, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(face), face, null)
        };
        transform.rotation = target;
    }

    public void Rotate(CardinalDirection direction)
    {
        var axis = GetRotationAxis(direction);
        var angle = GetRotationAngle(direction);
        var cubeTransform = transform;
        var currentRotation = cubeTransform.rotation;
        cubeTransform.rotation = target;
        transform.Rotate(axis, angle, Space.World);
        var next = cubeTransform.rotation;
        target = next;
        cubeTransform.rotation = currentRotation;
    }

    private static float GetRotationAngle(CardinalDirection direction) => direction switch
    {
        CardinalDirection.North => -90f,
        CardinalDirection.South => 90,
        CardinalDirection.East => 90,
        CardinalDirection.West => -90f,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    private static Vector3 GetRotationAxis(CardinalDirection direction) => direction switch
    {
        CardinalDirection.North => Vector3.right,
        CardinalDirection.South => Vector3.right,
        CardinalDirection.East => Vector3.up,
        CardinalDirection.West => Vector3.up,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public void RotateToTarget()
    {
        tempRotation = transform.rotation;
        transform.rotation = target;
    }

    public void ReturnToTempRotation()
    {
        transform.rotation = tempRotation;
    }
}