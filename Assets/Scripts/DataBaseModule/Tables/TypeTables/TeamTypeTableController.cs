using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBaseModule.Tables.TypeTables.Base;
using CharacterModule.ModelPart.Data;
using Mono.Data.Sqlite;

namespace DataBaseModule.Tables.TypeTables
{
    public class TeamTypeTableController : BaseTypeTableController
    {
        private readonly List<TeamType> _teamTypes = new ()
        {
            TeamType.Enemies,
            TeamType.Castle,
            TeamType.Players
        };

        public Dictionary<string, TeamType> GetTeamTypes(string dbName)
        {
            string commandText = $"SELECT * FROM TeamType;";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

            TeamType[] teamTypes = _teamTypes.ToArray();
            Dictionary<string, TeamType> data = new Dictionary<string, TeamType>();
            int index = 0;
            while (dataReader.Read())
            {
                data.Add(dataReader["type"].ToString(), teamTypes[index]);
                index++;
            }
            
            dataReader.Close();
            connection.Close();

            return data;
        }

        protected override void FillTable(string dbName)
        {
            string commandText = $"DELETE FROM CellType;";
            ExecuteCommand(dbName, commandText);
            foreach (var teamType in _teamTypes)
            {
                commandText = 
                    $"INSERT INTO TeamType (type) " +
                    $"VALUES ('{teamType.ToString().Split('.').Last().ToLower()}');";
                ExecuteCommand(dbName, commandText);
            }
        }
    }
}