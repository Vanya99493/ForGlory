using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.BehaviourModule
{
    public class PlayerBehaviour
    {
        public void StartPlayerBehaviour(PlayerTeamPresenter playerTeam, PlaygroundPresenter playgroundPresenter)
        {
            if (playerTeam.UpdateCurrentState(playgroundPresenter))
            {
                playerTeam.EnterIdleState(playgroundPresenter);
            }
        }
    }
}