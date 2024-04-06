using System;
using LevelModule.Data;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class LevelDataProvider
    {
        [SerializeField] private LevelDataContainer levelDataContainer;

        public LevelDifficultyData GetLevelDifficultyData(LevelDifficulty levelDifficulty) =>
            levelDataContainer.LevelDifficultyDatas[levelDifficulty];

        public LevelDifficultyData GetBackgroundLevelData() => levelDataContainer.BackgroundLevelData;
    }
}