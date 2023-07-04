using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Background : MonoBehaviour
{
    private Camera _camera;
    private Color color = Color.white;
    private float colorOffset;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        Random.InitState(DateTime.Now.Millisecond);
        colorOffset = Random.value;
    }

    void Update()
    {
        var time = Time.time * 0.05f + colorOffset;
        var r = Mathf.PerlinNoise(time, time);
        var g = Mathf.Sin(time);
        var b = Mathf.Cos(time);
        color = new Color(r, g, b);
        _camera.backgroundColor = color;
    }
}
