using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UIModule.Panels.Base;

namespace UIModule.Panels
{
    public class PauseMenuPanel : BasePanel
    {
        protected override void SubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOnPause();
        }

        protected override void UnsubscribeActions()
        {
            ServiceLocator.Instance.GetService<PauseController>().TurnOffPause();
        }
    }
}