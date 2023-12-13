using BattleModule.ViewPart;
using CharacterModule.ViewPart;
using CustomClasses;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/Prefabs/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        public BattlegroundView BattlegroundView;
        public TeamView TeamView;
        public SerializableDictionary<string, CharacterView> CharacterPrefabsMap;
    }
}