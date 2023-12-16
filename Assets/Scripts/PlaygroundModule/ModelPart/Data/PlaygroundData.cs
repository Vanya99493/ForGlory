using System;

namespace PlaygroundModule.ModelPart.Data
{
    [Serializable]
    public class PlaygroundData
    {
        public int Height;
        public int Width;
        public int LengthOfWaterLine;
        public int LengthOfCoast;
        private CellType[,] Playground;
    }
}