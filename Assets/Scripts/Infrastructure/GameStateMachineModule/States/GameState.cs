using System;
using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using LevelModule;
using UIModule;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameState : IGameState
    {
        public event Action StateEnded;

        private readonly UIController _uiController;
        private readonly CameraFollower _mainCamera;

        private Level _currentLevel;
        
        public GameState(UIController uiController, CameraFollower mainCamera, ICoroutineRunner coroutineRunner, 
            CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _uiController = uiController;
            _mainCamera = mainCamera;
            
            _currentLevel = new Level(coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
            
            SubscribeOnUIActions();
            SubscribeStepChangingActions();
        }
        
        public void Enter()
        {
            _currentLevel.StartLevel(3);
            _currentLevel.SetCameraTarget(_mainCamera);
            _uiController.ActivateGameHud();
        }

        public void Exit()
        {
            _currentLevel.ResetLevel();
            _mainCamera.ResetTarget();
            _currentLevel.RemoveLevel();
            _uiController.gameHudBasePanel.ResetNextStepButton();
        }

        private void SubscribeOnUIActions()
        {
            _uiController.gameHudBasePanel.NextStepAction += _currentLevel.NextStep;
        }

        private void SubscribeStepChangingActions()
        {
            _currentLevel.StartStepChangingAction += _uiController.gameHudBasePanel.BlockNextStepButton;
            _currentLevel.EndStepChangingAction += _uiController.gameHudBasePanel.UnblockNextStepButton;
        }
    }
}