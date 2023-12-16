using System;

namespace CharacterModule.ModelPart
{
    public abstract class CharacterModel
    {
        public event Action Death; 

        public readonly int Id;
        public readonly string Name;
        
        public int Health { get; private set; }
        public int MaxEnergy { get; private set; }
        public int Initiative { get; private set; }
        public int Damage { get; private set; }
        
        private int _maxHealth;

        public CharacterModel(int id, string name, int maxHealth, int maxEnergy, int initiative, int damage, int currentHealth = -1)
        {
            Id = id;
            Name = name;
            _maxHealth = maxHealth;
            Health = currentHealth == -1 ? _maxHealth : currentHealth;
            MaxEnergy = maxEnergy;
            Initiative = initiative;
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
                Death?.Invoke();
                return;
            }

            Health -= damage;
        }
    }
}