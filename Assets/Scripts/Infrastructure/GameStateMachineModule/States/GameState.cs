using System;
using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule;
using LevelModule.Data;
using UIModule;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameState : IGameState
    {
        public event Action StateEnded;

        private readonly UIController _uiController;
        private readonly CameraFollower _mainCamera;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;

        private Level _currentLevel;
        
        public GameState(UIController uiController, CameraFollower mainCamera, ICoroutineRunner coroutineRunner, 
            CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _uiController = uiController;
            _mainCamera = mainCamera;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
            
            ServiceLocator.Instance.RegisterService(new PauseController());
            
            _currentLevel = new Level(coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
            
            SubscribeOnUIActions();
            SubscribeStepChangingActions();
            SubscribeLevelBattleActions();
        }
        
        public void Enter(LevelData levelData)
        {
            _currentLevel.StartLevel(levelData);
            _currentLevel.SetCameraTarget(_mainCamera);
            _uiController.ActivateGameHud();
        }

        public void Exit()
        {
            _currentLevel.ResetLevel();
            _mainCamera.ResetTarget();
            _currentLevel.RemoveLevel();
            _uiController.gameHudPanel.ResetNextStepButton();
        }

        private void SubscribeOnUIActions()
        {
            _uiController.gameHudPanel.NextStepAction += _currentLevel.NextStep;
            _uiController.gameHudPanel.EnterAction += _uiController.ActivateCastleMenuPanel;
            _uiController.castleMenuPanel.ExitCastleAction += _uiController.ActivateGameHud;
        }

        private void SubscribeStepChangingActions()
        {
            _currentLevel.StartStepChangingAction += _uiController.gameHudPanel.BlockNextStepButton;
            _currentLevel.EndStepChangingAction += _uiController.gameHudPanel.UnblockNextStepButton;
        }

        private void SubscribeLevelBattleActions()
        {
            _currentLevel.BattleStartAction += OnBattleStart;
            _currentLevel.BattleEndAction += OnBattleEnd;
            _currentLevel.EndGameAction += OnGameEnd;
        }

        private void OnBattleStart()
        {
            _mainCamera.StayAtZero();
            _uiController.ActivateBattleHud();
        }

        private void OnBattleEnd()
        {
            _mainCamera.FollowTarget();
            _uiController.ActivateGameHud();
        }

        private void OnGameEnd()
        {
            _uiController.ActivateGameOverMenu();
        }
    }
}