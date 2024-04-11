using System.Collections.Generic;
using CameraModule;
using CastleModule.PresenterPart;
using CharacterModule.PresenterPart;
using DataBaseModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule.Data;
using LevelModule.LevelDataBuilderModule;
using PlaygroundModule.PresenterPart;
using UIModule;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;
        
        private readonly GameStateMachine _gameStateMachine;
        private readonly CellDataProvider _cellDataProvider;
        
        private CameraFollower _mainCamera;
        private InputHandler _inputHandler;
        private UIController _uiController;
        private DataBaseController _dbController;
        private LevelDataBuilder _levelDataBuilder;
        private LevelDataProvider _levelDataProvider;

        public Game(UIController uiController, CameraFollower mainCamera, CoroutineRunner coroutineRunner, 
            InputHandler inputHandler, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider,
            UIPrefabsProvider uiPrefabsProvider, LevelDataProvider levelDataProvider, string dbName)
        {
            ServiceLocator.Instance.RegisterService(uiController);
            ServiceLocator.Instance.RegisterService(gameScenePrefabsProvider);
            ServiceLocator.Instance.RegisterService(uiPrefabsProvider);
            _uiController = uiController;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
            _cellDataProvider = cellDataProvider;
            _mainCamera = mainCamera;
            _inputHandler = inputHandler;
            _levelDataProvider = levelDataProvider;
            _gameStateMachine = new GameStateMachine(uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider, OnSaveGame);
            _dbController = new DataBaseController(dbName);
            _levelDataBuilder = new LevelDataBuilder();
            
            InitializeUIActions();
        }

        public void StartGame()
        {
            StartBackgroundLevel();
        }

        private void InitializeUIActions()
        {
            _uiController.mainMenuUIPanel.SelectDifficultyAction += StartNewLevel;
            _uiController.mainMenuUIPanel.EndGameAction += () =>
            {
                _uiController.ActivateConfirmWindow("Are you sure?", () =>
                {
                    Application.Quit();
                });
            };
            _uiController.loadLevelUIPanel.EnterLoadLevelAction += LoadSavesFromDB;
            _uiController.pauseMenuUIPanel.ExitToMainMenuAction += () => 
            {
                _uiController.ActivateConfirmWindow("Are you sure?", () =>
                {
                    _uiController.battleHudUIPanel.UnsubscribeInfoPanel();
                    _uiController.pauseMenuUIPanel.Exit();
                    StartBackgroundLevel();
                });
            };
            _uiController.loseGameUIPanel.ExitToMainMenuAction += StartBackgroundLevel;
            _uiController.winGameUIPanel.ExitToMainMenuAction += StartBackgroundLevel;
        }

        private void LoadSavesFromDB()
        {
            _uiController.loadLevelUIPanel.FillSaves(
                _dbController.GetLevels(), 
                levelIndex =>
                {
                    _uiController.ActivateConfirmWindow("Are you sure?", () => LoadLevel(levelIndex));
                },
                levelIndex =>
                {
                    _uiController.ActivateConfirmWindow("Are you sure?", () => DeleteLevel(levelIndex));
                }
                );
        }

        private void StartBackgroundLevel()
        {
            CharacterIdSetter characterIdSetter = new CharacterIdSetter(0);
            LevelData levelData = _levelDataBuilder.BuildNewLevelData(_levelDataProvider.GetBackgroundLevelData(), 
                _gameScenePrefabsProvider, _cellDataProvider, characterIdSetter, _dbController);
            
            _gameStateMachine.Enter<MainMenuState>(levelData);
        }

        private void StartNewLevel(LevelDifficulty levelDifficulty)
        {
            CharacterIdSetter characterIdSetter = new CharacterIdSetter(0);
            LevelData levelData = _levelDataBuilder.BuildNewLevelData(_levelDataProvider.GetLevelDifficultyData(levelDifficulty), 
                _gameScenePrefabsProvider, _cellDataProvider, characterIdSetter, _dbController);
            
            _gameStateMachine.Enter<GameState>(levelData);
        }

        private void OnSaveGame(PlaygroundPresenter playgroundPresenter, CastlePresenter castlePresenter, 
            PlayerTeamPresenter playerTeamPresenter, List<EnemyTeamPresenter> enemyTeamPresenters, CharacterIdSetter characterIdSetter)
        {
            LevelData levelData = _levelDataBuilder.BuildDBData(playgroundPresenter, castlePresenter, playerTeamPresenter,
                enemyTeamPresenters, characterIdSetter);
            _dbController.SaveLevel(levelData);
        }

        private void LoadLevel(int index)
        {
            LevelData levelData = _dbController.GetLevelDataById(index);
            _gameStateMachine.Enter<GameState>(levelData);
        }

        private void DeleteLevel(int index)
        {
            _dbController.DeleteLevel(index);
            LoadSavesFromDB();
        }
    }
}