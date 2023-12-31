﻿using CameraModule;
using DataBaseModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.InputHandlerModule;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
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

        private LevelData _newLevelData;

        public Game(UIController uiController, CameraFollower mainCamera, CoroutineRunner coroutineRunner, 
            InputHandler inputHandler, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider,
            LevelData newLevelData)
        {
            ServiceLocator.Instance.RegisterService(uiController);
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
            _mainCamera = mainCamera;
            _inputHandler = inputHandler;
            _newLevelData = newLevelData;
            _gameStateMachine = new GameStateMachine(uiController, mainCamera, coroutineRunner, cellDataProvider, gameScenePrefabsProvider);
            
            InitializeUIActions(uiController);
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<MainMenuState>(null);
        }

        private void InitializeUIActions(UIController uiController)
        {
            uiController.mainMenuPanel.StartGameAction += StartNewLevel;
            uiController.mainMenuPanel.EndGameAction += Application.Quit;
            uiController.gameHudPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>(null);
            uiController.gameOverMenuPanel.ExitToMainMenuAction += () => _gameStateMachine.Enter<MainMenuState>(null);
        }

        private void StartNewLevel()
        {
            LevelData levelData = new LevelDataBuilder().BuildLevelData(_newLevelData, _gameScenePrefabsProvider);
            
            _gameStateMachine.Enter<GameState>(levelData);
        }

        private void LoadLevel(int index)
        {
            LevelData levelData = new LevelDataBuilder().SetPrefabs(new DataBaseCover().GetLevelDataById(0), _gameScenePrefabsProvider);

            _gameStateMachine.Enter<GameState>(levelData);
        }
    }
}