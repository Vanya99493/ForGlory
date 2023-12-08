using System;
using CharacterModule.ModelPart;
using CharacterModule.ViewPart;
using Infrastructure.InputHandlerModule;

namespace CharacterModule.PresenterPart
{
    public class EnemyTeamPresenter : TeamPresenter
    {
        public override event Action<bool, int> ClickOnCharacterAction;
        
        public EnemyTeamPresenter(TeamModel model, TeamView view) : base(model, view)
        {
        }

        public override void ClickOnTeam(InputMouseButtonType mouseButtonType)
        {
            if (mouseButtonType == InputMouseButtonType.LeftMouseButton)
            {
                ClickOnCharacterAction?.Invoke(Model.MoveState, Model.Energy);
            }
        }
    }
}