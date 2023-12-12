using System;
using CharacterModule.PresenterPart.CharacterStates.Base;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates
{
    public class FollowCharacterState : ICharacterFollowState
    {
        public event Action StateEndedAction;
        
        public void Enter(PlaygroundPresenter playgroundPresenter, CharacterPresenter characterPresenter)
        {
            
            
            StateEndedAction?.Invoke();
        }

        public void Exit()
        {
            
        }
    }
}