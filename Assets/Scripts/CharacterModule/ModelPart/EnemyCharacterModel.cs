namespace CharacterModule.ModelPart
{
    public class EnemyCharacterModel : CharacterModel
    {
        public int MaxVision { get; private set; }
        
        public EnemyCharacterModel(string name, int maxHealth, int maxEnergy, int maxVision, int damage) : base(name, maxHealth, maxEnergy, damage)
        {
            MaxVision = maxVision;
        }
    }
}