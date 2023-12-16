using System.Collections.Generic;
using BattleModule.ViewPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/Prefabs/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        public BattlegroundView BattlegroundView;
        public TeamView TeamView;
        public List<CharacterFullData> Characters;
    }
}