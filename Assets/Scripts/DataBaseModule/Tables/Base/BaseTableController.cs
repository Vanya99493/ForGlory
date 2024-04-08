using System.Data;
using Mono.Data.Sqlite;

namespace DataBaseModule.Tables.Base
{
    public abstract class BaseTableController
    {
        protected void ExecuteCommand(string dbName, string commandText)
        {
            using (var connection = new SqliteConnection(dbName))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        protected (IDataReader, SqliteConnection) GetData(string dbName, string commandText)
        {
            var connection = new SqliteConnection(dbName);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = commandText;
            IDataReader reader = command.ExecuteReader();

            return (reader, connection);
        }  
    }
}