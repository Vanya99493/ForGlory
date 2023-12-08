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

        public bool SetCharacterOnCell(TeamPresenter team)
        {
            return Model.SetCharacterOnCell(team);
        }

        public void RemoveCharacterFromCell()
        {
            Model.RemoveCharacterFromCell();
        }

        public void ActivateCell() => _view.ActivateCell();
        public void DeactivateCell() => _view.DeactivateCell();
    }
}