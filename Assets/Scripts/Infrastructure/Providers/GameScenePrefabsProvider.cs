using System;
using System.Collections.Generic;
using BattleModule.ViewPart;
using CharacterModule.ModelPart.Data;
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
        public CharacterFullData GetCharacterByName(string name)
        {
            foreach (CharacterFullData characterFullData in gameScenePrefabsContainer.Characters)
            {
                if (characterFullData.CharacterData.Name == name)
                {
                    return characterFullData;
                }
            }

            throw new Exception($"Cannot find character with Name: {name}");
        }

        public List<CharacterFullData> GetCharactersByType(CharacterType type)
        {
            List<CharacterFullData> characters = new List<CharacterFullData>();
            foreach (CharacterFullData characterFullData in gameScenePrefabsContainer.Characters)
            {
                if (characterFullData.CharacterData.CharacterType == type)
                {
                    characters.Add(characterFullData);
                }
            }

            if (characters.Count > 0)
                return characters;
            
            throw new Exception($"Cannot find any character with CharacterType: {type}");
        }
    }
}