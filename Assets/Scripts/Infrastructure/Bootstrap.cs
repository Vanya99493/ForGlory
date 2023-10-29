using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CoroutineRunner coroutineRunner;
        [FormerlySerializedAs("cellPixelsPrefabsProvider")] [SerializeField] private CellPrefabsProvider cellPrefabsProvider;
        
        private Game _game;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(coroutineRunner);
            
            _game = new Game(coroutineRunner, cellPrefabsProvider);
            _game.StartGame();
        }
    }
}