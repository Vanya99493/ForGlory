using System;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class BattleHudPanel : BasePanel
    {
        public event Action AvoidAction;
        public event Action WinAction;

        [SerializeField] private Button avoidButton;
        [SerializeField] private Button winButton;
        
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