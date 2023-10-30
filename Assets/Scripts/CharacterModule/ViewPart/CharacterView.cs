using System;
using CharacterModule.PresenterPart;
using Inrefaces;
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
            _presenter.DestroyPlayground += OnDestroyPlayground;
        }
        
        public void Click()
        {
            _presenter.ClickOnCharacter();
        }

        private void OnDestroyPlayground()
        {
            Destroy?.Invoke(this);
        }
    }
}