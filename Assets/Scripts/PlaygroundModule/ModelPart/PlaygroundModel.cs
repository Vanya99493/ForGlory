using System.Collections.Generic;
using CharacterModule.PresenterPart;
using PlaygroundModule.PresenterPart;
using PlaygroundModule.PresenterPart.WideSearchModule;

namespace PlaygroundModule.ModelPart
{
    public class PlaygroundModel
    {
        public List<Node> ActiveCells;
        
        private CellPresenter[,] _playground;

        public int PlaygroundHeight => _playground.GetLength(0);
        public int PlaygroundWidth => _playground.GetLength(1);

        public PlaygroundModel()
        {
            ActiveCells = new List<Node>();
        }
        
        public void InitializePlayground(CellPresenter[,] playground)
        {
            _playground = new CellPresenter[playground.GetLength(0), playground.GetLength(1)];
            for (int i = 0; i < PlaygroundHeight; i++)
            {
                for (int j = 0; j < PlaygroundWidth; j++)
                {
                    _playground[i, j] = playground[i, j];
                }
            }
        }

        public CellPresenter GetCellPresenter(int heightIndex, int widthIndex) => _playground[heightIndex, widthIndex];

        public bool PreSetCharacterOnCell(TeamPresenter team, int heightCellIndex, int widthCellIndex)
        {
            return _playground[heightCellIndex, widthCellIndex].PreSetCharacterOnCell(team);
        }

        public bool SetCharacterOnCell(TeamPresenter team, int heightCellIndex, int widthCellIndex, bool isFirstInitialization = false)
        {
            return _playground[heightCellIndex, widthCellIndex].SetCharacterOnCell(team, isFirstInitialization);
        }
        
        public bool CheckCellOnCharacters(int heightCellIndex, int widthCellIndex)
        {
            return _playground[heightCellIndex, widthCellIndex].CheckCellOnCharacters();
        }

        public void RemoveCharacterFromCell(TeamPresenter team, int heightCellIndex, int widthCellIndex)
        {
            _playground[heightCellIndex, widthCellIndex].RemoveCharacterFromCell(team);
        }
    }
}