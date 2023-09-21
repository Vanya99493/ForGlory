using Infrastructure.CoroutineRunnerModule;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrap : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private CoroutineRunner coroutineRunner;
        
        private Game _game;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            DontDestroyOnLoad(coroutineRunner);
            
            _game = new Game(coroutineRunner);
            _game.StartGame();
        }
    }
}