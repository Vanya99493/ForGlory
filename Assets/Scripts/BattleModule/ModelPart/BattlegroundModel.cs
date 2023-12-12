using CharacterModule.PresenterPart;

namespace BattleModule.ModelPart
{
    public class BattlegroundModel
    {
        private PlayerCharacterPresenter[] _playersTeam;
        private EnemyCharacterPresenter[] _enemiesTeam;

        public BattlegroundModel(PlayerCharacterPresenter[] playersTeam, EnemyCharacterPresenter[] enemiesTeam)
        {
            _playersTeam = playersTeam;
            _enemiesTeam = enemiesTeam;
        }
    }
}