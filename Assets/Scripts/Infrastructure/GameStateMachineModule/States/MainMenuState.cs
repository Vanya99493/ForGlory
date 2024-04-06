using System;
using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using LevelModule;
using LevelModule.Data;
using UIModule;

namespace Infrastructure.GameStateMachineModule.States
{
    public class MainMenuState : IGameState
    {
        public event Action StateEnded;

        private UIController _uiController;
        private CameraFollower _mainCamera;

        private Level _backgroundLevel;
        
        public MainMenuState(UIController uiController,CameraFollower mainCamera, ICoroutineRunner coroutineRunner,
            CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _uiController = uiController;
            _mainCamera = mainCamera;
            _backgroundLevel = new Level(coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
        }
        
        public void Enter(LevelData levelData)
        {
            _backgroundLevel.StartBackgroundLevel(levelData);
            _uiController.ActivateMainMenu();
            _mainCamera.ActivateRotation();
        }

        public void Exit()
        {
            _backgroundLevel.RemoveBackgroundLevel();
            _mainCamera.DeactivateRotation();
        }
    }
}