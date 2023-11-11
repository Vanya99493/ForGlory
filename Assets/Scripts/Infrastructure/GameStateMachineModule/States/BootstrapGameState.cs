using System;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using Object = UnityEngine.Object;

namespace Infrastructure.GameStateMachineModule.States
{
    public class BootstrapGameState : IGameState
    {
        public event Action StateEnded;
        
        private const string GameSceneName = "GameScene";

        private readonly CoroutineRunner _coroutineRunner;
        private readonly HandlersProvider _handlersProvider;

        public BootstrapGameState(CoroutineRunner coroutineRunner, HandlersProvider handlersProvider)
        {
            _coroutineRunner = coroutineRunner;
            _handlersProvider = handlersProvider;
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
            InstantiateInputHandler();
            
            StateEnded?.Invoke();
        }

        private void InstantiateInputHandler()
        {
            Object.Instantiate(_handlersProvider.GetInputHandler());
        }
    }
}