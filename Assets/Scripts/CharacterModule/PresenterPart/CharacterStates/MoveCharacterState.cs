using System;
using System.Collections.Generic;
using CharacterModule.PresenterPart.CharacterStates.Base;
using CustomClasses;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;

namespace CharacterModule.PresenterPart.CharacterStates
{
    public class MoveCharacterState : ICharacterMoveState
    {
        public event Action StepEndedAction;
        public event Action<List<Pair<int, int>>> AddRouteAction;
        public event Action<PlaygroundPresenter> MoveAction;

        public void Enter(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter, int heightCoordinate, int widthCoordinate)
        {
            List<Pair<int, int>> route = new List<Pair<int, int>>();
            if (teamPresenter.Model.CanMove && ServiceLocator.Instance.GetService<WideSearch>().TryBuildRoute(
                    new Node(
                        teamPresenter.Model.HeightCellIndex, 
                        teamPresenter.Model.WidthCellIndex, 
                        playgroundPresenter.Model.GetCellPresenter(teamPresenter.Model.HeightCellIndex, teamPresenter.Model.WidthCellIndex).Model.CellType,
                        true
                    ),
                    new Node(heightCoordinate, widthCoordinate,
                        playgroundPresenter.Model.GetCellPresenter(heightCoordinate, widthCoordinate).Model.CellType,
                        playgroundPresenter.CheckCellOnCharacter(heightCoordinate, widthCoordinate)),
                    playgroundPresenter,
                    true,
                    out route
                ))
            {
                AddRouteAction?.Invoke(route);
                MoveAction?.Invoke(playgroundPresenter);
            }
            
            StepEndedAction?.Invoke();
        }

        public bool Update(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            if (teamPresenter.Model.RoutLength > 0)
            {
                MoveAction?.Invoke(playgroundPresenter);
                StepEndedAction?.Invoke();
                return false;
            }
            
            return true;
        }

        public void Exit()
        {
            
        }
    }
}