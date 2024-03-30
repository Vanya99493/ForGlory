using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.ModelPart.Data
{
    [CreateAssetMenu(fileName = "CharacterFullData", menuName = "ScriptableObjects/Data/Characters/CharacterFullData", order = 1)]
    public class CharacterFullData : ScriptableObject
    {
        public CharacterData CharacterData;
        public CharacterView CharacterPrefab;
        public Sprite Icon;
    }
}