using System;
using Infrastructure.GameStateMachineModule.States.Base;
using UIModule;

namespace Infrastructure.GameStateMachineModule.States
{
    public class MainMenuState : IGameState
    {
        public event Action StateEnded;

        private UIController _uiController;
        
        public MainMenuState(UIController uiController)
        {
            _uiController = uiController;
        }
        
        public void Enter()
        {
            _uiController.ActivateMainMenu();
        }

        public void Exit()
        {
            
        }
    }
}