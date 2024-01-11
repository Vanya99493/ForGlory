using System;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart.BehaviourModule;
using CharacterModule.ViewPart;
using Infrastructure.InputHandlerModule;

namespace CharacterModule.PresenterPart
{
    public class EnemyTeamPresenter : TeamPresenter
    {
        public override event Action<TeamPresenter> ClickOnCharacterAction;
        public event Action<EnemyTeamPresenter> FollowClickAction;
        
        public EnemyTeamPresenter(TeamModel model, TeamView view, EnemyBehaviour enemyBehaviour) : base(model, view, enemyBehaviour)
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