using System;
using System.Collections.Generic;
using CustomClasses;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;
using ScriptableObjects;
using UnityEngine;

namespace Infrastructure.Providers
{
    [Serializable]
    public class CellDataProvider
    {
        [SerializeField] private CellDataContainer cellDataContainer;

        public SerializableDictionary<Direction, List<CellType>> GetCellPixelsMoveRoots(CellType type)
        {
            List<CellData> cellDataContainers = cellDataContainer.CellData();
            
            foreach (var cellDataContainer in cellDataContainers)
                if (cellDataContainer.cellType == type)
                    return cellDataContainer.moveRoot;
            
            return null;
        }

        public SerializableDictionary<Direction, SerializableDictionary<CellType, double>> GetCellPixelsSpawnRoot(
            CellType type)
        {
            List<CellData> cellDataContainers = cellDataContainer.CellData();
            
            foreach (var cellDataContainer in cellDataContainers)
                if (cellDataContainer.cellType == type)
                    return cellDataContainer.spawnRoot;
            
            return null;
        }

        public List<CellView> GetCellPixelPrefabs(CellType type)
        {
            List<CellData> cellDataContainers = cellDataContainer.CellData();
            
            foreach (var cellDataContainer in cellDataContainers)
                if (cellDataContainer.cellType == type)
                    return cellDataContainer.cellPrefabs;
            
            return null;
        }
    }
}