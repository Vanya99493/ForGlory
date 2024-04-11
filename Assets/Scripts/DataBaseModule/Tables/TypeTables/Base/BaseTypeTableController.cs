using System;
using System.Collections.Generic;
using System.Data;
using DataBaseModule.Tables.Base;
using Mono.Data.Sqlite;

namespace DataBaseModule.Tables.TypeTables.Base
{
    public abstract class BaseTypeTableController : BaseTableController
    {
        public void Initialize(string dbName, string tableName, int typesCount)
        {
            if (!IsInitialized(dbName, tableName, typesCount))
                FillTable(dbName);
        }

        public Dictionary<string, int> GetStringTypes(string dbName, string tableName)
        {
            string commandText = $"SELECT * FROM {tableName};";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);
            
            Dictionary<string, int> data = new Dictionary<string, int>();
            while (dataReader.Read())
                data.Add(dataReader["type"].ToString(), Convert.ToInt32(dataReader["id"]));

            dataReader.Close();
            connection.Close();

            return data;
        }

        public Dictionary<int, string> GetStringTypesReverse(string dbName, string tableName)
        {
            string commandText = $"SELECT * FROM {tableName};";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);
            
            Dictionary<int, string> data = new Dictionary<int, string>();
            while (dataReader.Read())
                data.Add(Convert.ToInt32(dataReader["id"]), dataReader["type"].ToString());

            dataReader.Close();
            connection.Close();

            return data;
        }

        private bool IsInitialized(string dbName, string tableName, int typesCount)
        {
            var data = GetStringTypes(dbName, tableName);
            return data.Count == typesCount;
        }

     
        protected abstract void FillTable(string dbName);
    }
}