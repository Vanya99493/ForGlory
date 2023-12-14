using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using UIModule;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;
        
        private CameraFollower _mainCamera;
        private InputHandler _inputHandler;

        public Game(UIController uiController, CameraFollower mainCamera, CoroutineRunner coroutineRunner, InputHandler inputHandler, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            ServiceLocator.Instance.RegisterService(uiController);
            _mainCamera = mainCamera;
            _inputHandler = inputHandler;
            _gameStateMachine = new GameStateMachine(uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
            
            InitializeUIActions(uiController);
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void InitializeUIActions(UIController uiController)
        {
            uiController.mainMenuPanel.StartGameAction += () => _gameStateMachine.Enter<GameState>();
            uiController.mainMenuPanel.EndGameAction += Application.Quit;
            uiController.gameHudPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>();
            uiController.gameOverMenuPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>();
        }
    }
}