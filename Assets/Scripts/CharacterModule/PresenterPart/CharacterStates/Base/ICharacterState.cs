using System;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates.Base
{
    public interface ICharacterState
    {
        public event Action StepEndedAction;

        public bool Update(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter);
        public void Exit();
    }
}