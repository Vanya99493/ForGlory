using UnityEngine;

namespace UIModule.Panels.Base
{
    public abstract class BasePanel : MonoBehaviour
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