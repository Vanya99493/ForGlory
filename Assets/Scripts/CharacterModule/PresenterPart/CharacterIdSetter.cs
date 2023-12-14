namespace CharacterModule.PresenterPart
{
    public class CharacterIdSetter
    {
        private int _currentId;

        public CharacterIdSetter(int lastId)
        {
            _currentId = lastId + 1;
        }

        public int GetNewId()
        {
            return _currentId++;
        }
    }
}