using Infrastructure.ServiceLocatorModule;

namespace Infrastructure.Services
{
    public class CharacterIdSetter : IService
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