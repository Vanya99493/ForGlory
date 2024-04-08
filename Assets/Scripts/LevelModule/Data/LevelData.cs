using System;
using CharacterModule.ModelPart.Data;
using PlaygroundModule.ModelPart.Data;
using UnityEngine.Serialization;

namespace LevelModule.Data
{
    [Serializable]
    public class LevelData
    {
        public GeneralData GeneralData;
        public CreatedPlaygroundData PlaygroundData;
        public TeamsData TeamsData;
    }
}