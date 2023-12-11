using CharacterModule.ModelPart;
using CharacterModule.ViewPart;

namespace CharacterModule.PresenterPart
{
    public class PlayerCharacterPresenter : CharacterPresenter
    {
        public PlayerCharacterPresenter(PlayerCharacterModel model, PlayerCharacterView view) : base(model, view)
        {
        }
    }
}