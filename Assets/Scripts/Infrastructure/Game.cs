using CameraModule;
using DataBaseModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Data;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule.Data;
using LevelModule.LevelDataBuilderModule;
using UIModule;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;
        
        private readonly GameStateMachine _gameStateMachine;
        
        private CameraFollower _mainCamera;
        private InputHandler _inputHandler;
        private UIController _uiController;
        private DataBaseController _dbController;
        private LevelDataBuilder _levelDataBuilder;
        private GameData _gameData;

        private LevelData _newLevelData;

        public Game(UIController uiController, CameraFollower mainCamera, CoroutineRunner coroutineRunner, 
            InputHandler inputHandler, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider,
            UIPrefabsProvider uiPrefabsProvider, LevelData newLevelData)
        {
            ServiceLocator.Instance.RegisterService(uiController);
            ServiceLocator.Instance.RegisterService(gameScenePrefabsProvider);
            ServiceLocator.Instance.RegisterService(uiPrefabsProvider);
            _uiController = uiController;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
            _mainCamera = mainCamera;
            _inputHandler = inputHandler;
            _newLevelData = newLevelData;
            _gameStateMachine = new GameStateMachine(uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
            _dbController = new DataBaseController();
            _gameData = new GameData(_dbController.GetLastLevelId());
            _levelDataBuilder = new LevelDataBuilder();
            
            InitializeUIActions();
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<MainMenuState>(null);
        }

        private void InitializeUIActions()
        {
            _uiController.mainMenuUIPanel.StartGameAction += StartNewLevel;
            _uiController.mainMenuUIPanel.EndGameAction += Application.Quit;
            _uiController.loadLevelUIPanel.EnterLoadLevelAction += LoadSavesFromDB;
            _uiController.pauseMenuUIPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>(_levelDataBuilder.GetBackgroundLevelData());
            _uiController.loseGameUIPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>(_levelDataBuilder.GetBackgroundLevelData());
            _uiController.winGameUIPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>(_levelDataBuilder.GetBackgroundLevelData());
        }

        private void StartNewLevel()
        {
            LevelData levelData = _levelDataBuilder.BuildLevelData(_newLevelData, _gameScenePrefabsProvider);
            ServiceLocator.Instance.RegisterService(new CharacterIdSetter(0));
            
            _gameStateMachine.Enter<GameState>(levelData);
        }

        private void LoadSavesFromDB()
        {
            _uiController.loadLevelUIPanel.FillSaves(_dbController.GetLevelsId().ToArray(), LoadLevel, DeleteLevel);
        }

        private void LoadLevel(int index)
        {
            //LevelData levelData = new LevelDataBuilder().SetPrefabs(_dbController.GetLevelDataById(index), _gameScenePrefabsProvider);
            //_gameStateMachine.Enter<GameState>(levelData);
            
            Debug.Log($"Load {index} save");
        }

        private void DeleteLevel(int index)
        {
            Debug.Log($"Delete {index} save");
        }
    }
}