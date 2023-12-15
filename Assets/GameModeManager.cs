using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] private GameMode gameMode;
    [SerializeField] private List<GameObject> survivalModeObjects;
    [SerializeField] private List<GameObject> freePlayModeObjects;

    private enum GameMode
    {
        Survival, 
        FreePlay,
    }

    private void Start()
    {
        switch (gameMode)
        {
            case GameMode.Survival:
                EnterSurvivalMode();
                break;
            case GameMode.FreePlay:
                EnterFreePlayMode();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DisableAll()
    {
        survivalModeObjects.ForEach(o => o.SetActive(false));
        freePlayModeObjects.ForEach(o => o.SetActive(false));
    }

    public void EnterSurvivalMode()
    {
        DisableAll();
        survivalModeObjects.ForEach(o => o.SetActive(true));
    }

    public void EnterFreePlayMode()
    {
        DisableAll();
        freePlayModeObjects.ForEach(o => o.SetActive(true));
    }
    
}
