using PlaygroundModule.PresenterPart;

namespace CharacterModule.PresenterPart.BehaviourModule.Base
{
    public interface IBehaviour
    {
        public void Start(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter);
    }
}