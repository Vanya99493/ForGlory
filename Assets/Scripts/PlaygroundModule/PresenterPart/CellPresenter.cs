using CharacterModule.PresenterPart;
using PlaygroundModule.ModelPart;

namespace PlaygroundModule.PresenterPart
{
    public class CellPresenter
    {
        public CellModel Model { get; private set; }

        public CellPresenter(CellModel model)
        {
            Model = model;
        }

        public bool SetCharacterOnCell(CharacterPresenter character)
        {
            return Model.SetCharacterOnCell(character);
        }

        public void RemoveCharacterFromCell()
        {
            Model.RemoveCharacterFromCell();
        }
    }
}