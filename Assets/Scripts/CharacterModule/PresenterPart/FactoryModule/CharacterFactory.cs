using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart.FactoryModule
{
    public abstract class CharacterFactory
    {
        public abstract CharacterPresenter InstantiateCharacter(CharacterView characterPrefab, CharacterData data, Transform parent, Vector3 spawnPosition);
        
        protected void OnDestroyCharacter(CharacterView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}