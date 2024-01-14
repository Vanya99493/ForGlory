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
        public event Action<Vector3, Direction> MoveAction;
        public event Action EndStepAction;
        public event Action<PlaygroundPresenter> EndMoveAction;
        
        private float _speed = 4f;
        private Queue<Pair<int, int>> _route;
        private int _teamEnergy;
        private CharacterPresenter[] _characters;

        public int RoutLength => _route.Count;
        public int CharactersCount => _characters.Length;
        
        public bool MoveState { get; private set; }
        public bool CanMove { get; private set; }
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }
        public int TeamEnergy { get; private set; }

        protected TeamModel(CharacterPresenter[] characters, int heightCellIndex, int widthCellIndex)
        {
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;
            MoveState = false;
            CanMove = true;

            _teamEnergy = characters[0].Model.MaxEnergy;
            _characters = new CharacterPresenter[characters.Length];
            for (int i = 0; i < characters.Length; i++)
            {
                _characters[i] = characters[i];
                if (_characters[i].Model.MaxEnergy < _teamEnergy)
                    _teamEnergy = _characters[i].Model.MaxEnergy;
                //_characters[i].DeathAction += OnKillCharacter;
            }

            _route = new Queue<Pair<int, int>>();
            
            TeamEnergy = _teamEnergy;
        }

        public CharacterPresenter GetCharacterPresenter(int index) => _characters[index];

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
                Pair<int, int> checkPoint = _route.Dequeue();
                if (playgroundPresenter.PreSetCharacterOnCell(teamPresenter, checkPoint.FirstValue, checkPoint.SecondValue) == false)
                {
                    _route.Clear();
                    break;
                }
                playgroundPresenter.RemoveCharacterFromCell(teamPresenter, HeightCellIndex, WidthCellIndex);
                TeamEnergy--;

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
            }

            SwitchMoveState();
            CanMove = true;
            EndMoveAction?.Invoke(playgroundPresenter);
            EndStepAction?.Invoke();
        }

        private void OnKillCharacter(int id)
        {
            CharacterPresenter[] newCharacters = new CharacterPresenter[_characters.Length - 1];

            for (int i = 0, k = 0; i < _characters.Length; i++)
            {
                if (_characters[i].Model.Id != id)
                {
                    newCharacters[k] = _characters[i];
                    k++;
                }
            }

            _characters = newCharacters;
        }
    }
}