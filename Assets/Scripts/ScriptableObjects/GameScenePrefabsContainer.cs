using CharacterModule.ViewPart;
using CustomClasses;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/Prefabs/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        public SerializableDictionary<string, CharacterView> CharacterPrefabsMap;
    }
}