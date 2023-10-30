﻿using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private GameScenePrefabsProvider gameScenePrefabsProvider;
        [SerializeField] private CellPrefabsProvider cellPrefabsProvider;
        
        private Game _game;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(coroutineRunner);
            
            _game = new Game(coroutineRunner, cellPrefabsProvider, gameScenePrefabsProvider);
            _game.StartGame();
        }
    }
}