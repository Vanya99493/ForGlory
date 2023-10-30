using PlaygroundModule.ViewPart;

namespace PlaygroundModule.ModelPart
{
    public class PlaygroundModel
    {
        private readonly PlaygroundView _view;
        private Cell[,] _playground;

        public int Height => _playground.GetLength(0);
        public int Width => _playground.GetLength(1);

        public PlaygroundModel(PlaygroundView playgroundView)
        {
            _view = playgroundView;
        }

        public void InitializePlayground(Cell[,] playground)
        {
            _playground = new Cell[playground.GetLength(0), playground.GetLength(1)];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _playground[i, j] = playground[i, j];
                }
            }
        }

        public Cell GetCell(int heightIndex, int widthIndex) => _playground[heightIndex, widthIndex];
    }
}