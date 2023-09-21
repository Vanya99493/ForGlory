using System;

namespace Infrastructure.GameStateMachineModule.States.Base
{
    public interface IGameState
    {
        public event Action StateEnded;
        public void Enter();
        public void Exit();
    }
}