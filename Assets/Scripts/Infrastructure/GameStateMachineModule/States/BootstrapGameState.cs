using System;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;

namespace Infrastructure.GameStateMachineModule.States
{
    public class BootstrapGameState : IGameState
    {
        public event Action StateEnded;
        
        private const string GameSceneName = "GameScene";

        private readonly CoroutineRunner _coroutineRunner;

        public BootstrapGameState(CoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void Enter()
        {
            SceneLoader sceneLoader = new SceneLoader();
            sceneLoader.LoadScene(GameSceneName, _coroutineRunner, OnSceneLoaded);
        }

        public void Exit()
        {
            
        }

        private void OnSceneLoaded()
        {
            StateEnded?.Invoke();
        }
    }
}