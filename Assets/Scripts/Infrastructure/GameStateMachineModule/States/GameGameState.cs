using System;
using System.Collections.Generic;
using CameraModule;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart;
using CharacterModule.ViewPart;
using CustomClasses;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.GameStateMachineModule.States.Base;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
using PlaygroundModule.ViewPart;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Infrastructure.GameStateMachineModule.States
{
    public class GameGameState : IGameState
    {
        public event Action StateEnded;

        private readonly ICoroutineRunner _coroutineRunner;
        private readonly CellDataProvider _cellDataProvider;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;

        private PlaygroundPresenter _playgroundPresenter;
        private PlayerTeamPresenter _playerTeamPresenter;
        private EnemyTeamPresenter _enemyTeamPresenter;

        private WideSearch _bfsSearch;
        private List<Node> _activeCells;
        
        public GameGameState(ICoroutineRunner coroutineRunner, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _coroutineRunner = coroutineRunner;
            _cellDataProvider = cellDataProvider;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
            _bfsSearch = new WideSearch(cellDataProvider);
            _activeCells = new List<Node>();
        }
        
        public void Enter()
        {
            CreatePlayground();
            CreatePlayer();
            
            CreateEnemy();
            
            Camera.main.GetComponent<CameraFolower>().SetTarget(_playerTeamPresenter.View.transform);
        }

        public void Exit()
        {
            _playgroundPresenter.Destroy();
            _playerTeamPresenter.Destroy();
        }

        private void CreatePlayground()
        {
            PlaygroundView view = new PlaygroundFactory().InstantiatePlayground();
            PlaygroundModel model = new PlaygroundModel(view);
            _playgroundPresenter = new PlaygroundPresenter(model, _cellDataProvider);
            view.Initialize(_playgroundPresenter);

            int height = 25;
            int width = 25;
            int lengthOfWater = 2;
            int lengthOfCoast = 3;
            float playgroundSizeHeight = height * 1.0f;
            float playgroundSizeWidth = width * 1.0f;
            
            _playgroundPresenter.CreateAndSpawnPlayground(view.transform, height, width, lengthOfWater, lengthOfCoast, playgroundSizeHeight, playgroundSizeWidth, _cellDataProvider, OnCellClicked);
        }

        private void CreatePlayer()
        {
            int heightSpawnCellIndex, widthSpawnCellIndex;
            do
            {
                heightSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.Height);
                widthSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.Width);
            } while (_playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Hill &&
                     _playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Plain);

            PlayerCharacterPresenter[] players = new PlayerCharacterPresenter[]
            {
                new PlayerCharacterPresenter(new PlayerCharacterModel("Player", 100, 5, 10), new PlayerCharacterView())
            };
            
            TeamView view = new TeamFactory()
                .InstantiateCharacter(_gameScenePrefabsProvider.GetCharacterByName("Player"));
            TeamModel model = new PlayerTeamModel(heightSpawnCellIndex, widthSpawnCellIndex, players);
            _playerTeamPresenter = new PlayerTeamPresenter(model, view);
            _playerTeamPresenter.Model.SetPosition(_playgroundPresenter);
            _playerTeamPresenter.ClickOnCharacterAction += OnPlayerTeamClicked;
            _playgroundPresenter.SetCharacterOnCell(_playerTeamPresenter, heightSpawnCellIndex, widthSpawnCellIndex);
           
            // ***
            _playerTeamPresenter.Enter<int>();
            // ***
        }

        private void CreateEnemy()
        {
            int heightSpawnCellIndex, widthSpawnCellIndex;
            do
            {
                heightSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.Height);
                widthSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.Width);
            } while (_playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Hill &&
                     _playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Plain);

            EnemyCharacterPresenter[] enemies = new[]
            {
                new EnemyCharacterPresenter(new EnemyCharacterModel("Enemy", 50, 3, 5), new EnemyCharacterView())
            };
            
            TeamView view = new TeamFactory()
                .InstantiateCharacter(_gameScenePrefabsProvider.GetCharacterByName("Enemy"));
            TeamModel model = new EnemyTeamModel(heightSpawnCellIndex, widthSpawnCellIndex, enemies);
            _enemyTeamPresenter = new EnemyTeamPresenter(model, view);
            _enemyTeamPresenter.Model.SetPosition(_playgroundPresenter);
            _enemyTeamPresenter.ClickOnCharacterAction += OnPlayerTeamClicked;
            _playgroundPresenter.SetCharacterOnCell(_enemyTeamPresenter, heightSpawnCellIndex, widthSpawnCellIndex);
        } 

        private void OnPlayerTeamClicked(bool canMove, int energy)
        {
            _activeCells = _bfsSearch.GetCellsByLength(
                energy, 
                new Node(
                    _playerTeamPresenter.Model.HeightCellIndex, 
                    _playerTeamPresenter.Model.WidthCellIndex, 
                    _playgroundPresenter.Model.GetCellPresenter(_playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex).Model.CellType
                ), 
                _playgroundPresenter
                );
            if (canMove)
                ActivateCells();
            else
                DeactivateCells();
        }

        private void OnEnemyClicked(bool canMove, int energy)
        {
            
        }

        private void ActivateCells()
        {
            foreach (Node cell in _activeCells)
            {
                _playgroundPresenter.Model.GetCellPresenter(cell.HeightIndex, cell.WidthIndex).ActivateCell();
            }
        }

        private void DeactivateCells()
        {
            foreach (Node cell in _activeCells)
            {
                _playgroundPresenter.Model.GetCellPresenter(cell.HeightIndex, cell.WidthIndex).DeactivateCell();
            }
            _activeCells.Clear();
        }

        private void OnCellClicked(int heightIndex, int widthIndex)
        {
            List<Pair<int, int>> route = new List<Pair<int, int>>();
            if (_playerTeamPresenter.Model.MoveState && _bfsSearch.TryBuildRoute(
                    new Node(
                        _playerTeamPresenter.Model.HeightCellIndex, 
                        _playerTeamPresenter.Model.WidthCellIndex, 
                        _playgroundPresenter.Model.GetCellPresenter(_playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex).Model.CellType
                        ),
                    new Node(heightIndex, widthIndex,
                    _playgroundPresenter.Model.GetCellPresenter(heightIndex, widthIndex).Model.CellType),
                    _playgroundPresenter,
                    out route
                ));
            {
                _playgroundPresenter.RemoveCharacterFromCell(_playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex);
                _playerTeamPresenter.AddRoute(route);
                _playerTeamPresenter.Move(_coroutineRunner, _playgroundPresenter);
            }
            DeactivateCells();
        }
    }
}