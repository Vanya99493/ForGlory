using System;
using System.Collections.Generic;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;

namespace Infrastructure.GameStateMachineModule
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentGameState;

        public GameStateMachine(CoroutineRunner coroutineRunner, CellPrefabsProvider cellPrefabsProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _states = new Dictionary<Type, IGameState>()
            {
                { typeof(BootstrapGameState), new BootstrapGameState(coroutineRunner) },
                { typeof(MainMenuGameState), new MainMenuGameState() },
                { typeof(GameGameState), new GameGameState(cellPrefabsProvider, gameScenePrefabsProvider) }
            };
        }

        public void Enter<TState>() where TState : IGameState
        {
            _currentGameState?.Exit();
            _currentGameState = _states[typeof(TState)];
            _currentGameState.Enter();
        }

        public void SubscribeBootstrapStateEndAction(Action OnBootstrapStateEnd)
        {
            _states[typeof(BootstrapGameState)].StateEnded += OnBootstrapStateEnd;
        }
    }
}