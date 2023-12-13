using CharacterModule.PresenterPart;

namespace BattleModule.ModelPart
{
    public class BattlegroundModel
    {
        public PlayerTeamPresenter PlayerTeam;
        public EnemyTeamPresenter EnemyTeam;

        public void SetTeams(PlayerTeamPresenter playersTeam, EnemyTeamPresenter enemiesTeam)
        {
            PlayerTeam = playersTeam;
            EnemyTeam = enemiesTeam;
        }
    }
}