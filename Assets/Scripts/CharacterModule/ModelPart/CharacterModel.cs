using System;

namespace CharacterModule.ModelPart
{
    public abstract class CharacterModel
    {
        public event Action<CharacterModel> Death; 

        public readonly string Name;
        public int Health { get; private set; }
        public int MaxEnergy { get; private set; }
        public int Damage { get; private set; }
        
        private int _maxHealth;

        public CharacterModel(string name, int maxHealth, int maxEnergy, int damage)
        {
            Name = name;
            _maxHealth = maxHealth;
            Health = _maxHealth;
            MaxEnergy = maxEnergy;
            Damage = damage;
        }

        public void Heal(int health)
        {
            Health = Health + health > _maxHealth ? _maxHealth : Health + health;
        }

        public void TakeDamage(int damage)
        {
            if (Health - damage <= 0)
            {
                Health = 0;
                Death?.Invoke(this);
                return;
            }

            Health -= damage;
        }
    }
}