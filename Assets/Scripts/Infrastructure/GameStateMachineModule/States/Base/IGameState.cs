using System;
using LevelModule.Data;

namespace Infrastructure.GameStateMachineModule.States.Base
{
    public interface IGameState
    {
        public event Action StateEnded;
        public void Enter(LevelData levelData);
        public void Exit();
    }
}