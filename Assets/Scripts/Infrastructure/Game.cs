using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule;
using Infrastructure.GameStateMachineModule.States;

namespace Infrastructure
{
    public class Game
    {
        private readonly GameStateMachine _gameStateMachine;

        public Game(CoroutineRunner coroutineRunner)
        {
            _gameStateMachine = new GameStateMachine(coroutineRunner);
            _gameStateMachine.BootstrapStateEnd += EnterMainMenu;
        }

        public void StartGame()
        {
            _gameStateMachine.Enter<BootstrapGameState>();
        }

        private void EnterMainMenu()
        {
            _gameStateMachine.Enter<MainMenuGameState>();
        }
    }
}