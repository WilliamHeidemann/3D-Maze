using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;

public class Background : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    void Update()
    {
        _camera.backgroundColor = ColorPicker.color;
    }
}
