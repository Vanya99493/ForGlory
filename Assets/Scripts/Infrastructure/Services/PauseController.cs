using Infrastructure.ServiceLocatorModule;
using UnityEngine;

namespace Infrastructure.Services
{
    public class PauseController : IService
    {
        public bool IsPause { get; private set; }

        public PauseController()
        {
            IsPause = false;
        }

        public void TurnOnPause()
        {
            IsPause = true;
            Time.timeScale = 0;
        }

        public void TurnOffPause()
        {
            IsPause = false;
            Time.timeScale = 1;
        }

        public void ResetPause() => TurnOffPause();
    }
}