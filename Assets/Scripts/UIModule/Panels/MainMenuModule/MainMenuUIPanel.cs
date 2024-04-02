using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule.Data;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.MainMenuModule
{
    public class MainMenuUIPanel : BaseUIPanel
    {
        public event Action ToLoadLevelMenuAction;
        public event Action EndGameAction;
        public event Action<LevelDifficulty> SelectDifficultyAction;

        [SerializeField] private Button startButton;
        [SerializeField] private Button toLoadLevelMenuButton;
        [SerializeField] private Button endButton;
        [SerializeField] private SelectDifficultyPanel selectDifficultyPanel;

        private void Start()
        {
            selectDifficultyPanel.Initialize();
            selectDifficultyPanel.SelectDifficultyAction += OnSelectDifficulty;
        }

        protected override void SubscribeActions()
        {
            selectDifficultyPanel.gameObject.SetActive(false);
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
            startButton.onClick.AddListener(OnStartGame);
            toLoadLevelMenuButton.onClick.AddListener(OnToLoadLevelMenu);
            endButton.onClick.AddListener(OnEndGame);
        }

        protected override void UnsubscribeActions()
        {
            startButton.onClick.RemoveListener(OnStartGame);
            toLoadLevelMenuButton.onClick.RemoveListener(OnToLoadLevelMenu);
            endButton.onClick.RemoveListener(OnEndGame);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
        }

        private void OnStartGame()
        {
            selectDifficultyPanel.gameObject.SetActive(true);
        }

        private void OnSelectDifficulty(LevelDifficulty levelDifficulty)
        {
            SelectDifficultyAction?.Invoke(levelDifficulty);
        }

        private void OnToLoadLevelMenu()
        {
            ToLoadLevelMenuAction?.Invoke();
        }
        
        private void OnEndGame()
        {
            EndGameAction?.Invoke();
        }
    }
}