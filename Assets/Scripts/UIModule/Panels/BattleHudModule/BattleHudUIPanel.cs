using System;
using CharacterModule.PresenterPart;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels.BattleHudModule
{
    public class BattleHudUIPanel : BaseUIPanel
    {
        public event Action AvoidAction;
        public event Action WinAction;

        [SerializeField] private Button avoidButton;
        [SerializeField] private Button winButton;
        [SerializeField] private InfoPanel infoPanel;
        
        protected override void SubscribeActions()
        {
            avoidButton.onClick.AddListener(AvoidBattle);
            winButton.onClick.AddListener(WinBattle);
        }

        protected override void UnsubscribeActions()
        {
            avoidButton.onClick.RemoveListener(AvoidBattle);
            winButton.onClick.RemoveListener(WinBattle);
        }

        public void SubscribeInfoPanel(PlayerTeamPresenter playerTeam, EnemyTeamPresenter enemyTeam)
        {
            infoPanel.SubscribeBars(playerTeam, enemyTeam);
        }

        private void AvoidBattle()
        {
            AvoidAction?.Invoke();
        }

        private void WinBattle()
        {
            WinAction?.Invoke();
        }
    }
}