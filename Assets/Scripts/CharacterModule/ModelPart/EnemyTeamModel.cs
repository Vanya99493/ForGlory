using CharacterModule.PresenterPart;

namespace CharacterModule.ModelPart
{
    public class EnemyTeamModel : TeamModel
    {
        public int TeamVision { get; private set; }
        
        public EnemyTeamModel(int heightCellIndex, int widthCellIndex, CharacterPresenter[] enemies) : base(enemies, heightCellIndex, widthCellIndex)
        {
            TeamVision = ((EnemyCharacterModel)enemies[0].Model).MaxVision;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (((EnemyCharacterModel)enemies[i].Model).MaxVision > TeamVision)
                    TeamVision = ((EnemyCharacterModel)enemies[0].Model).MaxVision;
            }
        }
    }
}