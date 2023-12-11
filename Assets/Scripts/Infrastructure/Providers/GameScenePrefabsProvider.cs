using System;
using CharacterModule.ViewPart;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class GameScenePrefabsProvider
    {
        [SerializeField] private GameScenePrefabsContainer gameScenePrefabsContainer;

        public TeamView GetTeamView() => gameScenePrefabsContainer.TeamView;
        public CharacterView GetCharacterByName(string name) => gameScenePrefabsContainer.CharacterPrefabsMap[name];
    }
}