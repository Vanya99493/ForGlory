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

        private CharacterPresenter _presenter;

        public void Inititalize(CharacterPresenter presenter)
        {
            _presenter = presenter;
            _presenter.DestroyCharacter += OnDestroyCharacter;
        }

        public void Move(Vector3 newPosition)
        {
            transform.position = newPosition;
        }

        public void Click(InputMouseButtonType mouseButtonType)
        {
            _presenter.ClickOnCharacter(mouseButtonType);
        }

        private void OnDestroyCharacter()
        {
            Destroy?.Invoke(this);
        }
    }
}