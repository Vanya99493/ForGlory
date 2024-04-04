using System;
using CharacterModule.ModelPart;
using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public abstract class CharacterPresenter
    {
        public event Action<CharacterPresenter> ClickedAction;
        public event Action<int> DeathAction;
        
        public readonly CharacterModel Model;
        public readonly CharacterView View;

        protected CharacterPresenter(CharacterModel model, CharacterView view)
        {
            Model = model;
            View = view;
            Model.Death += OnDeath;
            View.ClickedAction += OnClick;
        }

        public void MoveCharacter(Vector3 targetPosition, float movementTime)
        {
            View.MoveCharacter(targetPosition, movementTime);
        }

        private void OnDeath()
        {
            View.DestroyView();
            DeathAction?.Invoke(Model.Id);
        }

        private void OnClick()
        {
            ClickedAction?.Invoke(this);
        }
    }
}