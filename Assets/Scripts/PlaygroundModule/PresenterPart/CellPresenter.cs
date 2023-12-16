using CharacterModule.PresenterPart;
using PlaygroundModule.ModelPart;
using PlaygroundModule.ViewPart;

namespace PlaygroundModule.PresenterPart
{
    public class CellPresenter
    {
        public CellModel Model { get; private set; }

        private CellView _view;

        public CellPresenter(CellModel model)
        {
            Model = model;
        }

        public void InitializeView(CellView view)
        {
            _view = view;
        }

        public bool PreSetCharacterOnCell(TeamPresenter team)
        {
            return Model.PreSetCharacterOnCell(team);
        }

        public bool SetCharacterOnCell(TeamPresenter team, bool isFirstInitialization = false)
        {
            return Model.SetCharacterOnCell(team, isFirstInitialization);
        }

        public bool CheckCellOnCharacters()
        {
            return Model.CheckCellOnCharacters();
        } 

        public void RemoveCharacterFromCell(TeamPresenter team)
        {
            Model.RemoveCharacterFromCell(team);
        }

        public void ActivateCell() => _view.ActivateCell();
        public void ActivateRedCell() => _view.ActivateRedCell();
        public void DeactivateCell() => _view.DeactivateCell();
    }
}