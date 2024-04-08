using System.Collections.Generic;
using System.Linq;
using DataBaseModule.Tables.TypeTables.Base;
using PlaygroundModule.ModelPart;

namespace DataBaseModule.Tables.TypeTables
{
    public class CellTypeTableController : BaseTypeTableController
    {
        protected override void FillTable(string dbName)
        {
            string commandText = $"DELETE FROM CellType;";
            ExecuteCommand(dbName, commandText);
            List<CellType> cellTypes = new List<CellType>()
            {
                CellType.Plain,
                CellType.Hill,
                CellType.Water,
                CellType.RampBT,
                CellType.RampLR,
                CellType.RampRL,
                CellType.RampTB
            };
            foreach (var cellType in cellTypes)
            {
                commandText = 
                    $"INSERT INTO CellType (type) " +
                    $"VALUES ('{cellType.ToString().Split('.').Last().ToLower()}');";
                ExecuteCommand(dbName, commandText);
            }
        }
    }
}