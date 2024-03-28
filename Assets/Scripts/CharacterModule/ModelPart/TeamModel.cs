using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using CharacterModule.PresenterPart;
using CustomClasses;
using Infrastructure.CoroutineRunnerModule;
using PlaygroundModule.ModelPart;
using PlaygroundModule.PresenterPart;
using UnityEngine;

namespace CharacterModule.ModelPart
{
    public abstract class TeamModel
    {
        public event Action<CharacterPresenter[]> SetupCharactersAction;
        public event Action<Vector3, Direction> MoveAction;
        public event Action EndStepAction;
        public event Action<PlaygroundPresenter> EndMoveAction;
        
        private float _speed = 4f;
        private Queue<Pair<int, int>> _route;
        private int _teamEnergy;
        protected CharacterPresenter _leftVanguard;
        protected CharacterPresenter _rightVanguard;
        protected CharacterPresenter _rearguard;

        public int CharactersCount => 3;
        public int RoutLength => _route.Count;
        
        public bool MoveState { get; private set; }
        public bool CanMove { get; private set; }
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }
        public int TeamEnergy { get; private set; }

        protected TeamModel(int heightCellIndex, int widthCellIndex)
        {
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;
            MoveState = false;
            CanMove = true;
            _route = new Queue<Pair<int, int>>();
        }

        public void SetCharacters(CharacterPresenter[] characters)
        {
            if (characters.Length == 0)
                return;

            List<CharacterPresenter> characterPresenters = new List<CharacterPresenter>();

            _leftVanguard = null;
            _rightVanguard = null;
            _rearguard = null;
            
            if (characters.Length > 0 && characters[0] != null)
            {
                _leftVanguard = characters[0];
                _leftVanguard.DeathAction += OnKillCharacter;
                characterPresenters.Add(_leftVanguard);
            }

            if (characters.Length > 1 && characters[1] != null)
            {
                _rightVanguard = characters[1];
                _rightVanguard.DeathAction += OnKillCharacter;
                characterPresenters.Add(_rightVanguard);
            }

            if (characters.Length > 2 && characters[2] != null)
            {
                _rearguard = characters[2];
                _rearguard.DeathAction += OnKillCharacter;
                characterPresenters.Add(_rearguard);
            }
            
            _teamEnergy = characterPresenters[0].Model.MaxEnergy;
            foreach (var characterPresenter in characterPresenters)
            {
                if (characterPresenter.Model.MaxEnergy < _teamEnergy)
                    _teamEnergy = characterPresenter.Model.MaxEnergy;
            }
            TeamEnergy = _teamEnergy;

            SetupCharactersAction?.Invoke(new[] { _leftVanguard, _rightVanguard, _rearguard });
        }

        public void SetCharacters(CharacterPresenter leftVanguard, CharacterPresenter rightVanguard, CharacterPresenter rearguard)
        {
            _leftVanguard = leftVanguard;
            _rightVanguard = rightVanguard;
            _rearguard = rearguard;

            List<CharacterPresenter> characters = new List<CharacterPresenter>();

            if (leftVanguard != null)
            {
                characters.Add(leftVanguard);
                _leftVanguard = leftVanguard;
                _leftVanguard.DeathAction += OnKillCharacter;
            }
            if (rightVanguard != null)
            {
                characters.Add(rightVanguard);
                _rightVanguard = rightVanguard;
                _rightVanguard.DeathAction += OnKillCharacter;
            }
            if (rearguard != null)
            {
                characters.Add(rearguard);
                _rearguard = rearguard;
                _rearguard.DeathAction += OnKillCharacter;
            }
            
            _teamEnergy = characters[0].Model.MaxEnergy;
            foreach (var character in characters)
            {
                if (character.Model.MaxEnergy < _teamEnergy)
                    _teamEnergy = character.Model.MaxEnergy;
            }
            TeamEnergy = _teamEnergy;
            
            SetupCharactersAction?.Invoke(new[] { _leftVanguard, _rightVanguard, _rearguard });
        }

        public CharacterPresenter GetCharacterPresenter(int index)
        {
            switch (index)
            {
                case 0: return _leftVanguard;
                case 1: return _rightVanguard;
                case 2: return _rearguard;
                default: 
                    Debug.Log($"Cannot get character by index {index}");
                    return null;
            }
        }

        public List<CharacterPresenter> GetCharacters()
        {
            List<CharacterPresenter> characters = new List<CharacterPresenter>();
            
            if (_leftVanguard != null)
                characters.Add(_leftVanguard);
            if (_rightVanguard != null)
                characters.Add(_rightVanguard);
            if (_rearguard != null)
                characters.Add(_rearguard);

            return characters;
        }

