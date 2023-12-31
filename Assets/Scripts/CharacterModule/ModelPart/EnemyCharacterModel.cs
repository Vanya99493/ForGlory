﻿namespace CharacterModule.ModelPart
{
    public class EnemyCharacterModel : CharacterModel
    {
        public int MaxVision { get; private set; }
        
        public EnemyCharacterModel(int id, string name, int maxHealth, int maxEnergy, int maxVision, int initiative, int damage, int currentHealth = -1) : base(id, name, maxHealth, maxEnergy, initiative, damage, currentHealth)
        {
            MaxVision = maxVision;
        }
    }
}