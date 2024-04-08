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
    public class PlayerTeamFactory : TeamFactory
    {
        public override TeamPresenter InstantiateTeam(TeamData teamData)
        {
            return null;
        }

        public override TeamPresenter InstantiateTeam(GameScenePrefabsProvider gameScenePrefabsProvider, 
            TeamData teamData, IBehaviour playerBehaviour)
        {
            PlayerCharacterFactory characterFactory = new PlayerCharacterFactory();

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
            
            TeamModel teamModel = new PlayerTeamModel(teamData.HeightCellIndex, teamData.WidthCellIndex);
            teamModel.SetCharacters(instantiatedCharacters);
            TeamPresenter teamPresenter = new PlayerTeamPresenter(teamModel, teamView, playerBehaviour as PlayerBehaviour);
            
            DownScaleTeam(teamPresenter);

            if (teamPresenter == null)
                throw new Exception("Player team initialization was wrong");
            
            return teamPresenter;
        }
    }
}