        public int GetAliveCharactersCount()
        {
            int aliveCharactersCount = 0;

            if (_leftVanguard != null)
                aliveCharactersCount++;
            if (_rightVanguard != null)
                aliveCharactersCount++;
            if (_rearguard != null)
                aliveCharactersCount++;

            return aliveCharactersCount;
        }

        public void SwitchMoveState()
        {
            MoveState = !MoveState;
        }

        public void AddRoute(List<Pair<int, int>> route)
        {
            _route.Clear();
            foreach (var checkPoint in route)
            {
                _route.Enqueue(new Pair<int, int>(checkPoint.FirstValue, checkPoint.SecondValue));
            }
        }

        public void ResetEnergy()
        {
            TeamEnergy = _teamEnergy;
        }
        
        public void ResetMovementSettings()
        {
            MoveState = false;
            CanMove = true;
        }

        public void Move(ICoroutineRunner coroutineRunner, PlaygroundPresenter playgroundPresenter, TeamPresenter teamPresenter)
        {
            if (_route.Count > 0)
            {
                CanMove = false;
                coroutineRunner.StartCoroutine(MovementCoroutine(playgroundPresenter, teamPresenter));
            }
            else
            {
                EndStepAction?.Invoke();
            }
        }

        public void SetPosition(PlaygroundPresenter playgroundPresenter)
        {
            MoveAction?.Invoke(playgroundPresenter.Model.GetCellPresenter(HeightCellIndex, WidthCellIndex).Model.MoveCellPosition, Direction.Down);
        }

        private IEnumerator MovementCoroutine(PlaygroundPresenter playgroundPresenter, TeamPresenter teamPresenter)
        {
            while (_route.Count > 0 && TeamEnergy > 0)
            {
                Pair<int, int> checkPoint = _route.Peek();
                if (playgroundPresenter.CheckCellOnCharacter(checkPoint.FirstValue, checkPoint.SecondValue) &&
                    _route.Count > 1)
                {
                    _route.Clear();
                    break;
                }
                if (playgroundPresenter.PreSetCharacterOnCell(teamPresenter, checkPoint.FirstValue, checkPoint.SecondValue) == false)
                {
                    _route.Clear();
                    break;
                }
                playgroundPresenter.RemoveCharacterFromCell(teamPresenter, HeightCellIndex, WidthCellIndex);

                Direction direction = HeightCellIndex - checkPoint.FirstValue == 0
                    ?
                    WidthCellIndex - checkPoint.SecondValue == 1 ? Direction.Left : Direction.Right
                    : HeightCellIndex - checkPoint.FirstValue == -1
                        ? Direction.Down
                        : Direction.Up;
                    
                Vector3 targetPosition = playgroundPresenter.Model.GetCellPresenter(checkPoint.FirstValue, checkPoint.SecondValue).Model.MoveCellPosition;
                Vector3 currentPosition = playgroundPresenter.Model.GetCellPresenter(HeightCellIndex, WidthCellIndex).Model.MoveCellPosition;
                float xDifference = targetPosition.x - currentPosition.x;  
                float yDifference = targetPosition.y - currentPosition.y;  
                float zDifference = targetPosition.z - currentPosition.z;  
                float deltaTime = 0f;
            
                while(deltaTime <= 1f)
                {
                    MoveAction?.Invoke(new Vector3(
                        currentPosition.x + xDifference * deltaTime,
                        currentPosition.y + yDifference * deltaTime,
                        currentPosition.z + zDifference * deltaTime
                    ), direction);
                    deltaTime += Time.fixedDeltaTime * _speed;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
                HeightCellIndex = checkPoint.FirstValue;
                WidthCellIndex = checkPoint.SecondValue;
                
                playgroundPresenter.SetCharacterOnCell(teamPresenter, HeightCellIndex, WidthCellIndex);
                _route.Dequeue();
                TeamEnergy--;
            }

            SwitchMoveState();
            CanMove = true;
            EndMoveAction?.Invoke(playgroundPresenter);
            EndStepAction?.Invoke();
        }

        private void OnKillCharacter(int id)
        {
            if (_leftVanguard?.Model.Id == id)
                _leftVanguard = null;
            if (_rightVanguard?.Model.Id == id)
                _rightVanguard = null;
            if (_rearguard?.Model.Id == id)
                _rearguard = null;
        }
    }
}