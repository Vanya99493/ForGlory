using CharacterModule.PresenterPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.ViewPart;

namespace PlaygroundModule.ModelPart
{
    public class PlaygroundModel
    {
        private readonly PlaygroundView _view;
        private CellPresenter[,] _playground;

        public int Height => _playground.GetLength(0);
        public int Width => _playground.GetLength(1);

        public PlaygroundModel(PlaygroundView playgroundView)
        {
            _view = playgroundView;
        }

        public void InitializePlayground(CellPresenter[,] playground)
        {
            _playground = new CellPresenter[playground.GetLength(0), playground.GetLength(1)];
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    _playground[i, j] = playground[i, j];
                }
            }
        }

        public CellPresenter GetCellPresenter(int heightIndex, int widthIndex) => _playground[heightIndex, widthIndex];

        public bool SetCharacterOnCell(TeamPresenter team, int heightCellIndex, int widthCellIndex)
        {
            return _playground[heightCellIndex, widthCellIndex].SetCharacterOnCell(team);
        }
        
        public bool CheckCellOnCharacter(int heightCellIndex, int widthCellIndex)
        {
            return _playground[heightCellIndex, widthCellIndex] != null;
        }

        public void RemoveCharacterFromCell(int heightCellIndex, int widthCellIndex)
        {
            _playground[heightCellIndex, widthCellIndex].RemoveCharacterFromCell();
        }
    }
}