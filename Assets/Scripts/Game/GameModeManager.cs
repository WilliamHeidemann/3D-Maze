using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    public class GameModeManager : MonoBehaviour
    {
        [SerializeField] private GameMode gameMode;
        [SerializeField] private List<GameObject> timeTrialModeObjects;
        [SerializeField] private List<GameObject> classicModeObjects;
        [SerializeField] private List<GameObject> levelSelectObjects;

        private enum GameMode
        {
            TimeTrial,
            Classic,
        }

        private void Start()
        {
            switch (gameMode)
            {
                case GameMode.TimeTrial:
                    EnterTimeTrialMode();
                    break;
                case GameMode.Classic:
                    EnterLevelSelect();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisableAll()
        {
            timeTrialModeObjects.ForEach(o => o.SetActive(false));
            classicModeObjects.ForEach(o => o.SetActive(false));
            levelSelectObjects.ForEach(o => o.SetActive(false));
        }

        public void EnterTimeTrialMode()
        {
            DisableAll();
            timeTrialModeObjects.ForEach(o => o.SetActive(true));
        }

        public void EnterLevelSelect()
        {
            DisableAll();
            levelSelectObjects.ForEach(o => o.SetActive(true));
        }
    
    }
}
