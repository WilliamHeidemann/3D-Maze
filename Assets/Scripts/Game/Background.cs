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
    [SerializeField] private Material mazeMaterial;

    private void Start()
    {
        _camera = GetComponent<Camera>();
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
        mazeMaterial.color = Complementary(r, g, b);
    }

    private static Color Complementary(float r, float g, float b) => new Color(1f - r, 1f - g, 1f - b);
}
