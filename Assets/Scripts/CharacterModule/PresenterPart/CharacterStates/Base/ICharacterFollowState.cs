using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.CharacterStates.Base
{
    public interface ICharacterFollowState : ICharacterState
    {
        public void Enter(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter, TeamPresenter targetTeamPresenter);
    }
}