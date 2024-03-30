using System;
using TMPro;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.GameHudModule
{
    public class GameHudUIPanel : BaseUIPanel
    {
        public event Action ExitToMainMenuAction;
        public event Action NextStepAction;
        public event Action EnterAction;

        [SerializeField] private Button exitToMainMenuButton;
        [SerializeField] private Button enterButton;
        [SerializeField] private NextStepButton nextStepButton;

        private bool _isBlockedNextStepButton = false;

        private void Awake()
        {
            nextStepButton.ShowNextStepImage();
        }

        public void ResetNextStepButton()
        {
            nextStepButton.ShowNextStepImage();
            _isBlockedNextStepButton = false;
        }
        
        public void BlockNextStepButton()
        {
            nextStepButton.ShowWaitStepImage();
            _isBlockedNextStepButton = true;
        }

        public void UnblockNextStepButton()
        {
            nextStepButton.ShowNextStepImage();
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
            nextStepButton.Button.onClick.AddListener(NextStep);
            enterButton.onClick.AddListener(Enter);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(ExitToMainMenu);
            nextStepButton.Button.onClick.RemoveListener(NextStep);
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