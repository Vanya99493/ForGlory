using System;
using TMPro;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class GameHudPanel : BasePanel
    {
        public event Action ExitToMainMenuAction;
        public event Action NextStepAction;

        [SerializeField] private Button exitToMainMenuButton;
        [SerializeField] private Button nextStepButton;

        private bool _isBlockedNextStepButton = false;

        public void ResetNextStepButton()
        {
            nextStepButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT";
            _isBlockedNextStepButton = false;
        }
        
        public void BlockNextStepButton()
        {
            nextStepButton.GetComponentInChildren<TextMeshProUGUI>().text = "0_0";
            _isBlockedNextStepButton = true;
        }

        public void UnblockNextStepButton()
        {
            nextStepButton.GetComponentInChildren<TextMeshProUGUI>().text = "NEXT";
            _isBlockedNextStepButton = false;
        }
        
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
            if (_isBlockedNextStepButton)
                return;
            NextStepAction?.Invoke();
        }
    }
}