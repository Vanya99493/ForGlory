using System.Collections.Generic;
using System.Linq;
using CharacterModule.ModelPart.Data;
using DataBaseModule.Tables.Base;
using DataBaseModule.Tables.TypeTables;

namespace DataBaseModule.Tables
{
    public class CharacterTableController : BaseTableController
    {
        private CharacterPositionTypeTableController _characterPositionTypeTableController;

        public CharacterTableController(CharacterPositionTypeTableController characterPositionTypeTableController)
        {
            _characterPositionTypeTableController = characterPositionTypeTableController;
        }

        public void AddCharacterData(string dbName, Dictionary<int, TeamData> teams)
        {
            Dictionary<string, int> characterPositionTypeData = _characterPositionTypeTableController.
                GetCellTypes(dbName, "CharacterPositionType");
            foreach (var team in teams)
            {
                for (int i = 0; i < team.Value.CharactersData.Length; i++)
                {
                    string commandText =
                        $"INSERT INTO Characters (team_id, character_position_type_id, name, character_id, " +
                        $"current_health, max_health, current_energy, max_energy, initiative, damage, vision) " +
                        $"VALUES ('{team.Key}', " +
                        $"'{characterPositionTypeData[team.Value.CharactersData[i].PositionType.ToString().Split('.').Last().ToLower()]}', " +
                        $"'{team.Value.CharactersData[i].Name}', '{team.Value.CharactersData[i].Id}', " +
                        $"'{team.Value.CharactersData[i].CurrentHealth}', '{team.Value.CharactersData[i].MaxHealth}', " +
                        $"'{team.Value.CharactersData[i].CurrentEnergy}', '{team.Value.CharactersData[i].MaxEnergy}', " +
                        $"'{team.Value.CharactersData[i].Initiative}', '{team.Value.CharactersData[i].Damage}', " +
                        $"'{team.Value.CharactersData[i].Vision}')";
                    
                    ExecuteCommand(dbName, commandText);
                }
            }
        }
    }
}