using System;

namespace CharacterModule.ModelPart
{
    public abstract class CharacterModel
    {
        public event Action Death;
        public event Action<int, int> Damaged;

        public readonly int Id;
        public readonly string Name;
        
        public int Health { get; private set; }
        public int MaxEnergy { get; private set; }
        public int Initiative { get; private set; }
        public int Damage { get; private set; }
        public int MaxHealth { get; private set; }

        public CharacterModel(int id, string name, int maxHealth, int maxEnergy, int initiative, int damage, int currentHealth = -1)
        {
            Id = id;
            Name = name;
            MaxHealth = maxHealth;
            Health = currentHealth == -1 ? MaxHealth : currentHealth;
            MaxEnergy = maxEnergy;
            Initiative = initiative;
            Damage = damage;
        }

        public void Heal(int health)
        {
            Health = Health + health > MaxHealth ? MaxHealth : Health + health;
        }

        public void TakeDamage(int damage)
        {
            if (Health - damage <= 0)
            {
                Health = 0;
                Damaged?.Invoke(MaxHealth, Health);
                Death?.Invoke();
                return;
            }

            Health -= damage;
            Damaged?.Invoke(MaxHealth, Health);
        }
    }
}