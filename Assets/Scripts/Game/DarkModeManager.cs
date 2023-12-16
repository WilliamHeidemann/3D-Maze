using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkModeManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Material squareMaterial;
    [SerializeField] private Material wallMaterial;
    [SerializeField] private Material playerGoalMaterial;

    [Header("Dark Mode Colors")] 
    [SerializeField] private Color backgroundDark;
    [SerializeField] private Color squareDark;
    [SerializeField] private Color wallDark;
    [SerializeField] private Color playerGoalDark;
    
    [Header("Bright Mode Colors")] 
    [SerializeField] private Color backgroundBright;
    [SerializeField] private Color squareBright;
    [SerializeField] private Color wallBright;
    [SerializeField] private Color playerGoalBright;

    private bool _isDark = false;
    

    private void Start()
    {
        SetDark(_isDark);
    }

    public void SetDark(bool dark)
    {
        mainCamera.backgroundColor = dark ? backgroundDark : backgroundBright;
        squareMaterial.color = dark ? squareDark : squareBright;
        wallMaterial.color = dark ? wallDark : wallBright;
        playerGoalMaterial.color = dark ? playerGoalDark : playerGoalBright;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            _isDark = !_isDark;
            SetDark(_isDark);
        }
    }
}
