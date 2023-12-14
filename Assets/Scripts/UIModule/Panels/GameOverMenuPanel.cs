using System;
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
            exitToMainMenuButton.onClick.AddListener(OnExitToMainMenu);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(OnExitToMainMenu);
        }

        private void OnExitToMainMenu()
        {
            ExitToMainMenuAction?.Invoke();
        }
    }
}