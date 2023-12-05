using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CoroutineRunner coroutineRunner;
        [SerializeField] private HandlersProvider handlersProvider;
        [SerializeField] private GameScenePrefabsProvider gameScenePrefabsProvider;
        [FormerlySerializedAs("cellPrefabsProvider")] [SerializeField] private CellDataProvider cellDataProvider;
        
        private Game _game;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(coroutineRunner);
            
            _game = new Game(coroutineRunner, handlersProvider, cellDataProvider, gameScenePrefabsProvider);
            _game.StartGame();
        }
    }
}