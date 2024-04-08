using System.Collections.Generic;
using System.Linq;
using CharacterModule.ModelPart.Data;
using DataBaseModule.Tables.TypeTables.Base;

namespace DataBaseModule.Tables.TypeTables
{
    public class CharacterPositionTypeTableController : BaseTypeTableController
    {
        protected override void FillTable(string dbName)
        {
            string commandText = $"DELETE FROM CharacterPositionType;";
            ExecuteCommand(dbName, commandText);
            List<PositionType> positionTypes = new List<PositionType>()
            {
                PositionType.Castle,
                PositionType.LeftVanguard,
                PositionType.RightVanguard,
                PositionType.Rearguard
            };
            foreach (var positionType in positionTypes)
            {
                commandText = 
                    $"INSERT INTO CharacterPositionType (type) " +
                    $"VALUES ('{positionType.ToString().Split('.').Last().ToLower()}');";
                ExecuteCommand(dbName, commandText);
            }
        }
    }
}