using System;
using System.Collections.Generic;
using CustomClasses;
using PlaygroundModule.ViewPart;

namespace PlaygroundModule.ModelPart
{
    [Serializable]
    public class CellData
    {
        public CellType cellType;
        public List<CellView> cellPrefabs;
        public SerializableDictionary<Direction, List<CellType>> moveRoot;
        public SerializableDictionary<Direction, SerializableDictionary<CellType, double>> spawnRoot;
    }
}