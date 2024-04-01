using System;
using TMPro;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.GameHudModule
{
    public class GameHudUIPanel : BaseUIPanel
    {
        public event Action OpenPauseMenuAction;
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
            exitToMainMenuButton.onClick.AddListener(OnopenPauseMenu);
            nextStepButton.Button.onClick.AddListener(OnNextStep);
            enterButton.onClick.AddListener(OnEnter);
        }

        protected override void UnsubscribeActions()
        {
            exitToMainMenuButton.onClick.RemoveListener(OnopenPauseMenu);
            nextStepButton.Button.onClick.RemoveListener(OnNextStep);
            enterButton.onClick.RemoveListener(OnEnter);
        }

        private void OnopenPauseMenu()
        {
            OpenPauseMenuAction?.Invoke();
        }

        private void OnNextStep()
        {
            if (_isBlockedNextStepButton)
                return;
            NextStepAction?.Invoke();
        }

        private void OnEnter()
        {
            EnterAction?.Invoke();
        }
    }
}