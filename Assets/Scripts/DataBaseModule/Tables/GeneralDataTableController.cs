using DataBaseModule.Tables.Base;
using LevelModule.Data;

namespace DataBaseModule.Tables
{
    public class GeneralDataTableController : BaseTableController
    {
        public void AddGeneralData(string dbName, int levelId, GeneralData generalData)
        {
            string commandText =
                $"INSERT INTO GeneralData (level_id, last_character_id) " +
                $"VALUES ('{levelId}', '{generalData.LastCharacterId}')";
            ExecuteCommand(dbName, commandText);
        }
    }
}