using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates.Base
{
    public interface ICharacterIdleState : ICharacterState
    {
        public void Enter(PlaygroundPresenter playgroundPresenter);
    }
}