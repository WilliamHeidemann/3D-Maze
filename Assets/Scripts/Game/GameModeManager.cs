using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameModeManager : MonoBehaviour
    {
        public GameMode gameMode;
        [SerializeField] private List<GameObject> timeTrialModeObjects;
        [SerializeField] private List<GameObject> campaignModeObjects;
        [SerializeField] private List<GameObject> levelSelectObjects;
        
        private void Start()
        {
            switch (gameMode)
            {
                case GameMode.TimeTrial:
                    EnterTimeTrialMode();
                    break;
                case GameMode.Campaign:
                    EnterLevelSelect();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisableAll()
        {
            timeTrialModeObjects.ForEach(o => o.SetActive(false));
            campaignModeObjects.ForEach(o => o.SetActive(false));
            levelSelectObjects.ForEach(o => o.SetActive(false));
        }

        public void EnterTimeTrialMode()
        {
            DisableAll();
            timeTrialModeObjects.ForEach(o => o.SetActive(true));
            gameMode = GameMode.TimeTrial;
        }

        public void EnterLevelSelect()
        {
            DisableAll();
            levelSelectObjects.ForEach(o => o.SetActive(true));
            gameMode = GameMode.Campaign;
        }
    
    }
}
