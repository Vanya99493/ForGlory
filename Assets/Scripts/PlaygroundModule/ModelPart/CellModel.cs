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
        public CharacterPresenter CharacterOnCell { get; private set; }
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

        public bool SetCharacterOnCell(CharacterPresenter character)
        {
            if (CharacterOnCell != null)
            {
                return false;
            }
            
            CharacterOnCell = character;
            return true;
        }

        public void RemoveCharacterFromCell()
        {
            CharacterOnCell = null;
        }
    }
}