using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class CharacterData
    {
        public int Id;
        public string Name;
        public int CurrentHealth;
        public int MaxHealth;
        public int CurrentEnergy;
        public int MaxEnergy;
        public int Initiative;
        public int Damage;
        public int Vision;
        public CharacterType CharacterType;
        public PositionType PositionType;
    }
}