using CharacterModule.ViewPart;
using UnityEngine;

namespace CharacterModule.PresenterPart
{
    public class TeamFactory
    {
        public TeamView InstantiateCharacter(TeamView teamPrefab)
        {
            TeamView teamView = Object.Instantiate(teamPrefab, new Vector3(), Quaternion.identity);
            teamView.Destroy += OnDestroy;
            return teamView;
        }

        private void OnDestroy(TeamView gameObject)
        {
            Object.Destroy(gameObject.gameObject);
        }
    }
}