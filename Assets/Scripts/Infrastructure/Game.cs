using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using UIModule;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;
        
        private UIController _uiController;
        private CameraFollower _mainCamera;
        private InputHandler _inputHandler;

        public Game(UIController uiController, CameraFollower mainCamera, CoroutineRunner coroutineRunner, InputHandler inputHandler, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _uiController = uiController;
            _mainCamera = mainCamera;
            _inputHandler = inputHandler;
            _gameStateMachine = new GameStateMachine(_uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
            
            InitializeUIActions();
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void InitializeUIActions()
        {
            _uiController.mainMenuBasePanel.StartGameAction += () => _gameStateMachine.Enter<GameState>();
            _uiController.mainMenuBasePanel.EndGameAction += Application.Quit;
            _uiController.gameHudBasePanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>();
        }
    }
}