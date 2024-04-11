namespace CharacterModule.ModelPart
{
    public class PlayerCharacterModel : CharacterModel
    {
        public PlayerCharacterModel(int id, string name, int maxHealth, int maxEnergy, int initiative, int damage, int currentEnergy = -1, int currentHealth = -1) : base(id, name, maxHealth, maxEnergy, initiative, damage, currentEnergy, currentHealth)
        {
        }
    }
}