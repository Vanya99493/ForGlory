using System.Collections.Generic;
using System.Linq;
using DataBaseModule.Tables.TypeTables.Base;
using CharacterModule.ModelPart.Data;

namespace DataBaseModule.Tables.TypeTables
{
    public class TeamTypeTableController : BaseTypeTableController
    {
        protected override void FillTable(string dbName)
        {
            string commandText = $"DELETE FROM CellType;";
            ExecuteCommand(dbName, commandText);
            List<TeamType> cellTypes = new List<TeamType>()
            {
                TeamType.Players,
                TeamType.Enemies,
                TeamType.Castle
            };
            foreach (var cellType in cellTypes)
            {
                commandText = 
                    $"INSERT INTO TeamType (type) " +
                    $"VALUES ('{cellType.ToString().Split('.').Last().ToLower()}');";
                ExecuteCommand(dbName, commandText);
            }
        }
    }
}