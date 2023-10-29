using System;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using PlaygroundModule.PresenterPart;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameGameState : IGameState
    {
        public event Action StateEnded;

        private readonly CellPrefabsProvider _cellPrefabsProvider;
        
        public GameGameState(CellPrefabsProvider cellPrefabsProvider)
        {
            _cellPrefabsProvider = cellPrefabsProvider;
        }
        
        public void Enter()
        {
            CreatePlayground();
        }

        public void Exit()
        {
            
        }

        private void CreatePlayground()
        {
            PlaygroundPresenter playgroundPresenter = new PlaygroundPresenter(_cellPrefabsProvider);

            int height = 6;
            int width = 6;
            float playgroundSizeHeight = height * 1.0f;
            float playgroundSizeWidth = width * 1.0f;
            
            playgroundPresenter.CreateAndSpawnPlayground(height, width, playgroundSizeHeight, playgroundSizeWidth);
        }
    }
}