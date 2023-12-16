﻿using System;

namespace CharacterModule.ModelPart.Data
{
    [Serializable]
    public class TeamsData
    {
        public TeamData PlayerTeam;
        public TeamData[] EnemyTeams;
    }
}