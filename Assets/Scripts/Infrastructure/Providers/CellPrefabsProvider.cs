using System;
using System.Collections.Generic;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace Infrastructure.Providers
{
    [Serializable]
    public class CellPrefabsProvider
    {
        [SerializeField] private CellPrefabsContainer cellPrefabsContainer;

        public List<CellView> GetCellPixelPrefabs(CellType type) =>
            cellPrefabsContainer.CellPixelPrefabs(type);
    }
}