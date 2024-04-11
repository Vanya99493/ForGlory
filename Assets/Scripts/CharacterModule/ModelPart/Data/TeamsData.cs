using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class TeamsData
    {
        public TeamData PlayersInCastleTeam;
        public TeamData PlayerTeam;
        public TeamData[] EnemyTeams;
    }
}