using System;
using System.Collections.Generic;
using Infrastructure.ServiceLocatorModule;
using UIModule.Panels;
using UIModule.Panels.Base;
using UIModule.Panels.BattleHudModule;
using UIModule.Panels.CastleMenuModule;
using UIModule.Panels.GameHudModule;
using UnityEngine;

namespace UIModule
{
    public class UIController : MonoBehaviour, IService
    {
        public MainMenuUIPanel mainMenuUIPanel;
        public GameHudUIPanel gameHudUIPanel;
        public BattleHudUIPanel battleHudUIPanel;
        public GameOverMenuUIPanel gameOverMenuUIPanel;
        public CastleMenuUIPanel castleMenuUIPanel;
        
        private Dictionary<Type, BaseUIPanel> _panels;
        private BaseUIPanel _currentUIPanel;

        private void Awake()
        {
            _panels = new Dictionary<Type, BaseUIPanel>
            {
                { mainMenuUIPanel.GetType(), mainMenuUIPanel },
                { gameHudUIPanel.GetType(), gameHudUIPanel },
                { battleHudUIPanel.GetType(), battleHudUIPanel },
                { gameOverMenuUIPanel.GetType(), gameOverMenuUIPanel },
                { castleMenuUIPanel.GetType(), castleMenuUIPanel }
            };
            
            foreach (var panel in _panels)
            {
                panel.Value.gameObject.SetActive(false);
            }
        }

        public void ActivateMainMenu() => ActivatePanel<MainMenuUIPanel>();
        public void ActivateGameHud() => ActivatePanel<GameHudUIPanel>();
        public void ActivateBattleHud() => ActivatePanel<BattleHudUIPanel>();
        public void ActivateGameOverMenu() => ActivatePanel<GameOverMenuUIPanel>();
        public void ActivateCastleMenuUIPanel() => ActivatePanel<CastleMenuUIPanel>();
        //public void ActivatePause() => ActivatePanel<PauseMenuPanel>();
        
        private void ActivatePanel<TPanel>() where TPanel : BaseUIPanel
        {
            _currentUIPanel?.Exit();
            _currentUIPanel = _panels[typeof(TPanel)];
            _currentUIPanel.Enter();
        }
    }
}