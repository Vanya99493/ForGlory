using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.PauseMenuModule
{
    public class PauseMenuUIPanel : BaseUIPanel
    {
        public event Action SaveLevelAction;
        public event Action LoadLevelAction;
        public event Action ExitToMainMenuAction;

        [SerializeField] private Button closeGamePauseButton;
        [SerializeField] private Button closeBattlePauseButton;
        [SerializeField] private Button saveLevelButton;
        [SerializeField] private Button loadLevelButton;
        [SerializeField] private Button exitToMainMenuButton;
        
        private void Start()
        {
            closeGamePauseButton.onClick.AddListener(Exit);
            closeBattlePauseButton.onClick.AddListener(Exit);
            saveLevelButton.onClick.AddListener(OnSaveLevel);
            loadLevelButton.onClick.AddListener(OnLoadLevel);
            exitToMainMenuButton.onClick.AddListener(OnExitToMainMenu);
        }

        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
        }

        protected override void UnsubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
            ShowSaveButton();
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