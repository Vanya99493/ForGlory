using System;
using CharacterModule.ModelPart;

namespace CharacterModule.PresenterPart
{
    public class CharacterPresenter
    {
        public event Action DestroyPlayground;
        public event Action ClickOnCharacterAction;
        
        private CharacterModel _model;

        public CharacterPresenter(CharacterModel model)
        {
            _model = model;
        }

        public void Enter<TState>()
        {
            
        }
        
        public void ClickOnCharacter()
        {
            ClickOnCharacterAction?.Invoke();
        }
        
        public void Destroy()
        {
            DestroyPlayground?.Invoke();
        }
    }
}