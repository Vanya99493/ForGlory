using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart.FactoryModule
{
    public class PlayerCharacterFactory : CharacterFactory
    {
        public override CharacterPresenter InstantiateCharacter(CharacterView characterPrefab, CharacterData data, Transform parent, Vector3 spawnPosition)
        {
            CharacterView characterView = Object.Instantiate(characterPrefab, parent.transform);
            characterView.transform.position = spawnPosition;
            characterView.Destroy += OnDestroyCharacter;
            
            CharacterModel characterModel = new PlayerCharacterModel(data.Id, data.Name, data.MaxHealth, data.MaxEnergy, data.Initiative, data.Damage, data.CurrentHealth);
            CharacterPresenter characterPresenter = new PlayerCharacterPresenter(characterModel as PlayerCharacterModel, characterView as PlayerCharacterView);
            
            return characterPresenter;
        }
    }
}