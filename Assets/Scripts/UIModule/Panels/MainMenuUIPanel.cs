using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class MainMenuUIPanel : BaseUIPanel
    {
        public event Action StartGameAction;
        public event Action ToLoadLevelMenuAction;
        public event Action EndGameAction;

        [SerializeField] private Button startButton;
        [SerializeField] private Button toLoadLevelMenuButton;
        [SerializeField] private Button endButton;
        
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
            startButton.onClick.AddListener(StartGame);
            toLoadLevelMenuButton.onClick.AddListener(ToLoadLevelMenu);
            endButton.onClick.AddListener(EndGame);
        }

        protected override void UnsubscribeActions()
        {
            startButton.onClick.RemoveListener(StartGame);
            toLoadLevelMenuButton.onClick.RemoveListener(ToLoadLevelMenu);
            endButton.onClick.RemoveListener(EndGame);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
        }

        private void StartGame()
        {
            StartGameAction?.Invoke();
        }

        private void ToLoadLevelMenu()
        {
            ToLoadLevelMenuAction?.Invoke();
        }
        
        private void EndGame()
        {
            EndGameAction?.Invoke();
        }
    }
}