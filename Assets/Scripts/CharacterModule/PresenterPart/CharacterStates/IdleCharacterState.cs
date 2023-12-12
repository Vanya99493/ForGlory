using System;
using CharacterModule.PresenterPart.CharacterStates.Base;

namespace CharacterModule.PresenterPart.CharacterStates
{
    public class IdleCharacterState : ICharacterIdleState
    {
        public event Action StateEndedAction;
        
        public void Enter()
        {
            StateEndedAction?.Invoke();
        }

        public void Exit()
        {
            
        }
    }
}