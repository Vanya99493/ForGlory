using System.Collections.Generic;
using BattleModule.ViewPart;
using CastleModule.ViewPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameScenePrefabsContainer", menuName = "ScriptableObjects/Prefabs/GameScenePrefabsContainer", order = 1)]
    public class GameScenePrefabsContainer : ScriptableObject
    {
        public BattlegroundView BattlegroundView;
        public TeamView TeamView;
        public List<CharacterFullData> Characters;
        public CastleView CastleView;
    }
}