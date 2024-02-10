using System;
using System.Collections.Generic;
using Infrastructure.ServiceLocatorModule;
using UIModule.Panels;
using UIModule.Panels.Base;
using UnityEngine;

namespace UIModule
{
    public class UIController : MonoBehaviour, IService
    {
        public MainMenuPanel mainMenuPanel;
        public GameHudPanel gameHudPanel;
        public BattleHudPanel battleHudPanel;
        public GameOverMenuPanel gameOverMenuPanel;
        public CastleMenuPanel castleMenuPanel;
        
        private Dictionary<Type, BasePanel> _panels;
        private BasePanel _currentPanel;

        private void Awake()
        {
            _panels = new Dictionary<Type, BasePanel>
            {
                { mainMenuPanel.GetType(), mainMenuPanel },
                { gameHudPanel.GetType(), gameHudPanel },
                { battleHudPanel.GetType(), battleHudPanel },
                { gameOverMenuPanel.GetType(), gameOverMenuPanel },
                { castleMenuPanel.GetType(), castleMenuPanel }
            };
            
            foreach (var panel in _panels)
            {
                panel.Value.gameObject.SetActive(false);
            }
        }

        public void ActivateMainMenu() => ActivatePanel<MainMenuPanel>();
        public void ActivateGameHud() => ActivatePanel<GameHudPanel>();
        public void ActivateBattleHud() => ActivatePanel<BattleHudPanel>();
        public void ActivateGameOverMenu() => ActivatePanel<GameOverMenuPanel>();
        public void ActivateCastleMenuPanel() => ActivatePanel<CastleMenuPanel>();
        //public void ActivatePause() => ActivatePanel<PauseMenuPanel>();
        
        private void ActivatePanel<TPanel>() where TPanel : BasePanel
        {
            _currentPanel?.Exit();
            _currentPanel = _panels[typeof(TPanel)];
            _currentPanel.Enter();
        }
    }
}