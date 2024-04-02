using CharacterModule.ModelPart.Data;
using PlaygroundModule.ModelPart.Data;
using UnityEngine;

namespace LevelModule.Data
{
    [CreateAssetMenu(fileName = "LevelDifficultyData", menuName = "ScriptableObjects/Data/LevelDifficultyData")]
    public class LevelDifficultyData : ScriptableObject
    {
        public NewPlaygroundData PlaygroundData;
        public int CountOfEnemyTeams;
        public CharacterFullData[] CharactersInCastle;
    }
}