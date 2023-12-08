using CharacterModule.ModelPart;
using CharacterModule.ViewPart;

namespace CharacterModule.PresenterPart
{
    public abstract class CharacterPresenter
    {
        public readonly CharacterModel Model;
        public readonly CharacterView View;

        protected CharacterPresenter(CharacterModel model, CharacterView view)
        {
            Model = model;
            View = view;
        }
    }
}