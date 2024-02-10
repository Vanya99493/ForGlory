using Infrastructure.ServiceLocatorModule;

namespace Infrastructure.Services
{
    public class PauseController : IService
    {
        public bool IsPause { get; private set; }

        public PauseController()
        {
            IsPause = false;
        }

        public void TurnOnPause() => IsPause = true;
        public void TurnOffPause() => IsPause = false;
        public void ResetPause() => TurnOffPause();
    }
}