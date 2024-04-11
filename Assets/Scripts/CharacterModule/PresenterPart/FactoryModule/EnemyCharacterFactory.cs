using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using Infrastructure.ServiceLocatorModule;
using Infrastructure.Services;
using UnityEngine;

namespace CharacterModule.PresenterPart.FactoryModule
{
    public class EnemyCharacterFactory : CharacterFactory
    {
        public override CharacterPresenter InstantiateCharacter(CharacterView characterPrefab, CharacterData data, Transform parent, Vector3 spawnPosition)
        {
            CharacterView enemyView = Object.Instantiate(characterPrefab, parent.transform);
            enemyView.transform.position = spawnPosition;
            enemyView.Destroy += OnDestroyCharacter;
            _characterIdSetter ??= ServiceLocator.Instance.GetService<CharacterIdSetter>();

            CharacterModel enemyModel = new EnemyCharacterModel(_characterIdSetter.GetNewId(), data.Name,
                data.MaxHealth, data.MaxEnergy, data.Vision, data.Initiative, data.Damage, data.CurrentEnergy,
                data.CurrentHealth);
            CharacterPresenter characterPresenter =
                new EnemyCharacterPresenter(enemyModel as EnemyCharacterModel, enemyView as EnemyCharacterView);
            
            return characterPresenter;
        }
    }
}