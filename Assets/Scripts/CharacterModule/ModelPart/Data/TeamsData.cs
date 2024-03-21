using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class TeamsData
    {
        public CharacterFullData[] Players;
        public TeamData PlayerTeam;
        public TeamData[] EnemyTeams;
    }
}