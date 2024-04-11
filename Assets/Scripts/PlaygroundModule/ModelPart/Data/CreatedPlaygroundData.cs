using System;

namespace PlaygroundModule.ModelPart.Data
{
    [Serializable]
    public class CreatedPlaygroundData
    {
        public int DBPlaygroundId;
        public int CastleHeightIndex;
        public int CastleWidthIndex;
        public CellType[,] Playground;
    }
}