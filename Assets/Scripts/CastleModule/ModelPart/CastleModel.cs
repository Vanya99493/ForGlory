using CharacterModule.PresenterPart;

namespace CastleModule.ModelPart
{
    public class CastleModel
    {
        private PlayerCharacterPresenter[] _charactersInCastle;
        
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }

        public CastleModel(int heightCellIndex, int widthCellIndex, PlayerCharacterPresenter[] characters)
        {
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;

            _charactersInCastle = new PlayerCharacterPresenter[characters.Length];
            for (int i = 0; i < _charactersInCastle.Length; i++)
            {
                _charactersInCastle[i] = characters[i];
            }
        }
    }
}