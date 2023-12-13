using System;
using System.Collections.Generic;
using UIModule.Panels;
using UIModule.Panels.Base;
using UnityEngine;

namespace UIModule
{
    public class UIController : MonoBehaviour
    {
        public MainMenuBasePanel mainMenuBasePanel;
        public GameHudBasePanel gameHudBasePanel;
        
        private Dictionary<Type, BasePanel> _panels;
        private BasePanel _currentPanel;

        private void Awake()
        {
            _panels = new Dictionary<Type, BasePanel>
            {
                { mainMenuBasePanel.GetType(), mainMenuBasePanel },
                { gameHudBasePanel.GetType(), gameHudBasePanel }
            };
            
            foreach (var panel in _panels)
            {
                panel.Value.gameObject.SetActive(false);
            }
        }

        public void ActivateMainMenu() => ActivatePanel<MainMenuBasePanel>();
        public void ActivateGameHud() => ActivatePanel<GameHudBasePanel>();
        //public void ActivateBattleHud() => ActivatePanel<BattleHudBasePanel>();
        //public void ActivatePause() => ActivatePanel<PauseBasePanel>();
        
        private void ActivatePanel<TPanel>() where TPanel : BasePanel
        {
            _currentPanel?.Exit();
            _currentPanel = _panels[typeof(TPanel)];
            _currentPanel.Enter();
        }
    }
}