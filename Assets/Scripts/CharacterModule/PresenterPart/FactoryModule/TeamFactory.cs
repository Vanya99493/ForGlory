using CharacterModule.ModelPart.Data;
using CharacterModule.PresenterPart.BehaviourModule.Base;
using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart.FactoryModule
{
    public abstract class TeamFactory
    {
        protected Transform Parent;

        public abstract TeamPresenter InstantiateTeam(TeamData teamData);

        public abstract TeamPresenter InstantiateTeam(TeamView teamPrefab, TeamData teamData, IBehaviour characterBehaviour);

        public void RemoveParent()
        {
            Object.Destroy(Parent.gameObject);
        }
        
        protected void CreateParent()
        {
            Parent = new GameObject("TeamsParent").transform;
        }

        protected void OnDestroyTeam(TeamView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }

        public static void UpScaleTeam(TeamPresenter teamPresenter)
        {
            for (int i = 0; i < teamPresenter.Model.CharactersCount; i++)
            {
                var character = teamPresenter.Model.GetCharacterPresenter(i).View;
                character.transform.localScale = new Vector3(
                    character.transform.localScale.x * 1.5f,
                    character.transform.localScale.y * 1.5f,
                    character.transform.localScale.z * 1.5f
                );
            }
        }

        public static void DownScaleTeam(TeamPresenter teamPresenter)
        {
            for (int i = 0; i < teamPresenter.Model.CharactersCount; i++)
            {
                var character = teamPresenter.Model.GetCharacterPresenter(i).View;
                character.transform.localScale = new Vector3(
                    character.transform.localScale.x / 1.5f,
                    character.transform.localScale.y / 1.5f,
                    character.transform.localScale.z / 1.5f
                );
            }
        }

        public static void ResetTeamPosition(TeamPresenter teamPresenter)
        {
            for (int i = 0; i < teamPresenter.Model.CharactersCount; i++)
            {
                teamPresenter.Model.GetCharacterPresenter(i).View.transform.position = 
                    teamPresenter.View.characterPositions[i].position;
            }
        }
    }
}