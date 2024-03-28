using System;
using CustomClasses;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using Interfaces;
using PlaygroundModule.ModelPart;
using UnityEngine;

namespace CharacterModule.ViewPart
{
    public class TeamView : MonoBehaviour, IClickable, ICoroutineRunner
    {        
        public event Action<TeamView> Destroy;
        public event Action<InputMouseButtonType> ClickOnCharacter;

        public Transform[] characterPositions;
        
        private Vector3 _newPosition;
        private bool _canMove = false;
        private Direction _direction;
        
        private void Update()
        {
            if (_canMove)
            {
                Rotate(_direction);
                transform.position = _newPosition;
                _canMove = false;
            }
        }

        public void Move(Vector3 newPosition, Direction direction)
        {
            _newPosition = newPosition;
            _canMove = true;
            _direction = direction;
        }

        public void Rotate(Direction direction)
        {
            _direction = direction;
            transform.eulerAngles = DirectionEulerAngels.GetDirection(_direction);
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Click(InputMouseButtonType mouseButtonType)
        {
            ClickOnCharacter?.Invoke(mouseButtonType);
        }

        public void OnDestroyCharacter()
        {
            Destroy?.Invoke(this);
        }
    }
}