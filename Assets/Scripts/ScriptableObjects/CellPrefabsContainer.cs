using System.Collections.Generic;
using CustomClasses;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CellPrefabsContainer", menuName = "ScriptableObjects/Prefabs/CellPixelsPrefabsContainer", order = 2)]
    public class CellPrefabsContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<CellType, List<CellView>> cellPixels;

        public List<CellView> CellPixelPrefabs(CellType type) => cellPixels[type];
    }
}