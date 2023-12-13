using System;
using BattleModule.ViewPart;
using CharacterModule.ViewPart;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class GameScenePrefabsProvider
    {
        [SerializeField] private GameScenePrefabsContainer gameScenePrefabsContainer;

        public BattlegroundView GetBattlgroundView() => gameScenePrefabsContainer.BattlegroundView;
        public TeamView GetTeamView() => gameScenePrefabsContainer.TeamView;
        public CharacterView GetCharacterByName(string name) => gameScenePrefabsContainer.CharacterPrefabsMap[name];
    }
}