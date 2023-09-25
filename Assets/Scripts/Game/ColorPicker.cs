using System;
using UnityEngine;

namespace Game
{
    public class ColorPicker : MonoBehaviour
    {
        public static Color color;
        public static Color complementaryColor;
        [SerializeField] private Material colorMaterial;
        [SerializeField] private Material complementaryMaterial;

        private void Update()
        {
            var time = Time.time * 0.05f;
            var r = Mathf.PerlinNoise(time, time);
            var g = Mathf.Sin(time);
            var b = Mathf.Cos(time);
            color = new Color(r, g, b);
            colorMaterial.color = color;
            complementaryColor = new Color(1f - r, 1f - g, 1f - b);
            complementaryMaterial.color = complementaryColor;
        }
    }
}