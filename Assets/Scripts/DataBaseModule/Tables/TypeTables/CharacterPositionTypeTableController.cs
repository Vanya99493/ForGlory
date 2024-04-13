using System.Collections.Generic;
using System.Data;
using System.Linq;
using CharacterModule.ModelPart.Data;
using DataBaseModule.Tables.TypeTables.Base;
using Mono.Data.Sqlite;

namespace DataBaseModule.Tables.TypeTables
{
    public class CharacterPositionTypeTableController : BaseTypeTableController
    {
        private readonly List<PositionType> _positionTypes = new()
        {
            PositionType.Castle,
            PositionType.LeftVanguard,
            PositionType.RightVanguard,
            PositionType.Rearguard
        };

        public CharacterPositionTypeTableController(string dbName)
        {
            CreateTableIfNotExists(dbName);
        }

        public Dictionary<string, PositionType> GetPositionTypes(string dbName)
        {
            string commandText = $"SELECT * FROM CharacterPositionType;";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

            PositionType[] positionTypes = _positionTypes.ToArray();
            Dictionary<string, PositionType> data = new Dictionary<string, PositionType>();
            int index = 0;
            while (dataReader.Read())
            {
                data.Add(dataReader["type"].ToString(), positionTypes[index]);
                index++;
            }
            
            dataReader.Close();
            connection.Close();

            return data;
        }
        
        protected override void FillTable(string dbName)
        {
            string commandText = $"DELETE FROM CharacterPositionType;";
            ExecuteCommand(dbName, commandText);
            foreach (var positionType in _positionTypes)
            {
                commandText = 
                    $"INSERT INTO CharacterPositionType (type) " +
                    $"VALUES ('{positionType.ToString().Split('.').Last().ToLower()}');";
                ExecuteCommand(dbName, commandText);
            }
        }

        protected override void CreateTableIfNotExists(string dbName)
        {
            string commandText =
                $"CREATE TABLE IF NOT EXISTS CharacterPositionType (" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"type TEXT UNIQUE" +
                $");";
            ExecuteCommand(dbName, commandText);
        }
    }
}