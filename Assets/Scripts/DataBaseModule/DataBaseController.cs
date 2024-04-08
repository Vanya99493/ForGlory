using System.Collections.Generic;
using CharacterModule.ModelPart.Data;
using DataBaseModule.Tables;
using DataBaseModule.Tables.TypeTables;
using LevelModule.Data;

namespace DataBaseModule
{
    public class DataBaseController
    {
        private readonly string _dbName;

        private LevelTableController _levelTableController;
        private GeneralDataTableController _generalDataTableController;
        private PlaygroundTableController _playgroundTableController;
        private CellTableController _cellTableController;
        private TeamTableController _teamTableController;
        private CharacterTableController _characterTableController;

        private CellTypeTableController _cellTypeTableController;
        private CharacterPositionTypeTableController _characterPositionTypeTableController;
        private TeamTypeTableController _teamTypeTableController;

        public DataBaseController(string dbName)
        {
            _dbName = $"URI=file:{dbName};";

            _cellTypeTableController = new CellTypeTableController();
            _characterPositionTypeTableController = new CharacterPositionTypeTableController();
            _teamTypeTableController = new TeamTypeTableController();
            
            _cellTypeTableController.Initialize(_dbName, "CellType", 6);
            _characterPositionTypeTableController.Initialize(_dbName, "CharacterPositionType", 4);
            _teamTypeTableController.Initialize(_dbName, "TeamType", 3);
            
            _levelTableController = new LevelTableController();
            _generalDataTableController = new GeneralDataTableController();
            _playgroundTableController = new PlaygroundTableController();
            _cellTableController = new CellTableController(_cellTypeTableController);
            _teamTableController = new TeamTableController(_teamTypeTableController);
            _characterTableController = new CharacterTableController(_characterPositionTypeTableController);
        }
        
        public LevelData GetLevelDataById(int levelId)
        {
            // to do
            
            return null;
        }

        public int GetLastLevelId()
        {
            return _levelTableController.GetLastLevelIndex(_dbName);
        }

        public List<int> GetLevelsId()
        {
            return _levelTableController.GetAllLevelIndexes(_dbName);
        }

        public Dictionary<int, string> GetLevels()
        {
            return _levelTableController.GetLevels(_dbName);
        }

        public void SaveLevel(LevelData levelData)
        {
            int levelId = _levelTableController.AddLevel(_dbName);
            _generalDataTableController.AddGeneralData(_dbName, levelId, levelData.GeneralData);
            int playgroundId = _playgroundTableController.AddPlaygroundData(_dbName, levelId, levelData.PlaygroundData);
            _cellTableController.AddCellData(_dbName, playgroundId, levelData.PlaygroundData);
            Dictionary<int, TeamData> teams = _teamTableController.AddTeamsData(_dbName, levelId, levelData.TeamsData);
            _characterTableController.AddCharacterData(_dbName, teams);
        }

        public void DeleteLevel(int levelIndex)
        {
            _levelTableController.DeleteLevel(_dbName, levelIndex);
        }
    }
}