using System.Collections.Generic;
using CharacterModule.ModelPart.Data;
using DataBaseModule.Tables;
using DataBaseModule.Tables.TypeTables;
using LevelModule.Data;
using PlaygroundModule.ModelPart.Data;

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

            _cellTypeTableController = new CellTypeTableController(_dbName);
            _characterPositionTypeTableController = new CharacterPositionTypeTableController(_dbName);
            _teamTypeTableController = new TeamTypeTableController(_dbName);
            
            _cellTypeTableController.Initialize(_dbName, "CellType", 6);
            _characterPositionTypeTableController.Initialize(_dbName, "CharacterPositionType", 4);
            _teamTypeTableController.Initialize(_dbName, "TeamType", 3);
            
            _levelTableController = new LevelTableController(_dbName);
            _generalDataTableController = new GeneralDataTableController(_dbName);
            _playgroundTableController = new PlaygroundTableController(_dbName);
            _cellTableController = new CellTableController(_dbName, _cellTypeTableController);
            _teamTableController = new TeamTableController(_dbName, _teamTypeTableController);
            _characterTableController = new CharacterTableController(_dbName, _characterPositionTypeTableController);
        }
        
        public LevelData GetLevelDataById(int levelId)
        {
            GeneralData generalData = _generalDataTableController.GetGeneralData(_dbName, levelId);
            CreatedPlaygroundData createdPlaygroundData = _playgroundTableController.GetPlaygroundData(_dbName, levelId);
            _cellTableController.GetCellsData(_dbName, ref createdPlaygroundData);
            TeamsData teamsData = _teamTableController.GetTeamsData(_dbName, levelId);
            _characterTableController.GetCharactersData(_dbName, ref teamsData);
            
            LevelData levelData = new ()
            {
                GeneralData = generalData,
                PlaygroundData = createdPlaygroundData,
                TeamsData = teamsData
            };
            
            return levelData;
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
    }
}