using CharacterModule.PresenterPart.BehaviourModule.Base;
using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.BehaviourModule
{
    public class PlayerBehaviour : IBehaviour
    {
        public void Start(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            if (teamPresenter.UpdateCurrentState(playgroundPresenter))
            {
                teamPresenter.EnterIdleState(playgroundPresenter);
            }
        }
    }
}