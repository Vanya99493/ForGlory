using System;
using Infrastructure.CoroutineRunnerModule;
using Infrastructure.InputHandlerModule;
using Interfaces;
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
        
        private void Update()
        {
            if (_canMove)
            {
                transform.position = _newPosition;
                _canMove = false;
            }
        }

        public void Move(Vector3 newPosition)
        {
            _newPosition = newPosition;
            _canMove = true;
        }

        public void Rotate(Vector3 newRotation)
        {
            transform.eulerAngles = newRotation;
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