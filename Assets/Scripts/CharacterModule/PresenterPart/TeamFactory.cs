using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class TeamFactory
    {
        public (TeamView, CharacterView[]) InstantiateTeam(TeamView teamPrefab, params CharacterView[] characters)
        {
            TeamView teamView = Object.Instantiate(teamPrefab, new Vector3(), Quaternion.identity);
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

        private void OnDestroy(TeamView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}