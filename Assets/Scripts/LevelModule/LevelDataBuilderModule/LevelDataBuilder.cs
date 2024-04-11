using System;
using System.Collections.Generic;
using CastleModule.PresenterPart;
using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.PresenterPart;
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
        /*public LevelData GetBackgroundLevelData(LevelDifficultyData inputData, GameScenePrefabsProvider gameScenePrefabsProvider, 
            CellDataProvider cellDataProvider, CharacterIdSetter charactersIdSetter, DataBaseController dbController)
        {
            
        }*/

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

            CharacterData[] charactersInCastle = new CharacterData[inputData.CharactersInCastle.Length];
            for (int i = 0; i < charactersInCastle.Length; i++)
                charactersInCastle[i] = inputData.CharactersInCastle[i].CharacterData;

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
                    TeamType = TeamType.Enemies,
                    CharactersData = new CharacterData[Random.Range(1, 4)],
                    HeightCellIndex = -1,
                    WidthCellIndex = -1
                };

                CharacterType teamType = enemyTypes[Random.Range(0, enemyTypes.Length)];
                List<CharacterData> possibleCharacters = gameScenePrefabsProvider.GetCharactersByType(teamType);
                
                for (int j = 0; j < enemyTeams[i].CharactersData.Length; j++)
                {
                    enemyTeams[i].CharactersData[j] = possibleCharacters[Random.Range(0, possibleCharacters.Count)];
                }
            }

            LevelData newLevelData = new LevelData()
            {
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
                    PlayersInCastleTeam = new TeamData()
                    {
                        TeamType = TeamType.Castle,
                        CharactersData = charactersInCastle
                    }, 
                    PlayerTeam = new TeamData()
                    {
                        TeamType = TeamType.Players,
                        CharactersData = Array.Empty<CharacterData>(),
                        HeightCellIndex = -1,
                        WidthCellIndex = -1
                    },
                    EnemyTeams = enemyTeams
                }
            };

            return newLevelData;
        }

        public LevelData BuildDBData(PlaygroundPresenter playgroundPresenter, CastlePresenter castlePresenter,
            PlayerTeamPresenter playerTeamPresenter, List<EnemyTeamPresenter> enemyTeamPresenters, CharacterIdSetter characterIdSetter)
        {
            CellType[,] playground = new CellType[playgroundPresenter.Model.PlaygroundHeight,
                playgroundPresenter.Model.PlaygroundWidth];
            for (int i = 0; i < playground.GetLength(0); i++)
                for (int j = 0; j < playground.GetLength(1); j++)
                    playground[i, j] = playgroundPresenter.Model.GetCellPresenter(i, j).Model.CellType;

            CharacterPresenter[] charactersInCastle = castlePresenter.GetCharactersInCastle();
            CharacterData[] playersInCastle = new CharacterData[charactersInCastle.Length];
            for (int i = 0; i < playersInCastle.Length; i++)
            {
                playersInCastle[i] = new CharacterData()
                {
                    Id = charactersInCastle[i].Model.Id,
                    Name = charactersInCastle[i].Model.Name,
                    CurrentHealth = charactersInCastle[i].Model.Health,
                    MaxHealth = charactersInCastle[i].Model.MaxHealth,
                    CurrentEnergy = charactersInCastle[i].Model.Energy,
                    MaxEnergy = charactersInCastle[i].Model.MaxEnergy,
                    Initiative = charactersInCastle[i].Model.Initiative,
                    Damage = charactersInCastle[i].Model.Damage,
                    PositionType = PositionType.Castle
                };
            }
            TeamData playersInCastleTeam = new TeamData()
            {
                TeamType = TeamType.Castle,
                CharactersData = playersInCastle
            };

            CharacterData[] players = new CharacterData[playerTeamPresenter.Model.GetAliveCharactersCount()];
            var characters = playerTeamPresenter.Model.GetCharacters();
            int index = 0;
            foreach (var character in characters)
            {
                players[index] = new CharacterData()
                {
                    Id = character.Model.Id,
                    Name = character.Model.Name,
                    CurrentHealth = character.Model.Health,
                    MaxHealth = character.Model.MaxHealth,
                    CurrentEnergy = character.Model.Energy,
                    MaxEnergy = character.Model.MaxEnergy,
                    Initiative = character.Model.Initiative,
                    Damage = character.Model.Damage,
                    PositionType = character.Model.PositionType
                };
                index++;
            }
            TeamData playerTeamData = new TeamData()
            {
                CharactersData = players,
                TeamType = TeamType.Players,
                HeightCellIndex = playerTeamPresenter.Model.HeightCellIndex,
                WidthCellIndex = playerTeamPresenter.Model.WidthCellIndex
            };

            TeamData[] enemiesTeamData = new TeamData[enemyTeamPresenters.Count];
            index = 0;
            foreach (var enemyTeamPresenter in enemyTeamPresenters)
            {
                CharacterData[] enemies = new CharacterData[enemyTeamPresenter.Model.GetAliveCharactersCount()];
                var enemyCharacters = enemyTeamPresenter.Model.GetCharacters();
                var index2 = 0;
                foreach (var character in enemyCharacters)
                {
                    enemies[index2] = new CharacterData()
                    {
                        Id = character.Model.Id,
                        Name = character.Model.Name,
                        CurrentHealth = character.Model.Health,
                        MaxHealth = character.Model.MaxHealth,
                        CurrentEnergy = character.Model.Energy,
                        MaxEnergy = character.Model.MaxEnergy,
                        Initiative = character.Model.Initiative,
                        Damage = character.Model.Damage,
                        Vision = ((EnemyCharacterModel)character.Model).MaxVision,
                        PositionType = character.Model.PositionType
                    };
                    index2++;
                }
                
                enemiesTeamData[index] = new TeamData()
                {
                    CharactersData = enemgities,
                    TeamType = TeamType.Enemies,
                    HeightCellIndex = enemyTeamPresenter.Model.HeightCellIndex, 
                    WidthCellIndex = enemyTeamPresenter.Model.WidthCellIndex
                };
                index++;
            }
            
            LevelData levelData = new LevelData()
            {
                GeneralData = new GeneralData()
                {
                    LastCharacterId = characterIdSetter.GetNewId()
                },
                PlaygroundData = new CreatedPlaygroundData()
                {
                    CastleHeightIndex = castlePresenter.CastleHeightIndex,
                    CastleWidthIndex = castlePresenter.CastleWidthIndex,
                    Playground = playground
                },
                TeamsData = new TeamsData()
                {
                    PlayersInCastleTeam = playersInCastleTeam,
                    PlayerTeam = playerTeamData,
                    EnemyTeams = enemiesTeamData
                }
            };

            return levelData;
        }
    }
}