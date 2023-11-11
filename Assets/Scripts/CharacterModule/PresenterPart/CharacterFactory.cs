using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class CharacterFactory
    {
        public CharacterView InstantiateCharacter(CharacterView characterPrefab)
        {
            CharacterView characterView = Object.Instantiate(characterPrefab, new Vector3(), Quaternion.identity);
            characterView.Destroy += OnDestroy;
            return characterView;
        }

        private void OnDestroy(CharacterView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}