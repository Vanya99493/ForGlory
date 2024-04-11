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
    public class TeamTableController : BaseTableController
    {
        private TeamTypeTableController _teamTypeTableController;

        public TeamTableController(TeamTypeTableController teamTypeTableController)
        {
            _teamTypeTableController = teamTypeTableController;
        }
        
        public Dictionary<int, TeamData> AddTeamsData(string dbName, int levelId, TeamsData teamsData)
        {
            Dictionary<int, TeamData> returnedData = new Dictionary<int, TeamData>();
            Dictionary<string, int> teamTypeData = _teamTypeTableController.GetStringTypes(dbName, "TeamType");

            string commandText =
                $"INSERT INTO Teams (level_id, team_type_id, height_index, width_index) " +
                $"VALUES ('{levelId}', " +
                $"'{teamTypeData[TeamType.Castle.ToString().Split('.').Last().ToLower()]}', " +
                $"'{-1}', '{-1}');";
            ExecuteCommand(dbName, commandText);
            returnedData.Add(
                GetLastTeamId(dbName),
                new TeamData()
                {
                    CharactersData = teamsData.PlayersInCastleTeam.CharactersData
                }
            );
            
            commandText =
                $"INSERT INTO Teams (level_id, team_type_id, height_index, width_index) " +
                $"VALUES ('{levelId}', " +
                $"'{teamTypeData[teamsData.PlayerTeam.TeamType.ToString().Split('.').Last().ToLower()]}', " +
                $"'{teamsData.PlayerTeam.HeightCellIndex}', '{teamsData.PlayerTeam.WidthCellIndex}')";
            ExecuteCommand(dbName, commandText);
            returnedData.Add(
                GetLastTeamId(dbName),
                teamsData.PlayerTeam
            );

            for (int i = 0; i < teamsData.EnemyTeams.Length; i++)
            {
                commandText =
                    $"INSERT INTO Teams (level_id, team_type_id, height_index, width_index) " +
                    $"VALUES ('{levelId}', " +
                    $"'{teamTypeData[teamsData.EnemyTeams[i].TeamType.ToString().Split('.').Last().ToLower()]}', " +
                    $"'{teamsData.EnemyTeams[i].HeightCellIndex}', '{teamsData.EnemyTeams[i].WidthCellIndex}')";
                ExecuteCommand(dbName, commandText);
                returnedData.Add(
                    GetLastTeamId(dbName),
                    teamsData.EnemyTeams[i]
                );
            }

            return returnedData;
        }

        public TeamsData GetTeamsData(string dbName, int levelId)
        {
            Dictionary<TeamType, List<TeamData>> data = new Dictionary<TeamType, List<TeamData>>();
            Dictionary<string, TeamType> teamTypes = _teamTypeTableController.GetTeamTypes(dbName);
            Dictionary<string, int> teamStringTypes = _teamTypeTableController.GetStringTypes(dbName, "TeamType");
            
            foreach (var teamType in teamTypes)
                data.Add(teamType.Value, new List<TeamData>());

            foreach (var teamStringType in teamStringTypes)
            {
                string commandText =
                    $"SELECT * FROM Teams " +
                    $"WHERE level_id = {levelId} AND team_type_id = {teamStringType.Value};";

                (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

                TeamType currentTeamsType = teamTypes[teamStringType.Key];
                
                while (dataReader.Read())
                {
                    data[currentTeamsType].Add(new TeamData()
                        {
                            DBTeamId = Convert.ToInt32(dataReader["id"].ToString()),
                            TeamType = currentTeamsType,
                            HeightCellIndex = Convert.ToInt32(dataReader["height_index"].ToString()),
                            WidthCellIndex = Convert.ToInt32(dataReader["width_index"].ToString())
                        }
                    );
                }
            
                dataReader.Close();
                connection.Close();
            }

            if (data[TeamType.Castle].Count != 1 ||
                data[TeamType.Players].Count != 1)
                throw new Exception("Invalid data base teams data");

            TeamsData teamsData = new TeamsData()
            {
                PlayersInCastleTeam = data[TeamType.Castle][0],
                PlayerTeam = data[TeamType.Players][0],
                EnemyTeams = data[TeamType.Enemies].ToArray()
            };

            return teamsData;
        }

        private int GetLastTeamId(string dbName)
        {
            string commandName =
                $"SELECT * FROM Teams ORDER BY id DESC;";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandName);
            int lastTeamId = Convert.ToInt32(dataReader["id"]);
            dataReader.Close();
            connection.Close();

            return lastTeamId;
        }
    }
}