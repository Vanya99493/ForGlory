using System.Collections.Generic;
using CharacterModule.ModelPart;
using Infrastructure.ServiceLocatorModule;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;
using UnityEngine;

namespace CharacterModule.PresenterPart.BehaviourModule
{
    public class EnemyBehaviour
    {
        public void StartEnemiesBehaviour(List<EnemyTeamPresenter> enemies, PlaygroundPresenter playgroundPresenter, PlayerTeamPresenter playerTeam)
        {
            foreach (EnemyTeamPresenter enemy in enemies)
            {
                List<Node> neighborhood = GetNeighborhood(enemy, playgroundPresenter);
                    
                if (CheckNeighborhood(neighborhood, playgroundPresenter, playerTeam))
                {
                    enemy.EnterFollowState(playgroundPresenter, playerTeam);
                    continue;
                }
                if (Random.Range(0, 2) == 0)
                {
                    enemy.EnterIdleState(playgroundPresenter);
                }
                else
                {
                    Node node = neighborhood[Random.Range(0, neighborhood.Count)];
                    enemy.EnterMoveState(playgroundPresenter, node.HeightIndex, node.WidthIndex);
                }
            }
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

        private bool CheckNeighborhood(List<Node> neighborhood, PlaygroundPresenter playgroundPresenter, PlayerTeamPresenter targetTeam)
        {
            foreach (Node node in neighborhood)
            {
                if (playgroundPresenter.Model.GetCellPresenter(node.HeightIndex, node.WidthIndex).Model.TeamsOnCell
                    .Contains(targetTeam))
                {
                    return true;
                }
            }

            return false;
        }
    }
}