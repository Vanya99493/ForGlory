namespace CastleModule.ModelPart
{
    public class CastleModel
    {
        public int HeightCellIndex { get; private set; }
        public int WidthCellIndex { get; private set; }

        public CastleModel(int heightCellIndex, int widthCellIndex)
        {
            HeightCellIndex = heightCellIndex;
            WidthCellIndex = widthCellIndex;
        }
    }
}