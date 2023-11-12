using System;
using System.Collections.Generic;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart;
using CharacterModule.ViewPart;
using CustomClasses;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
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
            CreatePlayer(2, 2);
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
            
            _playgroundPresenter.CreateAndSpawnPlayground(view.transform, playgroundSizeHeight, playgroundSizeWidth, OnCellClicked);
        }

        private void CreatePlayer(int heightSpawnCellIndex, int widthSpawnCellIndex)
        {
            CharacterView view = new CharacterFactory()
                .InstantiateCharacter(_gameScenePrefabsProvider.GetCharacterByName("Player"));
            CharacterModel model = new CharacterModel(view, heightSpawnCellIndex, widthSpawnCellIndex);
            _playerPresenter = new CharacterPresenter(model);
            view.Inititalize(_playerPresenter);
            _playgroundPresenter.SetCharacterOnCell(_playerPresenter, heightSpawnCellIndex, widthSpawnCellIndex);
           
            // ***
            _playerPresenter.Enter<int>();
            // ***
        }

        private void OnCellClicked(int heightIndex, int widthIndex)
        {
            new Vector3(
                -2.5f + 1f * widthIndex,
                0.5f,
                2.5f - 1f * heightIndex
                );
            List<Pair<int, int>> route;
            WideSearch bfsSearch = new WideSearch();
            if (_playerPresenter.Model.CanMove && bfsSearch.TryBuildRoute(
                    new Node(
                        _playerPresenter.Model.HeightCellIndex, 
                        _playerPresenter.Model.WidthCellIndex, 
                        _playgroundPresenter.Model.GetCellPresenter(_playerPresenter.Model.HeightCellIndex, _playerPresenter.Model.WidthCellIndex).Model.CellType
                        ),
                    new Node(heightIndex, widthIndex,
                    _playgroundPresenter.Model.GetCellPresenter(heightIndex, widthIndex).Model.CellType),
                    _playgroundPresenter,
                    out route
                ));
            {
                _playgroundPresenter.RemoveCharacterFromCell(_playerPresenter.Model.HeightCellIndex, _playerPresenter.Model.WidthCellIndex);
                _playerPresenter.Move();
            }
        }
    }
}