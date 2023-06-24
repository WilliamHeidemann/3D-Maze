using System;
using System.Collections;
using System.Collections.Generic;
using Maze;
using UnityEngine;

public class MazeRotator : MonoBehaviour
{
    private Quaternion target;

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 1f);
    }

    public void Rotate(Orientation face)
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
    }
}
