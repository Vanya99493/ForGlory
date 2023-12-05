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

        public GameStateMachine(ICoroutineRunner coroutineRunner, HandlersProvider handlersProvider, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _states = new Dictionary<Type, IGameState>()
            {
                { typeof(BootstrapGameState), new BootstrapGameState(coroutineRunner, handlersProvider) },
                { typeof(MainMenuGameState), new MainMenuGameState() },
                { typeof(GameGameState), new GameGameState(coroutineRunner, cellDataProvider, gameScenePrefabsProvider) }
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