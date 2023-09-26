using System;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class CellPixelsPrefabsProvider
    {
        [SerializeField] private CellPixelsPrefabsContainer cellPixelsPrefabsContainer;

        public CellPixelView GetCellPixelPrefab(CellPixelInfo info) =>
            cellPixelsPrefabsContainer.CellPixelPrefabs(info);
    }
}