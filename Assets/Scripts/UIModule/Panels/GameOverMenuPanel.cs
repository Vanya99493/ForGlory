using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class GameOverMenuPanel : BasePanel
    {
        public event Action ExitToMainMenuAction;

        [SerializeField] private Button exitToMainMenuButton;
        
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
            exitToMainMenuButton.onClick.AddListener(OnExitToMainMenu);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(OnExitToMainMenu);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
        }

        private void OnExitToMainMenu()
        {
            ExitToMainMenuAction?.Invoke();
        }
    }
}