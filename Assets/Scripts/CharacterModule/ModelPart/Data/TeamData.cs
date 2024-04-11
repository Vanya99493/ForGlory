using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class TeamData
    {
        public int DBTeamId;
        public CharacterData[] CharactersData;
        public TeamType TeamType;
        public int HeightCellIndex;
        public int WidthCellIndex;
    }
}