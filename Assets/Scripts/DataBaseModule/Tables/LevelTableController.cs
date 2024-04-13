using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBaseModule.Tables.Base;
using Mono.Data.Sqlite;

namespace DataBaseModule.Tables
{
    public class LevelTableController : BaseTableController
    {
        public LevelTableController(string dbName)
        {
            CreateTableIfNotExists(dbName);
        }
        
        public int AddLevel(string dbName)
        {
            string commandText = $"INSERT INTO Levels (name) VALUES ('Save')";
            ExecuteCommand(dbName, commandText);
            
            return GetLastLevelIndex(dbName);
        }

        public List<int> GetAllLevelIndexes(string dbName)
        {
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, "SELECT * FROM Levels");
            List<int> indexes = new List<int>();

            while (dataReader.Read())
            {
                indexes.Add(Convert.ToInt32(dataReader["id"]));
            }
            dataReader.Close();
            connection.Close();

            return indexes;
        }

        public Dictionary<int, string> GetLevels(string dbName)
        {
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, "SELECT * FROM Levels");
            Dictionary<int, string> levels = new Dictionary<int, string>();

            while (dataReader.Read())
            {
                levels.Add(Convert.ToInt32(dataReader["id"]), dataReader["name"].ToString());
            }
            dataReader.Close();
            connection.Close();

            return levels;
        }

        public int GetLastLevelIndex(string dbName)
        {
            return GetAllLevelIndexes(dbName).Last();
        }

        public void DeleteLevel(string dbName, int levelIndex)
        {
            string commandText = $"DELETE FROM Levels WHERE id = {levelIndex};";
            ExecuteCommand(dbName, commandText);
        }

        protected override void CreateTableIfNotExists(string dbName)
        {
            string commandText =
                $"CREATE TABLE IF NOT EXISTS Levels (" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"name TEXT" +
                $");";
            ExecuteCommand(dbName, commandText);
        }
    }
}