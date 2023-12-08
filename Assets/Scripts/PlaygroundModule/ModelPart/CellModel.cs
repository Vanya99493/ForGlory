using CharacterModule.PresenterPart;
using PlaygroundModule.ViewPart;
using UnityEngine;

namespace PlaygroundModule.ModelPart
{
    public class CellModel
    {
        private CellType _cellType;
        private CellView _view;

        public CellType CellType => _cellType;
        
        public int CellHeightId { get; private set; }
        public int CellWidthId { get; private set; }
        public TeamPresenter TeamOnCell { get; private set; }
        public Vector3 MoveCellPosition => _view.MoveCellPosition;

        public CellModel(CellType cellType, int cellHeightId, int cellWidthId)
        {
            _cellType = cellType;
            CellHeightId = cellHeightId;
            CellWidthId = cellWidthId;
        }

        public void IntitializeView(CellView view)
        {
            _view = view;
        }

        public bool SetCharacterOnCell(TeamPresenter team)
        {
            if (TeamOnCell != null)
            {
                return false;
            }
            
            TeamOnCell = team;
            return true;
        }

        public void RemoveCharacterFromCell()
        {
            TeamOnCell = null;
        }
    }
}