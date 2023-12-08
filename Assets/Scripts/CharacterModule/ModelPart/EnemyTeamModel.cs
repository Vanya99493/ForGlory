using CharacterModule.PresenterPart;

namespace CharacterModule.ModelPart
{
    public class EnemyTeamModel : TeamModel
    {
        public EnemyTeamModel(int heightCellIndex, int widthCellIndex, CharacterPresenter[] enemies) : base(enemies, heightCellIndex, widthCellIndex)
        {
        }
    }
}