using System;

namespace CharacterModule.PresenterPart.CharacterStates.Base
{
    public interface ICharacterState
    {
        public event Action StateEndedAction;
        
        public void Exit();
    }
}