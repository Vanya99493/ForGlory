using System;
using System.Collections.Generic;
using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using LevelModule.Data;
using UIModule;
using UnityEngine;

namespace Infrastructure.GameStateMachineModule
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IGameState> _states;
        private IGameState _currentGameState;

        public GameStateMachine(UIController uiController, CameraFollower mainCamera, ICoroutineRunner coroutineRunner, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _states = new Dictionary<Type, IGameState>
            {
                { typeof(MainMenuState), new MainMenuState(uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider) },
                { typeof(GameState), new GameState(uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider) }
            };
        }

        public void Enter<TState>(LevelData levelData) where TState : IGameState
        {
            _currentGameState?.Exit();
            _currentGameState = _states[typeof(TState)];
            _currentGameState.Enter(levelData);
        }
    }
}