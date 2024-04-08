using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.EndGameModule
{
    public class LoseGameUIPanel : BaseUIPanel
    {
        public event Action ExitToMainMenuAction;

        [SerializeField] private Button exitToMainMenuButton;
        
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnGamePause();
            exitToMainMenuButton.onClick.AddListener(OnExitToMainMenu);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(OnExitToMainMenu);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffGamePause();
        }

        private void OnExitToMainMenu()
        {
            ExitToMainMenuAction?.Invoke();
        }
    }
}