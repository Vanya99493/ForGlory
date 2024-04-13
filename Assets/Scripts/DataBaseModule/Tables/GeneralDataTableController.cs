using System;
using System.Data;
using DataBaseModule.Tables.Base;
using Mono.Data.Sqlite;
using LevelModule.Data;

namespace DataBaseModule.Tables
{
    public class GeneralDataTableController : BaseTableController
    {
        public GeneralDataTableController(string dbName)
        {
            CreateTableIfNotExists(dbName);
        }
        
        public void AddGeneralData(string dbName, int levelId, GeneralData generalData)
        {
            string commandText =
                $"INSERT INTO GeneralData (level_id, last_character_id) " +
                $"VALUES ('{levelId}', '{generalData.LastCharacterId}')";
            ExecuteCommand(dbName, commandText);
        }

        public GeneralData GetGeneralData(string dbName, int levelId)
        {
            string commandText =
                $"SELECT * FROM GeneralData " +
                $"WHERE level_id = {levelId};";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

            GeneralData generalData = new GeneralData()
            {
                LastCharacterId = Convert.ToInt32(dataReader["last_character_id"])
            };
            
            dataReader.Close();
            connection.Close();

            return generalData;
        }

        protected override void CreateTableIfNotExists(string dbName)
        {
            string commandText =
                $"CREATE TABLE IF NOT EXISTS GeneralData (" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"level_id INTEGER REFERENCES Levels (id), " +
                $"last_character_id INTEGER" +
                $");";
            ExecuteCommand(dbName, commandText);
        }
    }
}