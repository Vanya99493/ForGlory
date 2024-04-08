using System.Collections.Generic;
using System.Linq;
using DataBaseModule.Tables.Base;
using DataBaseModule.Tables.TypeTables;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ModelPart.Data;

namespace DataBaseModule.Tables
{
    public class CellTableController : BaseTableController
    {
        private CellTypeTableController _cellTypeTableController;

        public CellTableController(CellTypeTableController cellTypeTableController)
        {
            _cellTypeTableController = cellTypeTableController;
        }
        
        public void AddCellData(string dbName, int playgroundId, CreatedPlaygroundData playgroundData)
        {
            Dictionary<string, int> cellTypeData = _cellTypeTableController.GetCellTypes(dbName, "CellType");
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
    }
}