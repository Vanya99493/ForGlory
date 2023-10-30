using System;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart;
using CharacterModule.ViewPart;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameGameState : IGameState
    {
        public event Action StateEnded;

        private readonly CellPrefabsProvider _cellPrefabsProvider;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;

        private PlaygroundPresenter _playgroundPresenter;
        private CharacterPresenter _playerPresenter;
        
        public GameGameState(CellPrefabsProvider cellPrefabsProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _cellPrefabsProvider = cellPrefabsProvider;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
        }
        
        public void Enter()
        {
            CreatePlayground();
            CreatePlayer();
        }

        public void Exit()
        {
            _playgroundPresenter.Destroy();
            _playerPresenter.Destroy();
        }

        private void CreatePlayground()
        {
            PlaygroundView view = new PlaygroundFactory().InstantiatePlayground();
            PlaygroundModel model = new PlaygroundModel(view);
            _playgroundPresenter = new PlaygroundPresenter(model, _cellPrefabsProvider);
            view.Initialize(_playgroundPresenter);

            int height = 6;
            int width = 6;
            float playgroundSizeHeight = height * 1.0f;
            float playgroundSizeWidth = width * 1.0f;
            
            _playgroundPresenter.CreateAndSpawnPlayground(view.transform, playgroundSizeHeight, playgroundSizeWidth);
        }

        private void CreatePlayer()
        {
            Vector3 playerSpawnPos = new Vector3(-1.5f, 0, 0.5f);
            
            CharacterView view = new CharacterFactory().InstantiateCharacter(
                _gameScenePrefabsProvider.GetCharacterByName("Player"), 
                playerSpawnPos,
                Quaternion.identity
                );
            CharacterModel model = new CharacterModel();
            _playerPresenter = new CharacterPresenter(model);
            view.Inititalize(_playerPresenter);
           
            // ***
            _playerPresenter.Enter<int>();
            // ***
        }
    }
}