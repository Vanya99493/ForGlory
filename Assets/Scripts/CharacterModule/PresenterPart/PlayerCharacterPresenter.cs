using CharacterModule.ModelPart;
using CharacterModule.ViewPart;

namespace CharacterModule.PresenterPart
{
    public class PlayerCharacterPresenter : CharacterPresenter
    {
        public PlayerCharacterPresenter(CharacterModel model, CharacterView view) : base(model, view)
        {
        }
    }
}