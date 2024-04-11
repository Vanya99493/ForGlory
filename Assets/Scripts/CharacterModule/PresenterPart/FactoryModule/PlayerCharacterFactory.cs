using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
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
            _characterIdSetter ??= ServiceLocator.Instance.GetService<CharacterIdSetter>();

            CharacterModel characterModel = new PlayerCharacterModel(_characterIdSetter.GetNewId(), data.Name,
                data.MaxHealth, data.MaxEnergy, data.Initiative, data.Damage, data.CurrentEnergy, data.CurrentHealth);
            CharacterPresenter characterPresenter = new PlayerCharacterPresenter(characterModel as PlayerCharacterModel,
                characterView as PlayerCharacterView);
            
            return characterPresenter;
        }
    }
}