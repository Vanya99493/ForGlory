using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class PauseMenuUIPanel : BaseUIPanel
    {
        public event Action CloseGamePauseMenuAction;
        public event Action CloseBattlePauseMenuAction;
        public event Action SaveLevelAction;
        public event Action LoadLevelAction;
        public event Action ExitToMainMenuAction;

        [SerializeField] private Button closeGamePauseButton;
        [SerializeField] private Button closeBattlePauseButton;
        [SerializeField] private Button saveLevelButton;
        [SerializeField] private Button loadLevelButton;
        [SerializeField] private Button exitToMainMenuButton;
        
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
            closeGamePauseButton.onClick.AddListener(OnReturnToGame);
            closeBattlePauseButton.onClick.AddListener(OnReturnToBattle);
            saveLevelButton.onClick.AddListener(OnSaveLevel);
            loadLevelButton.onClick.AddListener(OnLoadLevel);
            exitToMainMenuButton.onClick.AddListener(OnExitToMainMenu);
        }

        protected override void UnsubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
            ShowSaveButton();
            closeGamePauseButton.onClick.RemoveListener(OnReturnToGame);
            closeBattlePauseButton.onClick.RemoveListener(OnReturnToBattle);
            saveLevelButton.onClick.RemoveListener(OnSaveLevel);
            loadLevelButton.onClick.RemoveListener(OnLoadLevel);
            exitToMainMenuButton.onClick.RemoveListener(OnExitToMainMenu);
        }

        public void ActivateGamePauseButtons()
        {
            closeGamePauseButton.gameObject.SetActive(true);
            closeBattlePauseButton.gameObject.SetActive(false);
        }

        public void ActivateBattlePauseButtons()
        {
            closeGamePauseButton.gameObject.SetActive(false);
            closeBattlePauseButton.gameObject.SetActive(true);
        }

        public void HideSaveButton()
        {
            saveLevelButton.gameObject.SetActive(false);
        }

        private void ShowSaveButton()
        {
            saveLevelButton.gameObject.SetActive(true);
        }

        private void OnReturnToGame()
        {
            CloseGamePauseMenuAction?.Invoke();
        }

        private void OnReturnToBattle()
        {
            CloseBattlePauseMenuAction?.Invoke();
        }

        private void OnSaveLevel()
        {
            SaveLevelAction?.Invoke();
        }

        private void OnLoadLevel()
        {
            LoadLevelAction?.Invoke();
        }

        private void OnExitToMainMenu()
        {
            ExitToMainMenuAction?.Invoke();
        }
    }
}