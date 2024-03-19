using System;
using System.Collections.Generic;
using CharacterModule.PresenterPart.CharacterStates.Base;
using CustomClasses;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;

namespace CharacterModule.PresenterPart.CharacterStates
{
    public class FollowCharacterState : ICharacterFollowState
    {
        public event Action StepEndedAction;
        public event Action<List<Pair<int, int>>> AddRouteAction;
        public event Action<PlaygroundPresenter> MoveAction;

        public TeamPresenter TargetTeamPresenter;
        
        public void Enter(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter, TeamPresenter targetTeamPresenter)
        {
            TargetTeamPresenter = targetTeamPresenter;
            List<Pair<int, int>> route = new List<Pair<int, int>>();
            if (teamPresenter.Model.CanMove && ServiceLocator.Instance.GetService<WideSearch>().TryBuildRoute(
                    new MoveNode(
                        teamPresenter.Model.HeightCellIndex, 
                        teamPresenter.Model.WidthCellIndex, 
                        playgroundPresenter.Model.GetCellPresenter(teamPresenter.Model.HeightCellIndex, teamPresenter.Model.WidthCellIndex).Model.CellType,
                        true
                    ),
                    new MoveNode(
                        targetTeamPresenter.Model.HeightCellIndex, 
                        targetTeamPresenter.Model.WidthCellIndex,
                        playgroundPresenter.Model.GetCellPresenter(targetTeamPresenter.Model.HeightCellIndex, targetTeamPresenter.Model.WidthCellIndex).Model.CellType,
                        playgroundPresenter.CheckCellOnCharacter(targetTeamPresenter.Model.HeightCellIndex, targetTeamPresenter.Model.WidthCellIndex)),
                    playgroundPresenter,
                    false,
                    out route
                ))
            {
                playgroundPresenter.RemoveCharacterFromCell(teamPresenter, teamPresenter.Model.HeightCellIndex, teamPresenter.Model.WidthCellIndex);
                AddRouteAction?.Invoke(route);
                if (teamPresenter.Model.RoutLength <= teamPresenter.Model.TeamEnergy)
                    targetTeamPresenter = null;
                MoveAction?.Invoke(playgroundPresenter);
            }
            
            StepEndedAction?.Invoke();
        }

        public bool Update(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            if (TargetTeamPresenter == null)
            {
                return true;
            }
            
            Enter(teamPresenter, playgroundPresenter, TargetTeamPresenter);
            return false;
        }

        public void Exit()
        {
            
        }
    }
}