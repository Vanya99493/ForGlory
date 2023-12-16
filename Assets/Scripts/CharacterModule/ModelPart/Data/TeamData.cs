using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class TeamData
    {
        public CharacterFullData[] CharactersData;
        public int HeightCellIndex;
        public int WidthCellIndex;
    }
}