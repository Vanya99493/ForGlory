using System.Collections.Generic;
using CharacterModule.ModelPart;
using CharacterModule.PresenterPart.BehaviourModule.Base;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
using UnityEngine;

namespace CharacterModule.PresenterPart.BehaviourModule
{
    public class EnemyBehaviour : IBehaviour
    {
        public void Start(TeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            List<MoveNode> neighborhood = GetNeighborhood(teamPresenter as EnemyTeamPresenter, playgroundPresenter);

            if (CheckNeighborhood(neighborhood, playgroundPresenter, out var playerTeamPresenter) &&
                !playgroundPresenter.Model.GetCellPresenter(playerTeamPresenter.Model.HeightCellIndex, playerTeamPresenter.Model.WidthCellIndex).
                    Model.CheckCellOnCastle())
            {
                teamPresenter.EnterFollowState(playgroundPresenter, playerTeamPresenter);
                return;
            }

            if (Random.Range(0, 2) == 0)
            {
                MoveNode moveNode;
                for (int i = 0; i < 3; i++)
                {
                    moveNode = neighborhood[Random.Range(0, neighborhood.Count)];

                    if (!playgroundPresenter.CheckCellOnCharacter(moveNode.HeightIndex, moveNode.WidthIndex) && 
                        ServiceLocator.Instance.GetService<WideSearch>().TryBuildRoute(
                            new MoveNode(
                                teamPresenter.Model.HeightCellIndex, 
                                teamPresenter.Model.WidthCellIndex, 
                                playgroundPresenter.Model.GetCellPresenter(teamPresenter.Model.HeightCellIndex, teamPresenter.Model.WidthCellIndex).Model.CellType,
                                true),
                            moveNode,
                            playgroundPresenter,
                            true,
                            out var route
                        ))
                    {
                        teamPresenter.EnterMoveState(playgroundPresenter, moveNode.HeightIndex, moveNode.WidthIndex);
                        return;
                    }
                }
            }
            teamPresenter.EnterIdleState(playgroundPresenter);
        }

        private List<MoveNode> GetNeighborhood(EnemyTeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            List<MoveNode> neighborhood = ServiceLocator.Instance.GetService<WideSearch>().GetCellsByLength(
                ((EnemyTeamModel)teamPresenter.Model).TeamVision, 
                new MoveNode(
                    teamPresenter.Model.HeightCellIndex, 
                    teamPresenter.Model.WidthCellIndex, 
                    playgroundPresenter.Model.GetCellPresenter(teamPresenter.Model.HeightCellIndex, teamPresenter.Model.WidthCellIndex).Model.CellType,
                    true
                ),
                playgroundPresenter,
                false
            );
            
            return neighborhood;
        }

        private bool CheckNeighborhood(List<MoveNode> neighborhood, PlaygroundPresenter playgroundPresenter, out PlayerTeamPresenter targetTeam)
        {
            foreach (MoveNode node in neighborhood)
            {
                if (playgroundPresenter.Model.GetCellPresenter(node.HeightIndex, node.WidthIndex).Model
                    .CheckCellOnPlayer(out var playerPresenter) && !playgroundPresenter.Model.GetCellPresenter(
                        node.HeightIndex, node.WidthIndex).Model.CheckCellOnCastle())
                {
                    targetTeam = playerPresenter;
                    return true;
                }
            }

            targetTeam = null;
            return false;
        }
    }
}