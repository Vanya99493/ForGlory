using System;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart.BehaviourModule;
using CharacterModule.ViewPart;
using Infrastructure.InputHandlerModule;

namespace CharacterModule.PresenterPart
{
    public class PlayerTeamPresenter : TeamPresenter
    {
        public override event Action<TeamPresenter> ClickOnCharacterAction;
        
        public PlayerTeamPresenter(TeamModel model, TeamView view, PlayerBehaviour playerBehaviour) : base(model, view, playerBehaviour)
        {
        }

        public override void ClickOnTeam(InputMouseButtonType mouseButtonType)
        {
            if (mouseButtonType == InputMouseButtonType.LeftMouseButton && Model.CanMove)
            {
                Model.SwitchMoveState();
                ClickOnCharacterAction?.Invoke(this);
            }
        }
    }
}