using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DataBaseModule.Tables.Base;
using DataBaseModule.Tables.TypeTables;
using Mono.Data.Sqlite;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ModelPart.Data;

namespace DataBaseModule.Tables
{
    public class CellTableController : BaseTableController
    {
        private CellTypeTableController _cellTypeTableController;

        public CellTableController(string dbName, CellTypeTableController cellTypeTableController)
        {
            _cellTypeTableController = cellTypeTableController;
            CreateTableIfNotExists(dbName);
        }
        
        public void AddCellData(string dbName, int playgroundId, CreatedPlaygroundData playgroundData)
        {
            Dictionary<string, int> cellTypeData = _cellTypeTableController.GetStringTypes(dbName, "CellType");
            for (int i = 0; i < playgroundData.Playground.GetLength(0); i++)
            {
                for (int j = 0; j < playgroundData.Playground.GetLength(1); j++)
                {
                    if (playgroundData.Playground[i, j] == CellType.Water)
                        continue;
                    string commandText =
                        $"INSERT INTO Cells (playground_id, cell_type_id, height_index, width_index) " +
                        $"VALUES ('{playgroundId}', " +
                        $"'{cellTypeData[playgroundData.Playground[i, j].ToString().Split('.').Last().ToLower()]}', " +
                        $"'{i}', '{j}');";
            
                    ExecuteCommand(dbName, commandText);
                }
            }
        }

        public void GetCellsData(string dbName, ref CreatedPlaygroundData createdPlaygroundData)
        {
            string commandText =
                $"SELECT * FROM Cells " +
                $"WHERE playground_id = {createdPlaygroundData.DBPlaygroundId}";

            (IDataReader dataReader, SqliteConnection connection) = GetData(dbName, commandText);

            Dictionary<string, CellType> cellTypes = _cellTypeTableController.GetCellTypes(dbName);
            Dictionary<int, string> cellStringTypes = _cellTypeTableController.GetStringTypesReverse(dbName, "CellType");

            while (dataReader.Read())
            {
                createdPlaygroundData.Playground[
                    Convert.ToInt32(dataReader["height_index"].ToString()), 
                    Convert.ToInt32(dataReader["width_index"].ToString())
                    ] 
                    = cellTypes[cellStringTypes[Convert.ToInt32(dataReader["cell_type_id"].ToString())]];
            }
            
            dataReader.Close();
            connection.Close();
        }

        protected override void CreateTableIfNotExists(string dbName)
        {
            string commandText =
                $"CREATE TABLE IF NOT EXISTS Cells (" +
                $"id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                $"playground_id INTEGER REFERENCES Playgrounds (id), " +
                $"cell_type_id INTEGER REFERENCES CellType (id), " +
                $"height_index INTEGER, " +
                $"width_index INTEGER" +
                $");";
            ExecuteCommand(dbName, commandText);
        }
    }
}