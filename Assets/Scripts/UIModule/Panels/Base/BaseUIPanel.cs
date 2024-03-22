using UnityEngine;

namespace UIModule.Panels.Base
{
    public abstract class BaseUIPanel : BasePanel
    {
        public void Enter()
        {
            gameObject.SetActive(true);
            SubscribeActions();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            UnsubscribeActions();
        }

        protected abstract void SubscribeActions();
        protected abstract void UnsubscribeActions();
    }
}