using Infrastructure.ServiceLocatorModule;
using UnityEngine;

namespace Infrastructure.Services
{
    public class PauseController : IService
    {
        public bool IsGamePause { get; private set; }
        public bool IsInputPause { get; private set; }

        public PauseController()
        {
            IsGamePause = false;
            IsInputPause = false;
        }

        public void TurnOnGamePause()
        {
            IsGamePause = true;
            Time.timeScale = 0;
        }

        public void TurnOffGamePause()
        {
            IsGamePause = false;
            Time.timeScale = 1;
        }

        public void TurnOnInputPause()
        {
            IsInputPause = true;
        }

        public void TurnOffInputPause()
        {
            IsInputPause = false;
        }

        public void ResetGamePause() => TurnOffGamePause();
    }
}