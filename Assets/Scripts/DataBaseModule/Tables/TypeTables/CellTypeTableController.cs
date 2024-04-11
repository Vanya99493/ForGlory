using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBaseModule.Tables.TypeTables.Base;
using Mono.Data.Sqlite;
using PlaygroundModule.ModelPart;

namespace DataBaseModule.Tables.TypeTables
{
    public class CellTypeTableController : BaseTypeTableController
    {
        private readonly List<CellType> _cellTypes = new()
        {
            CellType.Plain,
            CellType.Hill,
            CellType.RampBT,
            CellType.RampLR,
            CellType.RampRL,
            CellType.RampTB
        };
        
        public Dictionary<string, CellType> GetCellTypes(string dbName)
        {
            string commandText = $"SELECT * FROM CellType;";
            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

            CellType[] cellTypes = _cellTypes.ToArray();
            Dictionary<string, CellType> data = new Dictionary<string, CellType>();
            int index = 0;
            while (dataReader.Read())
            {
                data.Add(dataReader["type"].ToString(), cellTypes[index]);
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
            foreach (var cellType in _cellTypes)
            {
                commandText = 
                    $"INSERT INTO CellType (type) " +
                    $"VALUES ('{cellType.ToString().Split('.').Last().ToLower()}');";
                ExecuteCommand(dbName, commandText);
            }
        }
    }
}