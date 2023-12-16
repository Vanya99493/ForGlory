using System;
using CharacterModule.ModelPart.Data;
using PlaygroundModule.ModelPart.Data;

namespace LevelModule.Data
{
    [Serializable]
    public class LevelData
    {
        public GeneralData GeneralData;
        public PlaygroundData PlaygroundData;
        public TeamsData TeamsData;
    }
}