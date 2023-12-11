using CharacterModule.ViewPart;
using CustomClasses;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/Prefabs/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        public TeamView TeamView;
        public SerializableDictionary<string, CharacterView> CharacterPrefabsMap;
    }
}