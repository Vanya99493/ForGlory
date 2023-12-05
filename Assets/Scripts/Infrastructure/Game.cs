using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;
using Infrastructure.Providers;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;

        public Game(CoroutineRunner coroutineRunner, HandlersProvider handlersProvider, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _gameStateMachine = new GameStateMachine(coroutineRunner, handlersProvider, cellDataProvider, gameScenePrefabsProvider);
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