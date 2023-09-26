using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private CellPixelsPrefabsProvider cellPixelsPrefabsProvider;
        [SerializeField] private CellDataProvider cellDataProvider;
        
        private Game _game;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(coroutineRunner);
            
            _game = new Game(coroutineRunner, cellPixelsPrefabsProvider, cellDataProvider);
            _game.StartGame();
        }
    }
}