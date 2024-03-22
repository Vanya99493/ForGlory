using System;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UIModule.Panels
{
    public class CastleMenuUIPanel : BaseUIPanel
    {
        public event Action ExitCastleAction;

        [SerializeField] private Button exitCastleButton;
        
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
            exitCastleButton.onClick.AddListener(ExitCastle);
        }

        protected override void UnsubscribeActions()
        {
            exitCastleButton.onClick.RemoveListener(ExitCastle);
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
        }

        private void ExitCastle()
        {
            ExitCastleAction?.Invoke();
        }
    }
}