using System;
using CameraModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using Infrastructure.SaveModule;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule;
using LevelModule.Data;
using UIModule;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameState : IGameState
    {
        public event SaveDelegate SaveAction;
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
            
            _currentLevel = new Level(coroutineRunner, cellDataProvider, gameScenePrefabsProvider, uiController);
            
            SubscribeOnUIActions();
            SubscribeStepChangingActions();
            SubscribeLevelBattleActions();
        }
        
        public void Enter(LevelData levelData)
        {
            _currentLevel.StartLevel(levelData);
            _currentLevel.SaveAction += SaveAction;
            _currentLevel.SetCameraTarget(_mainCamera);
            _uiController.ActivateCastleMenuUIPanel();
        }

        public void Exit()
        {
            _currentLevel.SaveAction -= SaveAction;
            _currentLevel.ResetLevel();
            _mainCamera.ResetTarget();
            _currentLevel.RemoveLevel();
            _uiController.gameHudUIPanel.ResetNextStepButton();
        }

        private void SubscribeOnUIActions()
        {
            _uiController.mainMenuUIPanel.ToLoadLevelMenuAction += _uiController.ActivateLoadLevelMenu;
            _uiController.loadLevelUIPanel.GoBackAction += OnGoBackFromLoadLevelMenu;
            _uiController.gameHudUIPanel.OpenPauseMenuAction += () =>
            {
                _uiController.ActivatePauseMenu();
                _uiController.pauseMenuUIPanel.ActivateGamePauseButtons();
            }; 
            _uiController.gameHudUIPanel.NextStepAction += _currentLevel.NextStep;
            _uiController.gameHudUIPanel.EnterAction += _uiController.ActivateCastleMenuUIPanel;
            _uiController.pauseMenuUIPanel.LoadLevelAction += _uiController.ActivateLoadLevelMenu;
            _uiController.castleMenuUIPanel.ExitCastleAction += _uiController.ActivateGameHud;
            _uiController.battleHudUIPanel.PauseBattleAction += () => {
                _uiController.ActivatePauseMenu();
                _uiController.pauseMenuUIPanel.ActivateBattlePauseButtons();
                _uiController.pauseMenuUIPanel.HideSaveButton();
            };
        }

        private void OnGoBackFromLoadLevelMenu()
        {
            if(_currentLevel.IsActive)
                _uiController.ActivateLastActivePanel();
            else
                _uiController.ActivateMainMenu();
        }

        private void SubscribeStepChangingActions()
        {
            _currentLevel.StartStepChangingAction += _uiController.gameHudUIPanel.BlockNextStepButton;
            _currentLevel.EndStepChangingAction += _uiController.gameHudUIPanel.UnblockNextStepButton;
        }

        private void SubscribeLevelBattleActions()
        {
            _currentLevel.BattleStartAction += OnBattleStart;
            _currentLevel.BattleEndAction += OnBattleEnd;
            _currentLevel.LoseGameAction += OnGameLose;
            _currentLevel.WinGameAction += OnGameWin;
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

        private void OnGameWin()
        {
            _uiController.ActivateWinGameMenu();
        }

        private void OnGameLose()
        {
            _uiController.ActivateLoseGameMenu();
        }
    }
}