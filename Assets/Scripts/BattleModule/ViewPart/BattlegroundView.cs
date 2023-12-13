using System;
using CharacterModule.PresenterPart;
using CharacterModule.ViewPart;
using UnityEngine;

namespace BattleModule.ViewPart
{
    public class BattlegroundView : MonoBehaviour
    {
        public event Action<BattlegroundView> Destroy;

        [SerializeField] private Transform[] playerSidePositions;
        [SerializeField] private Transform[] enemiesSidePositions;
        [SerializeField] private Transform attackStepPosition;

        public void SetCharactersOnBattleground(PlayerTeamPresenter players, EnemyTeamPresenter enemies)
        {
            for (int i = 0; i < players.Model.CharactersCount; i++)
            {
                players.Model.GetCharacterPresenter(i).View.transform.position = playerSidePositions[i].position;
            }

            for (int i = 0; i < enemies.Model.CharactersCount; i++)
            {
                enemies.Model.GetCharacterPresenter(i).View.transform.position = enemiesSidePositions[i].position;
            }
        }

        public void ActivateBattleground()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateBattleground()
        {
            gameObject.SetActive(false);
        }

        public void DestroyView()
        {
            Destroy?.Invoke(this);
        }
    }
}