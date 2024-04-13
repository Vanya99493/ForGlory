using System;
using System.Data;
using DataBaseModule.Tables.Base;
using Mono.Data.Sqlite;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ModelPart.Data;

namespace DataBaseModule.Tables
{
    public class PlaygroundTableController : BaseTableController
    {
        public PlaygroundTableController(string dbName)
        {
            CreateTableIfNotExists(dbName);
        }
        
        public int AddPlaygroundData(string dbName, int levelId, CreatedPlaygroundData playgroundData)
        {
            string commandText = 
                $"INSERT INTO Playgrounds (level_id, height, width, castle_height_index, castle_width_index)" +
                $"VALUES ('{levelId}', '{playgroundData.Playground.GetLength(0)}', " +
                $"'{playgroundData.Playground.GetLength(1)}', '{playgroundData.CastleHeightIndex}', " +
                $"'{playgroundData.CastleWidthIndex}');";
            
            ExecuteCommand(dbName, commandText);
            return GetLastPlaygroundId(dbName);
        }

        public CreatedPlaygroundData GetPlaygroundData(string dbName, int levelId)
        {
            string commandText =
                $"SELECT * FROM Playgrounds " +
                $"WHERE level_id = {levelId}";

            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

            CreatedPlaygroundData playground = new CreatedPlaygroundData()
            {
                DBPlaygroundId = Convert.ToInt32(dataReader["id"].ToString()),
                CastleHeightIndex = Convert.ToInt32(dataReader["castle_height_index"].ToString()),
                CastleWidthIndex = Convert.ToInt32(dataReader["castle_width_index"].ToString()),
                Playground = new CellType[Convert.ToInt32(dataReader["height"].ToString()), Convert.ToInt32(dataReader["width"].ToString())]
            };
            
            dataReader.Close();
            connection.Close();

            return playground;
        }

        private int GetLastPlaygroundId(string dbName)
        {
            string commandText =
                $"SELECT * FROM Playgrounds ORDER BY id DESC;";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);
            int lastPlaygroundId = Convert.ToInt32(dataReader["id"]);
            dataReader.Close();
            connection.Close();

            return lastPlaygroundId;
        }

        protected override void CreateTableIfNotExists(string dbName)
        {
            string commandText =
                $"CREATE TABLE IF NOT EXISTS Playgrounds (" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"level_id INTEGER REFERENCES Levels (id), " +
                $"height INTEGER, " +
                $"width INTEGER, " +
                $"castle_height_index INTEGER, " +
                $"castle_width_index INTEGER" +
                $");";
            ExecuteCommand(dbName, commandText);
        }
    }
}