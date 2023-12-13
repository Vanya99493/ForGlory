using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class TeamFactory
    {
        private Transform _parent;

        public void CreateParent()
        {
            _parent = new GameObject("TeamsParent").transform;
        }
        
        public (TeamView, CharacterView[]) InstantiateTeam(TeamView teamPrefab, params CharacterView[] characters)
        {
            if(_parent == null)
                CreateParent();
            
            TeamView teamView = Object.Instantiate(teamPrefab, new Vector3(), Quaternion.identity);
            teamView.transform.parent = _parent;
            teamView.Destroy += OnDestroy;

            CharacterView[] instantiatedCharacters = new CharacterView[characters.Length];
            
            for (int i = 0; i < characters.Length; i++)
            {
                instantiatedCharacters[i] = Object.Instantiate(characters[i], teamView.transform);
                instantiatedCharacters[i].transform.localScale = new Vector3(
                    instantiatedCharacters[i].transform.localScale.x / 1.5f,
                    instantiatedCharacters[i].transform.localScale.y / 1.5f,
                    instantiatedCharacters[i].transform.localScale.z / 1.5f
                    );

                instantiatedCharacters[i].transform.position = teamView.characterPositions[i].position;
            }
            
            return (teamView, instantiatedCharacters);
        }

        public void UpScaleTeam(TeamPresenter teamPresenter)
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

        public void DownScale(TeamPresenter teamPresenter)
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

        public void ResetTeamPosition(TeamPresenter teamPresenter)
        {
            for (int i = 0; i < teamPresenter.Model.CharactersCount; i++)
            {
                teamPresenter.Model.GetCharacterPresenter(i).View.transform.position = 
                    teamPresenter.View.characterPositions[i].position;
            }
        }

        private void OnDestroy(TeamView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}