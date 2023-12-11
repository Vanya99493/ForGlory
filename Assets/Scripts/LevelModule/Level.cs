using System.Collections.Generic;
using BattleModule.PresenterPart;
using CameraModule;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart;
using CharacterModule.ViewPart;
using CustomClasses;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace LevelModule
{
    public class Level
    {
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly CellDataProvider _cellDataProvider;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;
        
        private readonly WideSearch _bfsSearch;
        
        private PlaygroundPresenter _playgroundPresenter;
        private PlayerTeamPresenter _playerTeamPresenter;
        private List<EnemyTeamPresenter> _enemiesTeamPresenter;
        private BattlegroundPresenter _battlegroundPresenter;
        
        public Level(ICoroutineRunner coroutineRunner, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _coroutineRunner = coroutineRunner;
            _cellDataProvider = cellDataProvider;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;

            _bfsSearch = new WideSearch(cellDataProvider);
        }

        public void StartLevel()
        {
            _enemiesTeamPresenter = new List<EnemyTeamPresenter>();
            
            CreatePlayground();
            CreateBattleground();
            
            CreatePlayer();
            // need to create more enemies
            CreateEnemies(3);
        }

        public void ResetLevel()
        {
            _playgroundPresenter.DeactivateCells();
        }

        public void RemoveLevel()
        {
            _playgroundPresenter.Destroy();
            _playerTeamPresenter.Destroy();
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenter)
            {
                enemyTeamPresenter.Destroy();
            }
        }

        public void SetCameraTarget(CameraFollower camera)
        {
            camera.SetTarget(_playerTeamPresenter.View.transform);
        }

        public void NextStep()
        {
            _playerTeamPresenter.Model.ResetEnergy();
            _playerTeamPresenter.Model.ResetMovementSettings();
            _playgroundPresenter.DeactivateCells();
            
            // need to add enemy behaviour
        }
        
        private void CreatePlayground()
        {
            PlaygroundView view = new PlaygroundFactory().InstantiatePlayground();
            PlaygroundModel model = new PlaygroundModel();
            _playgroundPresenter = new PlaygroundPresenter(model, _cellDataProvider);
            view.Initialize(_playgroundPresenter);

            int height = 10;
            int width = 10;
            int lengthOfWater = 1;
            int lengthOfCoast = 2;
            float playgroundSizeHeight = height * 1.0f;
            float playgroundSizeWidth = width * 1.0f;
            
            _playgroundPresenter.CreateAndSpawnPlayground(view.transform, height, width, lengthOfWater, lengthOfCoast, 
                playgroundSizeHeight, playgroundSizeWidth, _cellDataProvider, 
                OnMoveCellClicked, OnCellClicked, OnTeamsCollision);
        }

        private void CreateBattleground()
        {
            _battlegroundPresenter = new BattlegroundPresenter();
        }

        private void CreatePlayer()
        {
            int heightSpawnCellIndex, widthSpawnCellIndex;
            do
            {
                heightSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.PlaygroundHeight);
                widthSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.PlaygroundWidth);
            } while (_playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Hill &&
                     _playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Plain &&
                     !_playgroundPresenter.CheckCellOnCharacter(heightSpawnCellIndex, widthSpawnCellIndex));

            (TeamView view, CharacterView[] characters) = new TeamFactory().InstantiateTeam(
                _gameScenePrefabsProvider.GetTeamView(), 
                _gameScenePrefabsProvider.GetCharacterByName("Player1"),
                _gameScenePrefabsProvider.GetCharacterByName("Player2")
                );

            PlayerCharacterPresenter[] players = new PlayerCharacterPresenter[characters.Length];
            for (int i = 0; i < players.Length; i++)
            {
                players[i] = new PlayerCharacterPresenter(new PlayerCharacterModel("Player", 100, 5, 10), (PlayerCharacterView)characters[i]);
            }
            
            TeamModel model = new PlayerTeamModel(heightSpawnCellIndex, widthSpawnCellIndex, players);
            _playerTeamPresenter = new PlayerTeamPresenter(model, view);
            _playerTeamPresenter.Model.SetPosition(_playgroundPresenter);
            _playerTeamPresenter.ClickOnCharacterAction += OnPlayerTeamClicked;

            _playgroundPresenter.SetCharacterOnCell(_playerTeamPresenter, heightSpawnCellIndex, widthSpawnCellIndex, true);
           
            // ***
            _playerTeamPresenter.Enter<int>();
            // ***
        }

        private void CreateEnemies(int enemyTeamsCount)
        {
            for (int i = 0; i < enemyTeamsCount; i++)
            {
                while (true)
                {
                    int heightSpawnCellIndex, widthSpawnCellIndex;
                    do
                    {
                        heightSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.PlaygroundHeight);
                        widthSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.PlaygroundWidth);
                    } while (_playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Hill &&
                             _playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Plain &&
                             !_playgroundPresenter.CheckCellOnCharacter(heightSpawnCellIndex, widthSpawnCellIndex));

                    (TeamView view, CharacterView[] characters) = new TeamFactory().InstantiateTeam(
                        _gameScenePrefabsProvider.GetTeamView(), 
                        _gameScenePrefabsProvider.GetCharacterByName("Enemy1"),
                        _gameScenePrefabsProvider.GetCharacterByName("Enemy2"),
                        _gameScenePrefabsProvider.GetCharacterByName("Enemy3")
                    );

                    EnemyCharacterPresenter[] enemies = new EnemyCharacterPresenter[characters.Length];
                    for (int j = 0; j < enemies.Length; j++)
                    {
                        enemies[j] = new EnemyCharacterPresenter(new EnemyCharacterModel("Enemy", 50, 3, 5), (EnemyCharacterView)characters[j]);
                    }
            
                    TeamModel model = new EnemyTeamModel(heightSpawnCellIndex, widthSpawnCellIndex, enemies);
                    EnemyTeamPresenter enemyTeamPresenter = new EnemyTeamPresenter(model, view);
                    enemyTeamPresenter.Model.SetPosition(_playgroundPresenter);
                    enemyTeamPresenter.ClickOnCharacterAction += OnEnemyTeamClicked;
                    enemyTeamPresenter.FollowClickAction += OnEnemyFollowClick;

                    if (_playgroundPresenter.SetCharacterOnCell(enemyTeamPresenter, heightSpawnCellIndex, widthSpawnCellIndex, true))
                    {
                        _enemiesTeamPresenter.Add(enemyTeamPresenter);
                        break;
                    }

                    enemyTeamPresenter.Destroy();
                }
            }
        } 

        private void OnPlayerTeamClicked(TeamPresenter playerTeamPresenter)
        {
            _playgroundPresenter.DeactivateCells();
            
            if (playerTeamPresenter.Model.MoveState)
            {
                _playgroundPresenter.SetAciveCells(_bfsSearch.GetCellsByLength(
                    playerTeamPresenter.Model.Energy, 
                    new Node(
                        playerTeamPresenter.Model.HeightCellIndex, 
                        playerTeamPresenter.Model.WidthCellIndex, 
                        _playgroundPresenter.Model.GetCellPresenter(playerTeamPresenter.Model.HeightCellIndex, playerTeamPresenter.Model.WidthCellIndex).Model.CellType,
                        true
                    ), 
                    _playgroundPresenter,
                    false
                ));
                _playgroundPresenter.ActivateCells();
            }
            else
            {
                _playgroundPresenter.ResetActiveCells();
            }
        }

        private void OnEnemyTeamClicked(TeamPresenter enemyTeamPresenter)
        {
            _playerTeamPresenter.Model.ResetMovementSettings();
            _playgroundPresenter.DeactivateCells();
            
            if (enemyTeamPresenter.Model.MoveState)
            {
                _playgroundPresenter.SetAciveCells(_bfsSearch.GetCellsByLength(
                    enemyTeamPresenter.Model.Energy, 
                    new Node(
                        enemyTeamPresenter.Model.HeightCellIndex, 
                        enemyTeamPresenter.Model.WidthCellIndex, 
                        _playgroundPresenter.Model.GetCellPresenter(enemyTeamPresenter.Model.HeightCellIndex, enemyTeamPresenter.Model.WidthCellIndex).Model.CellType,
                        true
                    ), 
                    _playgroundPresenter,
                    false
                ));
                _playgroundPresenter.ActivateRedCells();
            }
            else
            {
                _playgroundPresenter.ResetActiveCells();
            }
        }

        private void OnEnemyFollowClick(EnemyTeamPresenter enemyTeamPresenter)
        {
            _playgroundPresenter.DeactivateCells();
            List<Pair<int, int>> route = new List<Pair<int, int>>();
            if (_playerTeamPresenter.Model.MoveState && _bfsSearch.TryBuildRoute(
                    new Node(
                        _playerTeamPresenter.Model.HeightCellIndex, 
                        _playerTeamPresenter.Model.WidthCellIndex, 
                        _playgroundPresenter.Model.GetCellPresenter(_playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex).Model.CellType,
                        true
                    ),
                    new Node(enemyTeamPresenter.Model.HeightCellIndex, enemyTeamPresenter.Model.WidthCellIndex,
                        _playgroundPresenter.Model.GetCellPresenter(enemyTeamPresenter.Model.HeightCellIndex, enemyTeamPresenter.Model.WidthCellIndex).Model.CellType,
                        _playgroundPresenter.CheckCellOnCharacter(enemyTeamPresenter.Model.HeightCellIndex, enemyTeamPresenter.Model.WidthCellIndex)),
                    _playgroundPresenter,
                    false,
                    out route
                ))
            {
                _playgroundPresenter.RemoveCharacterFromCell(enemyTeamPresenter, _playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex);
                _playerTeamPresenter.AddRoute(route);
                _playerTeamPresenter.Move(_coroutineRunner, _playgroundPresenter);
            }
        }

        private void OnCellClicked()
        {
            _playgroundPresenter.DeactivateCells();
            _playgroundPresenter.ResetActiveCells();
            _playerTeamPresenter.Model.ResetMovementSettings();
        }

        private void OnMoveCellClicked(int heightIndex, int widthIndex)
        {
            _playgroundPresenter.DeactivateCells();
            List<Pair<int, int>> route = new List<Pair<int, int>>();
            if (_playerTeamPresenter.Model.MoveState && _bfsSearch.TryBuildRoute(
                    new Node(
                        _playerTeamPresenter.Model.HeightCellIndex, 
                        _playerTeamPresenter.Model.WidthCellIndex, 
                        _playgroundPresenter.Model.GetCellPresenter(_playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex).Model.CellType,
                        true
                        ),
                    new Node(heightIndex, widthIndex,
                    _playgroundPresenter.Model.GetCellPresenter(heightIndex, widthIndex).Model.CellType,
                    _playgroundPresenter.CheckCellOnCharacter(heightIndex, widthIndex)),
                    _playgroundPresenter,
                    true,
                    out route
                ))
            {
                _playgroundPresenter.RemoveCharacterFromCell(_playerTeamPresenter, _playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex);
                _playerTeamPresenter.AddRoute(route);
                _playerTeamPresenter.Move(_coroutineRunner, _playgroundPresenter);
            }
        }

        private void OnTeamsCollision(List<TeamPresenter> teams)
        {
            _battlegroundPresenter.StartBattle(teams);
        }
    }
}