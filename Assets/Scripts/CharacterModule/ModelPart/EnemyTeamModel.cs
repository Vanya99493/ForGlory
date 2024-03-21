using System.Collections.Generic;
using CharacterModule.PresenterPart;

namespace CharacterModule.ModelPart
{
    public class EnemyTeamModel : TeamModel
    {
        public int TeamVision { get; private set; }

        public EnemyTeamModel(int heightCellIndex, int widthCellIndex) : base(heightCellIndex, widthCellIndex)
        {
        }

        public void SetVision()
        {
            List<CharacterPresenter> characters = new List<CharacterPresenter>();

            if(_leftVanguard != null)
                characters.Add(_leftVanguard);
            if(_rightVanguard != null)
                characters.Add(_rightVanguard);
            if(_rearguard != null)
                characters.Add(_rearguard);

            TeamVision = ((EnemyCharacterModel)characters[0].Model).MaxVision;
            foreach (var character in characters)
            {
                if (((EnemyCharacterModel)character.Model).MaxVision > TeamVision)
                    TeamVision = ((EnemyCharacterModel)character.Model).MaxVision;
            }
        }
    }
}