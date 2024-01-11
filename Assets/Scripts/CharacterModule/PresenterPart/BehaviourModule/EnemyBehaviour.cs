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
            List<Node> neighborhood = GetNeighborhood(teamPresenter as EnemyTeamPresenter, playgroundPresenter);

            if (CheckNeighborhood(neighborhood, playgroundPresenter, out var playerTeamPresenter))
            {
                teamPresenter.EnterFollowState(playgroundPresenter, playerTeamPresenter);
                return;
            }

            if (Random.Range(0, 2) == 0)
            {
                Node node;
                for (int i = 0; i < 3; i++)
                {
                    node = neighborhood[Random.Range(0, neighborhood.Count)];

                    if (!playgroundPresenter.CheckCellOnCharacter(node.HeightIndex, node.WidthIndex) && 
                        ServiceLocator.Instance.GetService<WideSearch>().TryBuildRoute(
                            new Node(
                                teamPresenter.Model.HeightCellIndex, 
                                teamPresenter.Model.WidthCellIndex, 
                                playgroundPresenter.Model.GetCellPresenter(teamPresenter.Model.HeightCellIndex, teamPresenter.Model.WidthCellIndex).Model.CellType,
                                true),
                            node,
                            playgroundPresenter,
                            true,
                            out var route
                        ))
                    {
                        teamPresenter.EnterMoveState(playgroundPresenter, node.HeightIndex, node.WidthIndex);
                        return;
                    }
                }
            }
            teamPresenter.EnterIdleState(playgroundPresenter);
        }

        private List<Node> GetNeighborhood(EnemyTeamPresenter teamPresenter, PlaygroundPresenter playgroundPresenter)
        {
            List<Node> neighborhood = ServiceLocator.Instance.GetService<WideSearch>().GetCellsByLength(
                ((EnemyTeamModel)teamPresenter.Model).TeamVision, 
                new Node(
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

        private bool CheckNeighborhood(List<Node> neighborhood, PlaygroundPresenter playgroundPresenter, out PlayerTeamPresenter targetTeam)
        {
            foreach (Node node in neighborhood)
            {
                if (playgroundPresenter.Model.GetCellPresenter(node.HeightIndex, node.WidthIndex).Model
                    .CheckCellOnPlayer(out var playerPresenter))
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