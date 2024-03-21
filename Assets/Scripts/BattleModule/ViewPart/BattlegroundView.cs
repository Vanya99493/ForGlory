using System;
using CharacterModule.PresenterPart;
using Infrastructure.CoroutineRunnerModule;
using UnityEngine;

namespace BattleModule.ViewPart
{
    public class BattlegroundView : MonoBehaviour, ICoroutineRunner
    {
        public event Action<BattlegroundView> Destroy;

        [SerializeField] private Transform[] playerSidePositions;
        [SerializeField] private Transform[] enemiesSidePositions;
        [SerializeField] private Transform attackStepPosition;

        private Vector3 _lastPosition;

        public void SetCharactersOnBattleground(PlayerTeamPresenter players, EnemyTeamPresenter enemies)
        {
            for (int i = 0; i < players.Model.CharactersCount; i++)
            {
                var player = players.Model.GetCharacterPresenter(i);
                if (player == null)
                    continue;
                player.View.transform.position = playerSidePositions[i].position;
            }

            for (int i = 0; i < enemies.Model.CharactersCount; i++)
            {
                var enemy = enemies.Model.GetCharacterPresenter(i);
                if (enemy == null)
                    continue;
                enemy.View.transform.position = enemiesSidePositions[i].position;
            }
        }

        public void SetAttackPosition(CharacterPresenter attackCharacter)
        {
            _lastPosition = attackCharacter.View.transform.position;
            attackCharacter.View.transform.position = attackStepPosition.position;
        }

        public void ResetAttackPosition(CharacterPresenter attackCharacter)
        {
            attackCharacter.View.transform.position = _lastPosition;
            _lastPosition = new Vector3(0, 0, 0);
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