using System;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class GameHudBasePanel : BasePanel
    {
        public event Action ExitToMainMenuAction;
        public event Action NextStepAction;

        [SerializeField] private Button exitToMainMenuButton;
        [SerializeField] private Button nextStepButton;
        
        protected override void SubscribeActions()
        {
            exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
            nextStepButton.onClick.AddListener(NextStep);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(ExitToMainMenu);
            nextStepButton.onClick.RemoveListener(NextStep);
        }

        private void ExitToMainMenu()
        {
            ExitToMainMenuAction?.Invoke();
        }

        private void NextStep()
        {
            NextStepAction?.Invoke();
        }
    }
}