using System;
using System.Collections.Generic;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.GameStateMachineModule.States.Base;

namespace Infrastructure.GameStateMachineModule
{
    public class GameStateMachine
    {
        public event Action BootstrapStateEnd;
        
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentGameState;

        public GameStateMachine(CoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IGameState>()
            {
                { typeof(BootstrapGameState), new BootstrapGameState(coroutineRunner) },
                { typeof(MainMenuGameState), new MainMenuGameState() },
                { typeof(GameGameState), new GameGameState() }
            };

            SubscribeStateEndActions();
        }

        public void Enter<TState>() where TState : IGameState
        {
            _currentGameState?.Exit();
            _currentGameState = _states[typeof(TState)];
            _currentGameState.Enter();
        }

        private void SubscribeStateEndActions()
        {
            _states[typeof(BootstrapGameState)].StateEnded += BootstrapStateEnd;
        }
    }
}