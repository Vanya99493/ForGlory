using System;
using CharacterModule.PresenterPart.CharacterStates.Base;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates
{
    public class IdleCharacterState : ICharacterIdleState
    {
        public event Action StepEndedAction;
        public event Action<PlaygroundPresenter> MoveAction;
        
        public void Enter(PlaygroundPresenter playgroundPresenter)
        {
            MoveAction?.Invoke(playgroundPresenter);
            StepEndedAction?.Invoke();
        }

        public bool Update(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            return true;
        }

        public void Exit()
        {
            
        }
    }
}