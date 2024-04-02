using System;
using CharacterModule.PresenterPart;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class BattleHudUIPanel : BaseUIPanel
    {
        public event Action PauseBattleAction;
        public event Action AvoidAction;
        public event Action WinAction;

        [SerializeField] private Button pauseButton;
        [SerializeField] private Button avoidButton;
        [SerializeField] private Button winButton;
        [SerializeField] private InfoPanel infoPanel;
        
        protected override void SubscribeActions()
        {
            pauseButton.onClick.AddListener(OnPauseBattle);
            avoidButton.onClick.AddListener(OnAvoidBattle);
            winButton.onClick.AddListener(OnWinBattle);
            
            //infoPanel.UpdateBars();
        }

        protected override void UnsubscribeActions()
        {
            pauseButton.onClick.RemoveListener(OnPauseBattle);
            avoidButton.onClick.RemoveListener(OnAvoidBattle);
            winButton.onClick.RemoveListener(OnWinBattle);
        }

        public void SubscribeInfoPanel(PlayerTeamPresenter playerTeam, EnemyTeamPresenter enemyTeam)
        {
            infoPanel.SubscribeBars(playerTeam, enemyTeam);
        }

        public void UnsubscribeInfoPanel()
        {
            infoPanel.UnsubscribeBars();
        }

        private void OnPauseBattle()
        {
            PauseBattleAction?.Invoke();
        }

        private void OnAvoidBattle()
        {
            AvoidAction?.Invoke();
        }

        private void OnWinBattle()
        {
            WinAction?.Invoke();
        }
    }
}