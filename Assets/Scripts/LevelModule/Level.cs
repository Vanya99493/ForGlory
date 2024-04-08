using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleModule.ModelPart;
using BattleModule.PresenterPart;
using BattleModule.ViewPart;
using CameraModule;
using CastleModule.ModelPart;
using CastleModule.PresenterPart;
using CastleModule.ViewPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.PresenterPart;
using CharacterModule.PresenterPart.BehaviourModule;
using CharacterModule.PresenterPart.FactoryModule;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.Providers;
using Infrastructure.SaveModule;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using LevelModule.Data;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ModelPart.Data;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
using PlaygroundModule.ViewPart;
using UIModule;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LevelModule
{
    public class Level
    {
        public event SaveDelegate SaveAction;
        public event Action StartStepChangingAction;
        public event Action EndStepChangingAction;
        public event Action BattleStartAction;
        public event Action BattleEndAction;
        public event Action WinGameAction;
        public event Action LoseGameAction;
        
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly CellDataProvider _cellDataProvider;
        private readonly GameScenePrefabsProvider _gameScenePrefabsProvider;
        private readonly UIController _uiController;
        
        private readonly WideSearch _bfsSearch;
        
        private PlaygroundPresenter _playgroundPresenter;
        private CastlePresenter _castlePresenter;
        private BattlegroundPresenter _battlegroundPresenter;
        
        private PlayerTeamPresenter _playerTeamPresenter;
        private List<EnemyTeamPresenter> _enemiesTeamPresenters;

        private Coroutine _backgroundLiveCoroutine;
        private PlayerTeamFactory _playerTeamFactory;
        private EnemyTeamFactory _enemyTeamFactory;
        private bool _isStepChanging;
        private bool _isWaitingOnEndStepChanging;

        private int _enemiesStepCounter;
        private int _playerStepCounter;
        
        public bool IsActive { get; private set; }
        
        public Level(ICoroutineRunner coroutineRunner, CellDataProvider cellDataProvider, GameScenePrefabsProvider gameScenePrefabsProvider, UIController uiController)
        {
            IsActive = false;
            _coroutineRunner = coroutineRunner;
            _cellDataProvider = cellDataProvider;
            _gameScenePrefabsProvider = gameScenePrefabsProvider;
            _uiController = uiController;

            _bfsSearch = new WideSearch(cellDataProvider);
            ServiceLocator.Instance.RegisterService(_bfsSearch);

            SubscribeUIActions();
        }

        public void StartBackgroundLevel(LevelData levelData)
        {
            ServiceLocator.Instance.RegisterService(new CharacterIdSetter(levelData.GeneralData.LastCharacterId));
            _enemiesTeamPresenters = new List<EnemyTeamPresenter>();
            
            CreatePlayground(levelData.PlaygroundData);
            CreateEnemies(levelData.TeamsData.EnemyTeams);
            
            _backgroundLiveCoroutine = _coroutineRunner.StartCoroutine(BackgroundLiveCoroutine());
        }

        private IEnumerator BackgroundLiveCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(3f);
                _coroutineRunner.StartCoroutine(EnemiesBehaveStarter());
            }
        }

        public void RemoveBackgroundLevel()
        {
            _coroutineRunner.StopCoroutine(_backgroundLiveCoroutine);
            _playgroundPresenter.Destroy();
            _playgroundPresenter = null;
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.Destroy();
            }
            _enemyTeamFactory.RemoveParent();
            ServiceLocator.Instance.UnregisterService<CharacterIdSetter>();
        }

        public void StartLevel(LevelData levelData)
        {
            ServiceLocator.Instance.RegisterService(new CharacterIdSetter(levelData.GeneralData.LastCharacterId));
            IsActive = true;
            _isStepChanging = false;
            _isWaitingOnEndStepChanging = false;
            _enemiesTeamPresenters = new List<EnemyTeamPresenter>();
            
            CreatePlayground(levelData.PlaygroundData);
            CreateCastle(levelData.TeamsData.PlayersInCastle, levelData.PlaygroundData.CastleHeightIndex, levelData.PlaygroundData.CastleWidthIndex);
            levelData.TeamsData.PlayerTeam.HeightCellIndex = _castlePresenter.CastleHeightIndex;
            levelData.TeamsData.PlayerTeam.WidthCellIndex = _castlePresenter.CastleWidthIndex;
            
            CreateBattleground();

            _enemiesStepCounter = levelData.TeamsData.EnemyTeams.Length;
            _playerStepCounter = 1;

            CreatePlayerTeam(levelData.TeamsData.PlayerTeam);
            CreateEnemies(levelData.TeamsData.EnemyTeams);
        }

        public void ResetLevel()
        {
            _playgroundPresenter.DeactivateCells();
        }

        public void RemoveLevel()
        {
            _battlegroundPresenter.EndBattle -= OnEndBattle;
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

            IsActive = false;
            ServiceLocator.Instance.UnregisterService<CharacterIdSetter>();
        }

        public void SetCameraTarget(CameraFollower camera)
        {
            camera.SetTarget(_playerTeamPresenter.View.transform);
        }

        public void NextStep()
        {
            _isStepChanging = true;
            StartStepChangingAction?.Invoke();

            _playgroundPresenter.DeactivateCells();
            
            _coroutineRunner.StartCoroutine(StepBehaveStarter());
        }

        private IEnumerator StepBehaveStarter()
        {
            _playerTeamPresenter.StartBehave(_playgroundPresenter);
            
            if (_playerTeamPresenter.Model.RoutLength > 0)
            {
                while (_playerTeamPresenter.Model.TeamCurrentEnergy != 0 && _playerTeamPresenter.Model.RoutLength != 0)
                    yield return null;

                if (_playerTeamPresenter.Model.TeamCurrentEnergy > 0 || _isWaitingOnEndStepChanging)
                    EndStepChanging();
                else
                    _coroutineRunner.StartCoroutine(EnemiesBehaveStarter());
            }
            else
                _coroutineRunner.StartCoroutine(EnemiesBehaveStarter());
        }

        private IEnumerator EnemiesBehaveStarter()
        {
            yield return new WaitForSeconds(0.5f);
            
            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.StartBehave(_playgroundPresenter);
            }

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
            
            _playerTeamPresenter?.Model.ResetEnergy();
            _playerTeamPresenter?.Model.ResetMovementSettings();

            foreach (EnemyTeamPresenter enemyTeamPresenter in _enemiesTeamPresenters)
            {
                enemyTeamPresenter.Model.ResetEnergy();
                enemyTeamPresenter.Model.ResetMovementSettings();
            }

            _castlePresenter?.ResetHeroesEnergy();

            EndStepChanging();
        }

        private void EndStepChanging()
        {
            EndStepChangingAction?.Invoke();
            _isStepChanging = false;
        }
        
        private void CreatePlayground(CreatedPlaygroundData playgroundData)
        {
            PlaygroundView view = new PlaygroundFactory().InstantiatePlayground();
            PlaygroundModel model = new PlaygroundModel();
            _playgroundPresenter = new PlaygroundPresenter(model, view, _cellDataProvider);
            
            _playgroundPresenter.SpawnPlayground(view.transform, playgroundData.Playground, OnMoveCellClicked, OnCellClicked, 
                teams => _coroutineRunner.StartCoroutine(WaitOnEndStepChanging(teams)));
        }

        private void CreateCastle(CharacterData[] playersData, int castleHeightIndex, int castleWidthIndex)
        {
            if(castleHeightIndex == -1 || castleWidthIndex == -1)
                (castleHeightIndex, castleWidthIndex) = FindEmptyCell();
            
            CastleModel model = new CastleModel(castleHeightIndex, castleWidthIndex);
            CastleView view = new CastleFactory().InstantiateCastle(_gameScenePrefabsProvider.GetCastleView(), _playgroundPresenter.View.transform);
            _castlePresenter = new CastlePresenter(model, view);
            _playgroundPresenter.Model.GetCellPresenter(castleHeightIndex, castleWidthIndex).Model.SetCastleOnCell(_castlePresenter);
            _castlePresenter.SetPosition(_playgroundPresenter);

            _castlePresenter.TurnOnEvent += OnEnterCellWithCastle;
            _castlePresenter.TurnOffEvent += OnExitCellWithCastleCastle;
            
            PlayerCharacterPresenter[] instantiatedPlayers = CreateHeroes(playersData, _castlePresenter.View.transform);
            _castlePresenter.SetCharactersInCastle(instantiatedPlayers);

            ServiceLocator.Instance.GetService<UIController>().castleMenuUIPanel.EnterCastleAction += OnEnterCastle;
            ServiceLocator.Instance.GetService<UIController>().castleMenuUIPanel.AcceptCastleAction += OnAcceptTeamChanging;
        }

        private void OnEnterCellWithCastle()
        {
            ServiceLocator.Instance.GetService<UIController>().gameHudUIPanel.ShowEnterButton();
        }

        private void OnExitCellWithCastleCastle()
        {
            ServiceLocator.Instance.GetService<UIController>().gameHudUIPanel.HideEnterButton();
        }

        private void OnEnterCastle()
        {
            ServiceLocator.Instance.GetService<UIController>().castleMenuUIPanel.Enter(_castlePresenter.GetCharactersInCastle(), _playerTeamPresenter);
        }

        private void OnAcceptTeamChanging(int[] newPlayersTeamId)
        {
            _playerTeamPresenter.View.Rotate(Direction.Down);
            
            List<CharacterPresenter> allCharacters = _castlePresenter.GetCharactersInCastle().ToList();
            foreach (var character in _playerTeamPresenter.Model.GetCharacters())
                allCharacters.Add(character as PlayerCharacterPresenter);

            PlayerCharacterPresenter[] newTeamCharacters = new PlayerCharacterPresenter[newPlayersTeamId.Length];
            List<PlayerCharacterPresenter> charactersInCastle = new List<PlayerCharacterPresenter>();

            foreach (var character in allCharacters)
            {
                var wasAdded = false;
                for (int i = 0; i < newTeamCharacters.Length; i++)
                {
                    if (character.Model.Id == newPlayersTeamId[i])
                    {
                        newTeamCharacters[i] = character as PlayerCharacterPresenter;
                        wasAdded = true;
                        break;
                    }
                }

                if (!wasAdded)
                    charactersInCastle.Add(character as PlayerCharacterPresenter);
            }
            
            _playerTeamPresenter.Model.SetCharacters(newTeamCharacters);
            _castlePresenter.SetCharactersInCastle(charactersInCastle.ToArray());
        }

        private PlayerCharacterPresenter[] CreateHeroes(CharacterData[] playersInCastle, Transform parent)
        {
            PlayerCharacterFactory characterFactory = new PlayerCharacterFactory();
            PlayerCharacterPresenter[] playersInCastlePresenters = new PlayerCharacterPresenter[playersInCastle.Length]; 

            for (int i = 0; i < playersInCastle.Length; i++)
            {
                playersInCastlePresenters[i] = characterFactory.InstantiateCharacter(
                    _gameScenePrefabsProvider.GetCharacterByName(playersInCastle[i].Name).CharacterPrefab,
                    playersInCastle[i],
                    parent,
                    _castlePresenter.View.transform.position
                ) as PlayerCharacterPresenter;
                playersInCastlePresenters[i].View.HideView();
            }

            return playersInCastlePresenters;
        }

        private void CreatePlayerTeam(TeamData playerTeamData)
        {
            _playerTeamFactory = new PlayerTeamFactory();
            _playerTeamPresenter = _playerTeamFactory.InstantiateTeam(_gameScenePrefabsProvider, playerTeamData, new PlayerBehaviour()) as PlayerTeamPresenter;
            
            _playerTeamPresenter.Model.SetPosition(_playgroundPresenter);
            _playgroundPresenter.SetCharacterOnCell(_playerTeamPresenter, playerTeamData.HeightCellIndex, playerTeamData.WidthCellIndex, true);
            
            _playerTeamPresenter.ClickOnCharacterAction += OnPlayerTeamClicked;
            _playerTeamPresenter.Model.EndStepAction += OnEndPlayerMove;
           
            _playerTeamPresenter.EnterIdleState(_playgroundPresenter);
        }

        private void CreateBattleground()
        {
            BattlegroundView view = new BattlegroundFactory().InstantiateBattleground(_gameScenePrefabsProvider.GetBattlegroundView());
            BattlegroundModel model = new BattlegroundModel();
            _battlegroundPresenter = new BattlegroundPresenter(model, view);
            _battlegroundPresenter.EndBattle += OnEndBattle;
        }

        private void CreateEnemies(TeamData[] enemyTeamsData)
        {
            _enemyTeamFactory = new EnemyTeamFactory();
            
            for (int i = 0; i < enemyTeamsData.Length; i++)
            {
                int heightSpawnCellIndex, widthSpawnCellIndex;
                do
                {
                    (heightSpawnCellIndex, widthSpawnCellIndex) = FindEmptyCell();
                } while (_playgroundPresenter.Model.GetCellPresenter(heightSpawnCellIndex, widthSpawnCellIndex).Model.TeamsOnCell.Count > 0);
                
                enemyTeamsData[i].HeightCellIndex = heightSpawnCellIndex;
                enemyTeamsData[i].WidthCellIndex = widthSpawnCellIndex;
                
                EnemyTeamPresenter enemyTeamPresenter = 
                    _enemyTeamFactory.InstantiateTeam(_gameScenePrefabsProvider, enemyTeamsData[i], new EnemyBehaviour()) as EnemyTeamPresenter;
                    
                enemyTeamPresenter.Model.SetPosition(_playgroundPresenter);
                    
                enemyTeamPresenter.ClickOnCharacterAction += OnEnemyTeamClicked;
                enemyTeamPresenter.FollowClickAction += OnEnemyFollowClick;

                enemyTeamPresenter.EnterIdleState(_playgroundPresenter);

                _playgroundPresenter.SetCharacterOnCell(enemyTeamPresenter, enemyTeamsData[i].HeightCellIndex,
                    enemyTeamsData[i].WidthCellIndex, true);
                enemyTeamPresenter.Model.EndStepAction += OnEndEnemyMove;
                _enemiesTeamPresenters.Add(enemyTeamPresenter);
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

        private void SubscribeUIActions()
        {
            _uiController.pauseMenuUIPanel.SaveLevelAction += () =>
            {
                _uiController.ActivateConfirmWindow("Are you sure?", () =>
                {
                    SaveAction?.Invoke(_playgroundPresenter, _castlePresenter, _playerTeamPresenter, _enemiesTeamPresenters, 
                        ServiceLocator.Instance.GetService<CharacterIdSetter>());
                });
            };
        }

        private void OnEndPlayerMove()
        {
            --_playerStepCounter;
            _playgroundPresenter.Model.GetCellPresenter(_playerTeamPresenter.Model.HeightCellIndex, _playerTeamPresenter.Model.WidthCellIndex).
                Model.ActivateCastleEvent();
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
                    playerTeamPresenter.Model.TeamCurrentEnergy, 
                    new MoveNode(
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
                    enemyTeamPresenter.Model.TeamCurrentEnergy, 
                    new MoveNode(
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
            _isWaitingOnEndStepChanging = true;
            
            while (_isStepChanging)
            {
                yield return null;
            }

            _isWaitingOnEndStepChanging = false;
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
                if (_enemiesTeamPresenters.Count <= 0)
                {
                    IsActive = false;
                    WinGameAction?.Invoke();
                }
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
                IsActive = false;
                LoseGameAction?.Invoke();
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