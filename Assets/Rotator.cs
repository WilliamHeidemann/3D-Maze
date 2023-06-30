using System;
using System.Collections;
using System.Collections.Generic;
using Maze;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private Quaternion target;
    private void Start()
    {
        target = Quaternion.Euler(transform.rotation.eulerAngles);
    }

    private void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target, 5f);
        if (Input.GetKeyDown(KeyCode.W)) Rotate(KeyCode.W);
        if (Input.GetKeyDown(KeyCode.S)) Rotate(KeyCode.S);
        if (Input.GetKeyDown(KeyCode.D)) Rotate(KeyCode.D);
        if (Input.GetKeyDown(KeyCode.A)) Rotate(KeyCode.A);
        if (Input.GetKeyDown(KeyCode.R))
        {
            target = Quaternion.identity;
            transform.rotation = target;
        }
    }
    
    public void Rotate(KeyCode key)
    {
        transform.rotation = target;
        var axis = key switch
        {
            KeyCode.W => Vector3.right,
            KeyCode.S => Vector3.right,
            KeyCode.D => Vector3.up,
            KeyCode.A => Vector3.up,
        };
        var angle = key switch
        {
            KeyCode.W => -90f,
            KeyCode.S => 90,
            KeyCode.D => 90,
            KeyCode.A => -90,
        };
        transform.Rotate(axis, angle, Space.World);
        var next = transform.rotation;
        transform.Rotate(axis, -angle, Space.World);
        target = next;
    }
}
