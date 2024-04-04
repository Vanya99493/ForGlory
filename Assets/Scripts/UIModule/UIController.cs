﻿using System;
using System.Collections.Generic;
using Infrastructure.ServiceLocatorModule;
using UIModule.Panels;
using UIModule.Panels.Base;
using UIModule.Panels.BattleHudModule;
using UIModule.Panels.CastleMenuModule;
using UIModule.Panels.EndGameModule;
using UIModule.Panels.GameHudModule;
using UIModule.Panels.LoadLevelMenuModule;
using UIModule.Panels.MainMenuModule;
using UIModule.Panels.PauseMenuModule;
using UnityEngine;

namespace UIModule
{
    public class UIController : MonoBehaviour, IService
    {
        [Header("UI panels")]
        public MainMenuUIPanel mainMenuUIPanel;
        public GameHudUIPanel gameHudUIPanel;
        public BattleHudUIPanel battleHudUIPanel;
        public LoseGameUIPanel loseGameUIPanel;
        public WinGameUIPanel winGameUIPanel;
        public CastleMenuUIPanel castleMenuUIPanel;
        public LoadLevelUIPanel loadLevelUIPanel;
        [Header(("Pause menu"))]
        public PauseMenuUIPanel pauseMenuUIPanel;
        [Header("Confirm window")]
        [SerializeField] private ConfirmWindow confirmWindow;
        
        private Dictionary<Type, BaseUIPanel> _panels;
        private BaseUIPanel _currentUIPanel;
        private Type _lastActivePanelType;

        private void Awake()
        {
            _panels = new Dictionary<Type, BaseUIPanel>
            {
                { mainMenuUIPanel.GetType(), mainMenuUIPanel },
                { gameHudUIPanel.GetType(), gameHudUIPanel },
                { battleHudUIPanel.GetType(), battleHudUIPanel },
                { loseGameUIPanel.GetType(), loseGameUIPanel },
                { winGameUIPanel.GetType(), winGameUIPanel },
                { castleMenuUIPanel.GetType(), castleMenuUIPanel },
                { loadLevelUIPanel.GetType(), loadLevelUIPanel }
            };
            
            foreach (var panel in _panels)
            {
                panel.Value.gameObject.SetActive(false);
            }
            pauseMenuUIPanel.gameObject.SetActive(false);
            confirmWindow.gameObject.SetActive(false);
        }

        public void ActivateMainMenu() => ActivatePanel<MainMenuUIPanel>();
        public void ActivateGameHud() => ActivatePanel<GameHudUIPanel>();
        public void ActivateBattleHud() => ActivatePanel<BattleHudUIPanel>();
        public void ActivateLoseGameMenu() => ActivatePanel<LoseGameUIPanel>();
        public void ActivateWinGameMenu() => ActivatePanel<WinGameUIPanel>();
        public void ActivateCastleMenuUIPanel() => ActivatePanel<CastleMenuUIPanel>();
        public void ActivateLoadLevelMenu() => ActivatePanel<LoadLevelUIPanel>();
        public void ActivateLastActivePanel() => ActivatePanel(_lastActivePanelType);

        public void ActivatePauseMenu()
        {
            _lastActivePanelType = _currentUIPanel.GetType();
            pauseMenuUIPanel.Enter();
        }

        public void ActivateConfirmWindow(string text, Action confirmAction) =>
            confirmWindow.SubscribeAndActivate(text, confirmAction);
        
        private void ActivatePanel<TPanel>() where TPanel : BaseUIPanel
        {
            _currentUIPanel?.Exit();
            _currentUIPanel = _panels[typeof(TPanel)];
            _currentUIPanel.Enter();
        }

        private void ActivatePanel(Type panel)
        {
            _currentUIPanel?.Exit();
            _currentUIPanel = _panels[panel];
            _currentUIPanel.Enter();
        }
    }
}