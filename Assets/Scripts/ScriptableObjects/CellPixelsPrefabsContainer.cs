using CustomClasses;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "CellPixelsPrefabsContainer", menuName = "ScriptableObjects/Prefabs/CellPixelsPrefabsContainer", order = 2)]
    public class CellPixelsPrefabsContainer : ScriptableObject
    {
        [SerializeField] private SerializableDictionary<CellPixelInfo, CellPixelView> cellPixels;

        public CellPixelView CellPixelPrefabs(CellPixelInfo info) => cellPixels[info];
    }
}