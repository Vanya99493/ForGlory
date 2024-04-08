using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule.Data;
using UIModule.Panels.Base;
using UIModule.Panels.LoadingScreenModule;
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
        [SerializeField] private BlackBackgroundPanel blackBackgroundPanel;

        private void Start()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnInputPause();
            selectDifficultyPanel.Initialize();
            selectDifficultyPanel.SelectDifficultyAction += OnSelectDifficulty;
        }

        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOffInputPause();
            selectDifficultyPanel.gameObject.SetActive(false);
            startButton.onClick.AddListener(OnStartGame);
            toLoadLevelMenuButton.onClick.AddListener(OnToLoadLevelMenu);
            endButton.onClick.AddListener(OnEndGame);
            blackBackgroundPanel.StartDisappear();
        }

        protected override void UnsubscribeActions()
        {
            startButton.onClick.RemoveListener(OnStartGame);
            toLoadLevelMenuButton.onClick.RemoveListener(OnToLoadLevelMenu);
            endButton.onClick.RemoveListener(OnEndGame);
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