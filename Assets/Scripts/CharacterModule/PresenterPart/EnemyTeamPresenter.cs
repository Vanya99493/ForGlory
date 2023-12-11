using System;
using CharacterModule.ModelPart;
using CharacterModule.ViewPart;
using Infrastructure.InputHandlerModule;

namespace CharacterModule.PresenterPart
{
    public class EnemyTeamPresenter : TeamPresenter
    {
        public override event Action<TeamPresenter> ClickOnCharacterAction;
        public event Action<EnemyTeamPresenter> FollowClickAction;
        
        public EnemyTeamPresenter(TeamModel model, TeamView view) : base(model, view)
        {
        }

        public override void ClickOnTeam(InputMouseButtonType mouseButtonType)
        {
            if (mouseButtonType == InputMouseButtonType.LeftMouseButton)
            {
                Model.SwitchMoveState();
                ClickOnCharacterAction?.Invoke(this);
            }
            else if (mouseButtonType == InputMouseButtonType.RightMouseButton)
            {
                FollowClickAction?.Invoke(this);
            }
        }
    }
}