using System;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class MainMenuPanel : BasePanel
    {
        public event Action StartGameAction;
        public event Action EndGameAction;

        [SerializeField] private Button startButton;
        [SerializeField] private Button endButton;

        protected override void SubscribeActions()
        {
            startButton.onClick.AddListener(StartGame);
            endButton.onClick.AddListener(EndGame);
        }

        protected override void UnsubscribeActions()
        {
            startButton.onClick.RemoveListener(StartGame);
            endButton.onClick.RemoveListener(EndGame);
        }

        private void StartGame()
        {
            StartGameAction?.Invoke();
        }

        private void EndGame()
        {
            EndGameAction?.Invoke();
        }
    }
}