namespace Infrastructure.Data
{
    public class GameData
    {
        private int _currentId;

        public GameData(int lastId)
        {
            _currentId = lastId + 1;
        }

        public int GetNewId()
        {
            return _currentId++;
        }
    }
}