using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class CharacterFactory
    {
        public CharacterView InstantiateCharacter(CharacterView characterPrefab, Vector3 position, Quaternion rotation)
        {
            CharacterView characterView = Object.Instantiate(characterPrefab, position, rotation);
            characterView.Destroy += OnDestroy;
            return characterView;
        }

        private void OnDestroy(CharacterView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}