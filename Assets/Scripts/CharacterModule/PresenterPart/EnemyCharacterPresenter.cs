using CharacterModule.ModelPart;
using CharacterModule.ViewPart;

namespace CharacterModule.PresenterPart
{
    public class EnemyCharacterPresenter : CharacterPresenter
    {
        public EnemyCharacterPresenter(EnemyCharacterModel model, EnemyCharacterView view) : base(model, view)
        {
        }
    }
}