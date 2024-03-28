using System;
using CharacterModule.ModelPart;
using CharacterModule.ViewPart;

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

        private void OnDeath()
        {
            View.DestroyView();
            //View.HideView();
            DeathAction?.Invoke(Model.Id);
        }

        private void OnClick()
        {
            ClickedAction?.Invoke(this);
        }
    }
}