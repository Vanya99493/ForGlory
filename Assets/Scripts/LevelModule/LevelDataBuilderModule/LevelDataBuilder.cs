using System;
using System.Collections.Generic;
using CharacterModule.ModelPart.Data;
using DataBaseModule;
using Infrastructure.Providers;
using Infrastructure.Services;
using LevelModule.Data;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ModelPart.Data;
using PlaygroundModule.PresenterPart;
using Random = UnityEngine.Random;

namespace LevelModule.LevelDataBuilderModule
{
    public class LevelDataBuilder
    {
        public LevelData GetBackgroundLevelData()
        {
            return null;
        }

        public LevelData BuildNewLevelData(LevelDifficultyData inputData, GameScenePrefabsProvider gameScenePrefabsProvider, 
            CellDataProvider cellDataProvider, CharacterIdSetter charactersIdSetter, DataBaseController dbController)
        {
            CellType[,] playground = new PlaygroundCreator().CreatePlaygroundByNewRootSpawnSystem(
                cellDataProvider,
                inputData.PlaygroundData.Height,
                inputData.PlaygroundData.Width,
                inputData.PlaygroundData.LengthOfWaterLine,
                inputData.PlaygroundData.LengthOfCoast
                );

            TeamData[] enemyTeams = new TeamData[inputData.CountOfEnemyTeams];

            CharacterType[] enemyTypes = 
            {
                CharacterType.Arthropods,
                CharacterType.Slimes,
                CharacterType.Skeletons
            };
            
            for (int i = 0; i < enemyTeams.Length; i++)
            {
                enemyTeams[i] = new TeamData
                {
                    CharactersData = new CharacterFullData[Random.Range(1, 4)],
                    HeightCellIndex = -1,
                    WidthCellIndex = -1
                };

                CharacterType teamType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                List<CharacterFullData> possibleCharacters = gameScenePrefabsProvider.GetCharactersByType(teamType);
                
                for (int j = 0; j < enemyTeams[i].CharactersData.Length; j++)
                {
                    enemyTeams[i].CharactersData[j] = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                }
            }

            LevelData newLevelData = new LevelData()
            {
                LevelId = dbController.GetLastLevelId(),
                GeneralData = new GeneralData()
                {
                    LastCharacterId = charactersIdSetter.GetNewId()
                },
                PlaygroundData = new CreatedPlaygroundData()
                {
                    CastleHeightIndex = -1,
                    CastleWidthIndex = -1,
                    Playground = playground
                },
                TeamsData = new TeamsData()
                {
                    PlayersInCastle = inputData.CharactersInCastle,
                    PlayerTeam = new TeamData()
                    {
                        CharactersData = Array.Empty<CharacterFullData>(),
                        HeightCellIndex = -1,
                        WidthCellIndex = -1
                    },
                    EnemyTeams = enemyTeams
                }
            };

            return newLevelData;
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