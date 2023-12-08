using CharacterModule.PresenterPart;

namespace CharacterModule.ModelPart
{
    public class PlayerTeamModel : TeamModel
    {
        public PlayerTeamModel(int heightCellIndex, int widthCellIndex, CharacterPresenter[] players) : base(players, heightCellIndex, widthCellIndex)
        {
        }
    }
}