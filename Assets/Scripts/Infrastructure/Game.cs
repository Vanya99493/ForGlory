using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.Providers;
using UnityEngine;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;

        public Game(CoroutineRunner coroutineRunner, CellPixelsPrefabsProvider cellPixelsPrefabsProvider, CellDataProvider cellDataProvider)
        {
            _gameStateMachine = new GameStateMachine(coroutineRunner, cellPixelsPrefabsProvider, cellDataProvider);
            _gameStateMachine.SubscribeBootstrapStateEndAction(EnterMainMenu);
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<BootstrapGameState>();
        }

        private void EnterMainMenu()
        {
            //_gameStateMachine.Enter<MainMenuGameState>();
            _gameStateMachine.Enter<GameGameState>();
        }
    }
}