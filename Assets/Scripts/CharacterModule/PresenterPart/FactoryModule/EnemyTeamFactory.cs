﻿using System;
using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.ViewPart;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CharacterModule.PresenterPart.FactoryModule
{
    public class EnemyTeamFactory : TeamFactory
    {
        public override TeamPresenter InstantiateTeam(TeamData teamData)
        {
            return null;
        }

        public override TeamPresenter InstantiateTeam(TeamView teamPrefab, TeamData teamData)
        {
            EnemyCharacterFactory characterFactory = new EnemyCharacterFactory();
            
            TeamView teamView = Object.Instantiate(teamPrefab, new Vector3(), Quaternion.identity);
            
            if(Parent == null)
                CreateParent();
            
            teamView.transform.parent = Parent;
            teamView.Destroy += OnDestroyTeam;
            
            CharacterPresenter[] instantiatedCharacters = new CharacterPresenter[teamData.CharactersData.Length];

            for (int i = 0; i < instantiatedCharacters.Length; i++)
            {
                instantiatedCharacters[i] = characterFactory.InstantiateCharacter(
                    teamData.CharactersData[i].CharacterPrefab, teamData.CharactersData[i].CharacterData,
                    teamView.transform, teamView.characterPositions[i].position
                    );
            }
            
            TeamModel teamModel = new EnemyTeamModel(teamData.HeightCellIndex, teamData.WidthCellIndex, instantiatedCharacters);
            TeamPresenter teamPresenter = new EnemyTeamPresenter(teamModel, teamView);
            
            DownScaleTeam(teamPresenter);
            
            if (teamPresenter == null)
                throw new Exception("Enemy team initialization was wrong");
            
            return teamPresenter;
        }
    }
}