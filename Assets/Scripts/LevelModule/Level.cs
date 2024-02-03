using System;
using System.Collections;
using System.Collections.Generic;
using BattleModule.ModelPart;
using BattleModule.PresenterPart;
using BattleModule.ViewPart;
using CameraModule;
using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.PresenterPart;
using CharacterModule.PresenterPart.BehaviourModule;
using CharacterModule.PresenterPart.FactoryModule;
using CharacterModule.ViewPart;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using Infrastructure.ServiceLocatorModule;
using LevelModule.Data;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ModelPart.Data;
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
        public event Action BattleStartAction;
        public event Action BattleEndAction;
        public event Action EndGameAction;
        
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly CellDataProvider _cellDataProvider;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;
        
        private readonly WideSearch _bfsSearch;
        
        private PlaygroundPresenter _playgroundPresenter;
        private PlayerTeamPresenter _playerTeamPresenter;
        private List<EnemyTeamPresenter> _enemiesTeamPresenters;
        private BattlegroundPresenter _battlegroundPresenter;

        private PlayerTeamFactory _playerTeamFactory;
        private EnemyTeamFactory _enemyTeamFactory;
        private EnemyBehaviour _enemyBehaviour;
        private bool _isStepChanging;
        
        // need to rebase it to level data builder
        private CharacterIdSetter _characterIdSetter;

        private int _enemiesStepCounter;
        private int _playerStepCounter;
        
        public Level(ICoroutineRunner coroutineRunner, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            _coroutineRunner = coroutineRunner;
            _cellDataProvider = cellDataProvider;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;

            _bfsSearch = new WideSearch(cellDataProvider);
            ServiceLocator.Instance.RegisterService(_bfsSearch);
        }

        public void StartLevel(LevelData levelData)
        {
            _isStepChanging = false;
            _enemiesTeamPresenters = new List<EnemyTeamPresenter>();
            _enemyBehaviour = new EnemyBehaviour();
            _characterIdSetter = new CharacterIdSetter(0);
            
            CreatePlayground(levelData.PlaygroundData);
            CreateBattleground();

            _enemiesStepCounter = levelData.TeamsData.EnemyTeams.Length;
            _playerStepCounter = 1;
            
            CreatePlayer(levelData.TeamsData.PlayerTeam);
            CreateEnemies(levelData.TeamsData.EnemyTeams);
        }

        public void ResetLevel()
        {
            _playgroundPresenter.DeactivateCells();
        }

        public void RemoveLevel()
        {
            _playgroundPresenter.Destroy();
            _playgroundPresenter = null;
            _battlegroundPresenter.Destroy();
            _battlegroundPresenter = null;
            _playerTeamPresenter?.Destroy();
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.Destroy();
            }
            _playerTeamFactory.RemoveParent();
            _enemyTeamFactory.RemoveParent();
        }

        public void SetCameraTarget(CameraFollower camera)
        {
            camera.SetTarget(_playerTeamPresenter.View.transform);
        }

        public void NextStep()
        {
            _isStepChanging = true;
            StartStepChangingAction?.Invoke();

            _playerTeamPresenter.StartBehave(_playgroundPresenter);
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.StartBehave(_playgroundPresenter);
            }
            
            _playgroundPresenter.DeactivateCells();

            _coroutineRunner.StartCoroutine(BlockCoroutine());
        }

        private IEnumerator BlockCoroutine()
        {
            float passedTime = 0f;
            while (_isStepChanging)
            {
                passedTime += Time.deltaTime;
                yield return null;
                if (passedTime > 5f)
                    break;
            }
            yield return new WaitForSeconds(0.5f);
            
            _playerTeamPresenter.Model.ResetEnergy();
            _playerTeamPresenter.Model.ResetMovementSettings();

            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.Model.ResetEnergy();
                enemyTeamPresenter.Model.ResetMovementSettings();
            }
            EndStepChangingAction?.Invoke();
            
            _isStepChanging = false;
        }
        
        private void CreatePlayground(PlaygroundData emptyPlaygroundData)
        {
            PlaygroundView view = new PlaygroundFactory().InstantiatePlayground();
            PlaygroundModel model = new PlaygroundModel();
            _playgroundPresenter = new PlaygroundPresenter(model, view, _cellDataProvider);
            
            _playgroundPresenter.CreateAndSpawnPlayground(view.transform, emptyPlaygroundData.Height, emptyPlaygroundData.Width, 
                emptyPlaygroundData.LengthOfWaterLine, emptyPlaygroundData.LengthOfCoast, 
                emptyPlaygroundData.Height * 1f, emptyPlaygroundData.Width * 1f, _cellDataProvider, 
                OnMoveCellClicked, OnCellClicked, /*OnTeamsCollision*/ 
                (List<TeamPresenter> teams) => _coroutineRunner.StartCoroutine(WaitOnEndStepChanging(teams)));
        }

        private void CreateBattleground()
        {
            BattlegroundView view = new BattlegroundFactory().InstantiateBattleground(_gameScenePrefabsProvider.GetBattlgroundView());
            BattlegroundModel model = new BattlegroundModel();
            _battlegroundPresenter = new BattlegroundPresenter(model, view);
            _battlegroundPresenter.EndBattle += OnEndBattle;
        }

        private void CreatePlayer(TeamData playerTeamData)
        {
            (int heightSpawnCellIndex, int widthSpawnCellIndex) = FindEmptyCell();

            playerTeamData.HeightCellIndex = heightSpawnCellIndex;
            playerTeamData.WidthCellIndex = widthSpawnCellIndex;

            _playerTeamFactory = new PlayerTeamFactory();
            _playerTeamPresenter = _playerTeamFactory.InstantiateTeam(_gameScenePrefabsProvider.GetTeamView(), playerTeamData, new PlayerBehaviour()) as PlayerTeamPresenter;
            
            _playerTeamPresenter.Model.SetPosition(_playgroundPresenter);
            _playgroundPresenter.SetCharacterOnCell(_playerTeamPresenter, heightSpawnCellIndex, widthSpawnCellIndex, true);
            
            _playerTeamPresenter.ClickOnCharacterAction += OnPlayerTeamClicked;
            _playerTeamPresenter.Model.EndStepAction += OnEndPlayerMove;
           
            _playerTeamPresenter.EnterIdleState(_playgroundPresenter);
        }

        private void CreateEnemies(TeamData[] enemyTeamsData)
        {
            _enemyTeamFactory = new EnemyTeamFactory();
            
            for (int i = 0; i < enemyTeamsData.Length; i++)
            {
                while (true)
                {
                    (int heightSpawnCellIndex, int widthSpawnCellIndex) = FindEmptyCell();

                    enemyTeamsData[i].HeightCellIndex = heightSpawnCellIndex;
                    enemyTeamsData[i].WidthCellIndex = widthSpawnCellIndex;
                    
                    EnemyTeamPresenter enemyTeamPresenter = 
                        _enemyTeamFactory.InstantiateTeam(_gameScenePrefabsProvider.GetTeamView(), enemyTeamsData[i], new EnemyBehaviour()) as EnemyTeamPresenter;
                    
                    enemyTeamPresenter.Model.SetPosition(_playgroundPresenter);
                    //_playgroundPresenter.SetCharacterOnCell(enemyTeamPresenter, enemyTeamsData[i].HeightCellIndex, enemyTeamsData[i].WidthCellIndex, true);
                    
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

        private (int, int) FindEmptyCell()
        {
            int heightSpawnCellIndex, widthSpawnCellIndex;
            do
            {
                heightSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.PlaygroundHeight);
                widthSpawnCellIndex = Random.Range(0, _playgroundPresenter.Model.PlaygroundWidth);
            } while (_playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Hill &&
                     _playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.CellType != CellType.Plain &&
                     !_playgroundPresenter.CheckCellOnCharacter(heightSpawnCellIndex, widthSpawnCellIndex));

            return (heightSpawnCellIndex, widthSpawnCellIndex);
        }

        private void OnEndPlayerMove()
        {
            --_playerStepCounter;
            //Debug.Log("P: " + _playerStepCounter);
            CheckStepMovement();
        }

        private void OnEndEnemyMove()
        {
            --_enemiesStepCounter;
            //Debug.Log("E: " + _enemiesStepCounter);
            CheckStepMovement();
        }

        private void CheckStepMovement()
        {
            if (_enemiesStepCounter <= 0 && _playerStepCounter <= 0)
            {
                _enemiesStepCounter = _enemiesTeamPresenters.Count;
                _playerStepCounter = _playerTeamPresenter != null ? 1 : 0;
                //Debug.Log("P: " + _playerStepCounter + " E: " + _enemiesStepCounter);
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

            _playgroundPresenter?.DeactivateCells();
            _playgroundPresenter?.ResetActiveCells();
            _playerTeamPresenter?.Model.ResetMovementSettings();
        }

        private void OnMoveCellClicked(int heightIndex, int widthIndex)
        {
            if (_isStepChanging)
                return;

            _playgroundPresenter.DeactivateCells();
            _playerTeamPresenter.EnterMoveState(_playgroundPresenter, heightIndex, widthIndex);
        }

        private IEnumerator WaitOnEndStepChanging(List<TeamPresenter> teams)
        {
            while (_isStepChanging)
            {
                yield return null;
            }
            OnTeamsCollision(teams);
        }

        private void OnTeamsCollision(List<TeamPresenter> teams)
        {
            HidePlayground();
            BattleStartAction?.Invoke();

            foreach (TeamPresenter team in teams)
            {
                team.View.gameObject.SetActive(true);
                TeamFactory.UpScaleTeam(team);
            }
            _battlegroundPresenter.ShowBattleground();
            
            _battlegroundPresenter.StartBattle(teams);
        }

        private void OnEndBattle(bool isWin, PlayerTeamPresenter playerTeamPresenter, EnemyTeamPresenter enemyTeamPresenter)
        {
            _battlegroundPresenter.HideBattleground();
            ShowPlayground();
            BattleEndAction?.Invoke();
            
            if (isWin)
            {
                TeamFactory.DownScaleTeam(playerTeamPresenter);
                TeamFactory.ResetTeamPosition(playerTeamPresenter);
                _enemiesTeamPresenters.Remove(enemyTeamPresenter);
                --_enemiesStepCounter;
                _playgroundPresenter.RemoveCharacterFromCell(enemyTeamPresenter,
                    enemyTeamPresenter.Model.HeightCellIndex, enemyTeamPresenter.Model.WidthCellIndex);
                enemyTeamPresenter.Destroy();
            }
            else
            {
                TeamFactory.DownScaleTeam(enemyTeamPresenter);
                TeamFactory.ResetTeamPosition(enemyTeamPresenter);
                --_playerStepCounter;
                playerTeamPresenter.Destroy();
                _playgroundPresenter.RemoveCharacterFromCell(playerTeamPresenter,
                    playerTeamPresenter.Model.HeightCellIndex, playerTeamPresenter.Model.WidthCellIndex);
                _playerTeamPresenter = null;
                EndGameAction?.Invoke();
            }
        }

        private void HidePlayground()
        {
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.View.gameObject.SetActive(false);
            }
            _playgroundPresenter.HidePlayground();
        }

        private void ShowPlayground()
        {
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.View.gameObject.SetActive(true);
            }
            _playgroundPresenter.ShowPlayground();
        }
    }
}