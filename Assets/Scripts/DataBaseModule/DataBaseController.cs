using System.Collections.Generic;
using LevelModule.Data;

namespace DataBaseModule
{
    public class DataBaseController
    {
        public LevelData GetLevelDataById(int levelId)
        {
            return null;
        }

        public int GetLastLevelId()
        {
            return 0;
        }

        public List<int> GetLevelsId()
        {
            return new List<int> { 3, 5, 8, 9, 11, 15, 18, 19, 24 };
        }
    }
}