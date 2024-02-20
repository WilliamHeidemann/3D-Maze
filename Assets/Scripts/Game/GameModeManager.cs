using System;
using System.Collections.Generic;
using Game.SurvivalMode;
using UnityEngine;

namespace Game
{
    public class GameModeManager : MonoBehaviour
    {
        public GameMode gameMode;
        [SerializeField] private List<GameObject> timeTrialModeObjects;
        [SerializeField] private List<GameObject> campaignModeObjects;
        [SerializeField] private List<GameObject> levelSelectObjects;
        [SerializeField] private SurvivalModeStarter survivalModeStarter;
        [SerializeField] private Timer timer;
        [SerializeField] private PauseController pauseController;
        private bool isShowingGUI = true;

        private void Start()
        {
            switch (gameMode)
            {
                case GameMode.TimeTrial:
                    EnterTimeTrialMode();
                    break;
                case GameMode.Campaign:
                    EnterCampaignMode();
                    break;
                case GameMode.LevelSelect:
                    EnterLevelSelect();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H)) ToggleGUI();
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
            survivalModeStarter.FirstMaze();
            pauseController.enabled = true;
            gameMode = GameMode.TimeTrial;
        }

        public void EnterLevelSelect()
        {
            DisableAll();
            levelSelectObjects.ForEach(o => o.SetActive(true));
            timer.StartTimer(false);
            pauseController.enabled = false;
            gameMode = GameMode.LevelSelect;
        }

        public void EnterCampaignMode()
        {
            DisableAll();
            campaignModeObjects.ForEach(o => o.SetActive(true));
            timer.StartTimer(false);
            pauseController.enabled = false;
            gameMode = GameMode.Campaign;
        }
        
        public void ToggleGUI()
        {
            if (gameMode == GameMode.LevelSelect) return;
            isShowingGUI = !isShowingGUI;
            if (!isShowingGUI) DisableAll();
            else
            {
                if (gameMode == GameMode.Campaign) campaignModeObjects.ForEach(o => o.SetActive(true));
                else if (gameMode == GameMode.TimeTrial) timeTrialModeObjects.ForEach(o => o.SetActive(true));
            }
        }
    
    }
}
