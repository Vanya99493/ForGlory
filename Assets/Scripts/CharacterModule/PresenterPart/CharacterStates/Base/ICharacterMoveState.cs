using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates.Base
{
    public interface ICharacterMoveState : ICharacterState
    {
        public void Enter(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter, int heightCoordinate, int widthCoordinate);
    }
}