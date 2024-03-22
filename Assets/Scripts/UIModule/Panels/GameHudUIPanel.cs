using System;
using TMPro;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class GameHudUIPanel : BaseUIPanel
    {
        public event Action ExitToMainMenuAction;
        public event Action NextStepAction;
        public event Action EnterAction;

        [SerializeField] private Button exitToMainMenuButton;
        [SerializeField] private Button nextStepButton;
        [SerializeField] private Button enterButton;

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

        public void ShowEnterButton()
        {
            enterButton.gameObject.SetActive(true);
        }

        public void HideEnterButton()
        {
            enterButton.gameObject.SetActive(false);
        }
        
        protected override void SubscribeActions()
        {
            exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
            nextStepButton.onClick.AddListener(NextStep);
            enterButton.onClick.AddListener(Enter);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(ExitToMainMenu);
            nextStepButton.onClick.RemoveListener(NextStep);
            enterButton.onClick.RemoveListener(Enter);
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

        private void Enter()
        {
            EnterAction?.Invoke();
        }
    }
}