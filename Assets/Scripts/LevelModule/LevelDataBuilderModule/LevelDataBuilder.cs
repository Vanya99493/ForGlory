using System.Collections.Generic;
using CharacterModule.ModelPart.Data;
using Infrastructure.Providers;
using LevelModule.Data;
using UnityEngine;

namespace LevelModule.LevelDataBuilderModule
{
    public class LevelDataBuilder
    {
        public LevelData GetBackgroundLevelData()
        {
            return null;
        }
        
        public LevelData BuildLevelData(LevelData inputData, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            for (int i = 0; i < inputData.TeamsData.PlayerTeam.CharactersData.Length; i++)
            {
                inputData.TeamsData.PlayerTeam.CharactersData[i] = gameScenePrefabsProvider.GetCharacterByName(
                    inputData.TeamsData.PlayerTeam.CharactersData[i].CharacterData.Name);
            }

            CharacterType[] EnemyTypes = 
            {
                CharacterType.Arthropods,
                CharacterType.Slimes,
                CharacterType.Skeletons
            };
            
            for (int i = 0; i < inputData.TeamsData.EnemyTeams.Length; i++)
            {
                inputData.TeamsData.EnemyTeams[i].CharactersData = new CharacterFullData[Random.Range(1, 4)];
                
                CharacterType teamType = EnemyTypes[Random.Range(0, EnemyTypes.Length)];
                List<CharacterFullData> possibleCharacters = gameScenePrefabsProvider.GetCharactersByType(teamType);
                
                for (int j = 0; j < inputData.TeamsData.EnemyTeams[i].CharactersData.Length; j++)
                {
                    inputData.TeamsData.EnemyTeams[i].CharactersData[j] = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                }
            }

            return inputData;
        }

        public LevelData SetPrefabs(LevelData inputData, GameScenePrefabsProvider gameScenePrefabsProvider)
        {
            for (int i = 0; i < inputData.TeamsData.PlayerTeam.CharactersData.Length; i++)
            {
                inputData.TeamsData.PlayerTeam.CharactersData[i].CharacterPrefab = gameScenePrefabsProvider.GetCharacterByName(
                    inputData.TeamsData.PlayerTeam.CharactersData[i].CharacterData.Name).CharacterPrefab;
            }
            
            for (int i = 0; i < inputData.TeamsData.EnemyTeams.Length; i++)
            {
                for (int j = 0; j < inputData.TeamsData.EnemyTeams[i].CharactersData.Length; j++)
                {
                    inputData.TeamsData.EnemyTeams[i].CharactersData[j].CharacterPrefab = gameScenePrefabsProvider.GetCharacterByName(
                        inputData.TeamsData.EnemyTeams[i].CharactersData[j].CharacterData.Name).CharacterPrefab;
                }
            }

            return inputData;
        }
    }
}