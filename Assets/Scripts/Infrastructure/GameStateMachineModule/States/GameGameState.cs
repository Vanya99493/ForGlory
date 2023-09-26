using System;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.ViewPart;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameGameState : IGameState
    {
        public event Action StateEnded;

        private readonly CellPixelsPrefabsProvider _cellPixelsPrefabsProvider;
        private readonly CellDataProvider _cellDataProvider;
        
        public GameGameState(CellPixelsPrefabsProvider cellPixelsPrefabsProvider, CellDataProvider cellDataProvider)
        {
            _cellPixelsPrefabsProvider = cellPixelsPrefabsProvider;
            _cellDataProvider = cellDataProvider;
        }
        
        public void Enter()
        {
            PlaygroundFactory playgroundFactory = new PlaygroundFactory();
            PlaygroundView playgroundView = playgroundFactory.InstantiatePlayground();
            PlaygroundModel model = new PlaygroundModel(playgroundView);
            playgroundView.Initialize(new PlaygroundPresenter(model, _cellPixelsPrefabsProvider));
            
            playgroundView.SpawnPlayground();
        }

        public void Exit()
        {
            
        }
    }
}