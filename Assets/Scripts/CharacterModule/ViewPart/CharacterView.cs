using System;
using CharacterModule.PresenterPart;
using Infrastructure.InputHandlerModule;
using Interfaces;
using UnityEngine;

namespace CharacterModule.ViewPart
{
    public class CharacterView : MonoBehaviour, IClickable
    {
        public event Action<CharacterView> Destroy;
        public event Action<InputMouseButtonType> ClickOnCharacter;

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