using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class TeamsData
    {
        public CharacterFullData[] PlayersInCastle;
        public TeamData PlayerTeam;
        public TeamData[] EnemyTeams;
    }
}