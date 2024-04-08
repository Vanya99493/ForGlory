using System;
using System.Data;
using DataBaseModule.Tables.Base;
using Mono.Data.Sqlite;
using PlaygroundModule.ModelPart.Data;

namespace DataBaseModule.Tables
{
    public class PlaygroundTableController : BaseTableController
    {
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

        private int GetLastPlaygroundId(string dbName)
        {
            string commandName =
                $"SELECT * FROM Playgrounds ORDER BY id DESC;";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandName);
            int lastPlaygroundId = Convert.ToInt32(dataReader["id"]);
            dataReader.Close();
            connection.Close();

            return lastPlaygroundId;
        }
    }
}