using System;
using System.Collections.Generic;
using System.Collections;
using CharacterModule.PresenterPart;
using CustomClasses;
using Infrastructure.CoroutineRunnerModule;
using PlaygroundModule.PresenterPart;
using UnityEngine;

namespace CharacterModule.ModelPart
{
    public abstract class TeamModel
    {
        public event Action<Vector3> MoveAction;
        public event Action EndStepAction;
        public event Action<PlaygroundPresenter> EndMoveAction;
        
        private float _speed = 4f;
        private Queue<Pair<int, int>> _route;
        private int _teamEnergy;
        private CharacterPresenter[] _characters;

        public int RoutLength => _route.Count; 
        
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
            }

            _route = new Queue<Pair<int, int>>();
            
            TeamEnergy = _teamEnergy;
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

        public void Move(ICoroutineRunner coroutineRunner, PlaygroundPresenter playgroundPresenter, TeamPresenter playerTeam)
        {
            if (_route.Count > 0)
            {
                CanMove = false;
                playgroundPresenter.RemoveCharacterFromCell(playerTeam, HeightCellIndex, WidthCellIndex);
                coroutineRunner.StartCoroutine(MovementCoroutine(playgroundPresenter));
            }
            else
            {
                EndStepAction?.Invoke();
            }
        }

        public void SetPosition(PlaygroundPresenter playgroundPresenter)
        {
            MoveAction?.Invoke(playgroundPresenter.Model.GetCellPresenter(HeightCellIndex, WidthCellIndex).Model.MoveCellPosition);
        }

        private IEnumerator MovementCoroutine(PlaygroundPresenter playgroundPresenter)
        {
            while (_route.Count > 0 && TeamEnergy > 0)
            {
                Pair<int, int> checkPoint = _route.Dequeue();
                TeamEnergy--;
                    
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
                    ));
                    deltaTime += Time.fixedDeltaTime * _speed;
                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
                HeightCellIndex = checkPoint.FirstValue;
                WidthCellIndex = checkPoint.SecondValue;
            }

            SwitchMoveState();
            CanMove = true;
            EndMoveAction?.Invoke(playgroundPresenter);
            EndStepAction?.Invoke();
        }
    }
}