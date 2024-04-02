using CharacterModule.PresenterPart;
using UnityEngine;

namespace CastleModule.ModelPart
{
    public class CastleModel
    {
        public PlayerCharacterPresenter[] CharactersInCastle;
        
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }

        public CastleModel(int heightCellIndex, int widthCellIndex)
        {
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;
        }

        public void SetCharactersInCastle(PlayerCharacterPresenter[] characters, Transform parent)
        {
            CharactersInCastle = new PlayerCharacterPresenter[characters.Length];
            for (int i = 0; i < CharactersInCastle.Length; i++)
            {
                CharactersInCastle[i] = characters[i];
                CharactersInCastle[i].View.HideView();
                CharactersInCastle[i].View.transform.SetParent(parent);
            }
        }

        public void ResetHeroesEnergy()
        {
            for (int i = 0; i < CharactersInCastle.Length; i++)
            {
                CharactersInCastle[i].Model.ResetEnergy();
            }
        }
    }
}