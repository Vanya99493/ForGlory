namespace PlaygroundModule.ModelPart
{
    public class Cell
    {
        private CellType _cellType;

        public CellType CellType => _cellType;

        public Cell(CellType cellType)
        {
            _cellType = cellType;
        }
    }
}