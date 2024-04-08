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
            Dictionary<string, int> teamTypeData = _teamTypeTableController.GetCellTypes(dbName, "TeamType");

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
                    CharactersData = teamsData.PlayersInCastle
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