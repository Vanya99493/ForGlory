using System;
using CharacterModule.ModelPart;
using CharacterModule.ModelPart.Data;
using CharacterModule.PresenterPart.BehaviourModule;
using CharacterModule.PresenterPart.BehaviourModule.Base;
using CharacterModule.ViewPart;
using Infrastructure.Providers;
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

        public override TeamPresenter InstantiateTeam(GameScenePrefabsProvider gameScenePrefabsProvider,
            TeamData teamData, IBehaviour enemyBehaviour)
        {
            EnemyCharacterFactory characterFactory = new EnemyCharacterFactory();
            
            TeamView teamView = Object.Instantiate(gameScenePrefabsProvider.GetTeamView(), new Vector3(), Quaternion.identity);
            
            if(Parent == null)
                CreateParent();
            
            teamView.transform.parent = Parent;
            teamView.Destroy += OnDestroyTeam;
            
            CharacterPresenter[] instantiatedCharacters = new CharacterPresenter[teamData.CharactersData.Length];

            for (int i = 0; i < instantiatedCharacters.Length; i++)
            {
                instantiatedCharacters[i] = characterFactory.InstantiateCharacter(
                    gameScenePrefabsProvider.GetCharacterByName(teamData.CharactersData[i].Name).CharacterPrefab,
                    teamData.CharactersData[i],
                    teamView.transform, teamView.characterPositions[i].position
                    );
            }
            
            TeamModel teamModel = new EnemyTeamModel(teamData.HeightCellIndex, teamData.WidthCellIndex);
            teamModel.SetCharacters(instantiatedCharacters);
            ((EnemyTeamModel)teamModel).SetVision();
            TeamPresenter teamPresenter = new EnemyTeamPresenter(teamModel, teamView, enemyBehaviour as EnemyBehaviour);
            
            DownScaleTeam(teamPresenter);
            
            if (teamPresenter == null)
                throw new Exception("Enemy team initialization was wrong");
            
            return teamPresenter;
        }
    }
}