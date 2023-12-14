namespace CharacterModule.ModelPart
{
    public class PlayerCharacterModel : CharacterModel
    {
        public PlayerCharacterModel(int id, string name, int maxHealth, int maxEnergy, int initiative, int damage) : base(id, name, maxHealth, maxEnergy, initiative, damage)
        {
        }
    }
}