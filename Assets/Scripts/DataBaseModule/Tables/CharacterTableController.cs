using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CharacterModule.ModelPart.Data;
using DataBaseModule.Tables.Base;
using DataBaseModule.Tables.TypeTables;
using Mono.Data.Sqlite;

namespace DataBaseModule.Tables
{
    public class CharacterTableController : BaseTableController
    {
        private CharacterPositionTypeTableController _characterPositionTypeTableController;

        public CharacterTableController(string dbName, CharacterPositionTypeTableController characterPositionTypeTableController)
        {
            _characterPositionTypeTableController = characterPositionTypeTableController;
            CreateTableIfNotExists(dbName);
        }

        public void AddCharacterData(string dbName, Dictionary<int, TeamData> teams)
        {
            Dictionary<string, int> characterPositionTypeData = _characterPositionTypeTableController.
                GetStringTypes(dbName, "CharacterPositionType");
            foreach (var team in teams)
            {
                for (int i = 0; i < team.Value.CharactersData.Length; i++)
                {
                    string commandText =
                        $"INSERT INTO Characters (team_id, character_position_type_id, name, character_id, " +
                        $"current_health, max_health, current_energy, max_energy, initiative, damage, vision) " +
                        $"VALUES ('{team.Key}', " +
                        $"'{characterPositionTypeData[team.Value.CharactersData[i].PositionType.ToString().Split('.').Last().ToLower()]}', " +
                        $"'{team.Value.CharactersData[i].Name}', '{team.Value.CharactersData[i].Id}', " +
                        $"'{team.Value.CharactersData[i].CurrentHealth}', '{team.Value.CharactersData[i].MaxHealth}', " +
                        $"'{team.Value.CharactersData[i].CurrentEnergy}', '{team.Value.CharactersData[i].MaxEnergy}', " +
                        $"'{team.Value.CharactersData[i].Initiative}', '{team.Value.CharactersData[i].Damage}', " +
                        $"'{team.Value.CharactersData[i].Vision}')";
                    
                    ExecuteCommand(dbName, commandText);
                }
            }
        }

        public void GetCharactersData(string dbName, ref TeamsData teamsData)
        {
            List<TeamData> teams = teamsData.EnemyTeams.ToList();
            teams.Add(teamsData.PlayersInCastleTeam);
            teams.Add(teamsData.PlayerTeam);

            Dictionary<string, PositionType> characterPositions =
                _characterPositionTypeTableController.GetPositionTypes(dbName);
            Dictionary<int, string> characterStringPositions =
                _characterPositionTypeTableController.GetStringTypesReverse(dbName, "CharacterPositionType");

            foreach (var team in teams)
            {
                string commandText =
                    $"SELECT * FROM Characters " +
                    $"WHERE team_id = {team.DBTeamId};";

                List<CharacterData> characters = new List<CharacterData>();

                (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

                while (dataReader.Read())
                {
                    characters.Add(new CharacterData()
                    {
                        Id = Convert.ToInt32(dataReader["character_id"].ToString()),
                        Name = dataReader["name"].ToString(),
                        CurrentHealth = Convert.ToInt32(dataReader["current_health"].ToString()),
                        MaxHealth = Convert.ToInt32(dataReader["max_health"].ToString()),
                        CurrentEnergy = Convert.ToInt32(dataReader["current_energy"].ToString()),
                        MaxEnergy = Convert.ToInt32(dataReader["max_energy"].ToString()),
                        Initiative = Convert.ToInt32(dataReader["initiative"].ToString()),
                        Damage = Convert.ToInt32(dataReader["damage"].ToString()),
                        Vision = Convert.ToInt32(dataReader["vision"].ToString()),
                        PositionType =
                            characterPositions[
                                characterStringPositions[
                                    Convert.ToInt32(dataReader["character_position_type_id"].ToString())]]
                    });
                }

                team.CharactersData = characters.ToArray();
                
                dataReader.Close();
                connection.Close();
            }
        }

        protected override void CreateTableIfNotExists(string dbName)
        {
            string commandText =
                $"CREATE TABLE IF NOT EXISTS Characters (" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"team_id INTEGER REFERENCES Teams (id), " +
                $"character_position_type_id INTEGER REFERENCES CharacterPositionType (id), " +
                $"name TEXT, " +
                $"character_id INTEGER, " +
                $"current_health INTEGER, " +
                $"max_health INTEGER, " +
                $"current_energy INTEGER, " +
                $"max_energy INTEGER, " +
                $"initiative INTEGER, " +
                $"damage INTEGER, " +
                $"vision INTEGER" +
                $");";
            ExecuteCommand(dbName, commandText);
        }
    }
}