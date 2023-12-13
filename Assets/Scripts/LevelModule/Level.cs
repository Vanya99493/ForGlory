using System;
using System.Collections;
using System.Collections.Generic;
using BattleModule.PresenterPart;
using CameraModule;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart;
using CharacterModule.PresenterPart.BehaviourModule;
using CharacterModule.ViewPart;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
using PlaygroundModule.ViewPart;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelModule
{
    public class Level
    {
        public event Action StartStepChangingAction;
        public event Action EndStepChangingAction;
        
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly CellDataProvider _cellDataProvider;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;
        
        private readonly WideSearch _bfsSearch;
        
        private PlaygroundPresenter _playgroundPresenter;
        private PlayerTeamPresenter _playerTeamPresenter;
        private List<EnemyTeamPresenter> _enemiesTeamPresenters;
        private BattlegroundPresenter _battlegroundPresenter;

        private EnemyBehaviour _enemyBehaviour;
        private bool _isStepChanging;

        private int _enemiesStepCounter;
        private int _playerStepCounter;
        
        public Level(ICoroutineRunner coroutineRunner, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _coroutineRunner = coroutineRunner;
            _cellDataProvider = cellDataProvider;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;

            _bfsSearch = new WideSearch(cellDataProvider);
            ServiceLocator.Instance.RegisterService(_bfsSearch);

            _enemyBehaviour = new EnemyBehaviour();
        }

        public void StartLevel(int enemiesCount)
        {
            _isStepChanging = false;
            _enemiesTeamPresenters = new List<EnemyTeamPresenter>();
            
            CreatePlayground();
            CreateBattleground();

            _enemiesStepCounter = enemiesCount;
            _playerStepCounter = 1;
            
            CreatePlayer();
            CreateEnemies(enemiesCount);
        }

        public void ResetLevel()
        {
            _playgroundPresenter.DeactivateCells();
        }

        public void RemoveLevel()
        {
            _playgroundPresenter.Destroy();
            _playerTeamPresenter.Destroy();
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
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
            _isStepChanging = true;
            StartStepChangingAction?.Invoke();

            new PlayerBehaviour().StartPlayerBehaviour(_playerTeamPresenter, _playgroundPresenter);
            _enemyBehaviour.StartEnemiesBehaviour(_enemiesTeamPresenters, _playgroundPresenter, _playerTeamPresenter);
            
            _playgroundPresenter.DeactivateCells();

            _coroutineRunner.StartCoroutine(BlockCoroutine());
        }

        private IEnumerator BlockCoroutine()
        {
            while (_isStepChanging)
            {
                yield return null;
            }
            
            _playerTeamPresenter.Model.ResetEnergy();
            _playerTeamPresenter.Model.ResetMovementSettings();

            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.Model.ResetEnergy();
                enemyTeamPresenter.Model.ResetMovementSettings();
            }
            EndStepChangingAction?.Invoke();
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
            _playerTeamPresenter.Model.EndStepAction += OnEndPlayerMove;
           
            _playerTeamPresenter.EnterIdleState(_playgroundPresenter);
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
                        enemies[j] = new EnemyCharacterPresenter(new EnemyCharacterModel("Enemy", 50, 3, 2, 5), (EnemyCharacterView)characters[j]);
                    }
            
                    TeamModel model = new EnemyTeamModel(heightSpawnCellIndex, widthSpawnCellIndex, enemies);
                    EnemyTeamPresenter enemyTeamPresenter = new EnemyTeamPresenter(model, view);
                    enemyTeamPresenter.Model.SetPosition(_playgroundPresenter);
                    enemyTeamPresenter.ClickOnCharacterAction += OnEnemyTeamClicked;
                    enemyTeamPresenter.FollowClickAction += OnEnemyFollowClick;

                    enemyTeamPresenter.EnterIdleState(_playgroundPresenter);
                    
                    if (_playgroundPresenter.SetCharacterOnCell(enemyTeamPresenter, heightSpawnCellIndex, widthSpawnCellIndex, true))
                    {
                        enemyTeamPresenter.Model.EndStepAction += OnEndEnemyMove;
                        _enemiesTeamPresenters.Add(enemyTeamPresenter);
                        break;
                    }

                    enemyTeamPresenter.Destroy();
                }
            }
        }

        private void OnEndPlayerMove()
        {
            --_playerStepCounter;
            CheckStepMovement();
        }

        private void OnEndEnemyMove()
        {
            --_enemiesStepCounter;
            CheckStepMovement();
        }

        private void CheckStepMovement()
        {
            if (_enemiesStepCounter <= 0 && _playerStepCounter <= 0)
            {
                _enemiesStepCounter = _enemiesTeamPresenters.Count;
                _playerStepCounter = 1;
                _isStepChanging = false;
            }
        }

        private void OnPlayerTeamClicked(TeamPresenter playerTeamPresenter)
        {
            if (_isStepChanging)
                return;
            
            _playgroundPresenter.DeactivateCells();
            
            if (playerTeamPresenter.Model.MoveState)
            {
                _playgroundPresenter.SetAciveCells(_bfsSearch.GetCellsByLength(
                    playerTeamPresenter.Model.TeamEnergy, 
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
            if (_isStepChanging)
                return;

            _playerTeamPresenter.Model.ResetMovementSettings();
            _playgroundPresenter.DeactivateCells();
            
            if (enemyTeamPresenter.Model.MoveState)
            {
                _playgroundPresenter.SetAciveCells(_bfsSearch.GetCellsByLength(
                    enemyTeamPresenter.Model.TeamEnergy, 
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
            if (_isStepChanging)
                return;

            _playgroundPresenter.DeactivateCells();
            _playerTeamPresenter.EnterFollowState(_playgroundPresenter, enemyTeamPresenter);
        }

        private void OnCellClicked()
        {
            if (_isStepChanging)
                return;

            _playgroundPresenter.DeactivateCells();
            _playgroundPresenter.ResetActiveCells();
            _playerTeamPresenter.Model.ResetMovementSettings();
        }

        private void OnMoveCellClicked(int heightIndex, int widthIndex)
        {
            if (_isStepChanging)
                return;

            _playgroundPresenter.DeactivateCells();
            _playerTeamPresenter.EnterMoveState(_playgroundPresenter, heightIndex, widthIndex);
        }

        private void OnTeamsCollision(List<TeamPresenter> teams)
        {
            _battlegroundPresenter.StartBattle(teams);
        }
    }
}